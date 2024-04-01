using RedBjorn.SuperTiles.Editors.Item;
using RedBjorn.SuperTiles.Items;
using RedBjorn.Utils;
using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace RedBjorn.SuperTiles.Editors
{
    public class ItemWindow : EditorWindowExtended
    {
        public SerializedObject SerializedObject;
        public SerializedObject SerializedSelector;
        public SerializedObject SerializedHandler;
        public TargetSelector CachedSelector;
        public ActionHandler CachedHandler;
        public IHandlerSubmenu HandlerSubmenu;
        public ISelectorSubmenu SelectorSubmenu;
        public ISubmenu Submenu;
        public ItemWindowSettings Settings;

        public Vector2 ScrollPos;
        public int TargetSelectorIndex;
        public string[] TargetSelectors;
        public int ActionHandlerIndex;
        public string[] ActionHandlers;

        public float CommonHeight = 60;
        public float MenuWidth = 150;
        public float Border = 4;
        public float WindowWidth;
        public float WindowHeight;

        public Color CommonColor => Settings.CommonColor;
        public Color MenuColor => Settings.MenuColor;
        public Color WorkAreaColor => Settings.WorkAreaColor;

        public int CachedTab;
        public int Tab
        {
            get
            {
                return CachedTab;
            }
            set
            {
                CachedTab = value;
            }
        }

        public ItemData CachedItem;
        public ItemData Item
        {
            get
            {
                return CachedItem;
            }
            set
            {
                if (CachedItem != value)
                {
                    CachedItem = value;
                    ItemUpdate();
                }
            }
        }

        public string CachedItemName;
        public string ItemName { get { return string.IsNullOrEmpty(CachedItemName) ? Settings.DefaultItemName : CachedItemName; } }

        public string CachedRootFolderPath;
        public string ItemFolderPath
        {
            get
            {
                ValidateName();
                return Path.Combine(CachedRootFolderPath, ItemName);
            }
        }

        public string ItemAssetPath
        {
            get
            {
                ValidateName();
                return Path.Combine(ItemFolderPath, string.Concat(ItemName, FileFormat.Asset));
            }
        }

        public string ItemHandlerName { get { return string.Concat(ItemName, Settings.HandlerItemSuffix); } }
        public string ItemHandlerAssetPath { get { return Path.Combine(ItemFolderPath, string.Concat(ItemHandlerName, FileFormat.Asset)); } }
        public string ItemTargetSelectorName { get { return string.Concat(ItemName, Settings.TargetSelectorItemSuffix); } }
        public string ItemSelectorAssetPath { get { return Path.Combine(ItemFolderPath, string.Concat(ItemTargetSelectorName, FileFormat.Asset)); } }

        [MenuItem("Tools/Red Bjorn/Editors/Item", priority = 230)]
        public static void DoShow()
        {
            DoShow(null, null);
        }

        public static void DoShow(ItemData item)
        {
            DoShow(item, "Assets");
        }

        public static void DoShow(ItemData item, string folderPath)
        {
            var window = (ItemWindow)GetWindow(typeof(ItemWindow));
            window.minSize = new Vector2(400f, 500f);
            window.titleContent = new GUIContent("Item Editor");
            window.CachedRootFolderPath = folderPath;
            window.Item = item;
            window.Show();
        }

        void OnEnable()
        {
            Settings = ItemWindowSettings.Instance;
            Submenu = new Edit(this);
            SelectorSubmenu = new Item.Selector.Edit();
            HandlerSubmenu = new Item.Handler.Edit();
            ItemUpdate();
            if (string.IsNullOrEmpty(CachedRootFolderPath))
            {
                CachedRootFolderPath = Settings.DefaultItemFolder;
            }
            if (string.IsNullOrEmpty(CachedItemName))
            {
                CachedItemName = Settings.DefaultItemName;
            }
            Undo.undoRedoPerformed += OnUndoRedoPerformed;
        }

        void OnDisable()
        {
            Undo.undoRedoPerformed -= OnUndoRedoPerformed;
        }

        void OnGUI()
        {
            var scale = EditorGUIUtility.pixelsPerPoint;
            WindowWidth = Screen.width / scale;
            WindowHeight = Screen.height / scale;

            Undo.RecordObject(this, "Item");
            var gui = GUI.enabled;
            var label = EditorGUIUtility.labelWidth;
            EditorGUIUtility.labelWidth = 90f;

            Submenu.Draw(this);

            if (SerializedObject != null)
            {
                if (!SerializedObject.targetObject)
                {
                    DefaultValues();
                }
                if (SerializedObject != null)
                {
                    SerializedObject.ApplyModifiedProperties();
                }
            }

            EditorGUIUtility.labelWidth = label;
            GUI.enabled = gui;
        }

        void DefaultValues()
        {
            SerializedObject = null;
            SerializedHandler = null;
            SerializedSelector = null;
            CachedSelector = null;
            CachedHandler = null;
        }

        void OnUndoRedoPerformed()
        {
            Repaint();
        }

        public void ItemUpdate()
        {
            if (Item)
            {
                CachedSelector = Item.Selector;
                CachedHandler = Item.ActionHandler;
                SerializedObject = new SerializedObject(Item);
                if (CachedHandler != null)
                {
                    SerializedHandler = new SerializedObject(CachedHandler);
                }
                if (CachedSelector != null)
                {
                    SerializedSelector = new SerializedObject(CachedSelector);
                }
            }
            else
            {
                DefaultValues();
            }
        }

        public void ItemCreateNew(string selectorType, string actionType)
        {
            try
            {
                var typeSelector = typeof(TargetSelector);
                var selectorName = string.Concat(typeSelector.Namespace, ".", nameof(Items.TargetSelectors), ".", selectorType, "Creator");
                var selectorCreator = typeSelector.Assembly.GetType(selectorName);
                if (selectorCreator == null)
                {
                    Log.E($"No creator class ({selectorName}) for action handler ({selectorType})");
                    return;
                }

                var typeHandler = typeof(ActionHandler);
                var handlername = string.Concat(typeHandler.Namespace, ".", nameof(Items.ActionHandlers), ".", actionType, "Creator");
                var actionCreator = typeHandler.Assembly.GetType(handlername);
                if (actionCreator == null)
                {
                    Log.E($"No creator class ({handlername}) for target selector ({actionType})");
                    return;
                }

                ValidateDirectory();

                var item = CreateInstance<ItemData>();
                item.Caption = ItemName;
                item.Visual = new ItemData.VisualConfig
                {
                    Color = Color.red,
                    IconSmall = Settings.ItemIconDefault,
                };
                var uniqueItemPath = AssetDatabase.GenerateUniqueAssetPath(ItemAssetPath);
                AssetDatabase.CreateAsset(item, uniqueItemPath);

                var handler = ((ActionHandlerCreator)Activator.CreateInstance(actionCreator)).Create(item, actionType);
                item.ActionHandler = handler;
                AssetDatabase.CreateAsset(handler, AssetDatabase.GenerateUniqueAssetPath(ItemHandlerAssetPath));
                EditorUtility.SetDirty(handler);

                var selector = ((TargetSelectorCreator)Activator.CreateInstance(selectorCreator)).Create(item, selectorType);
                item.Selector = selector;
                AssetDatabase.CreateAsset(selector, AssetDatabase.GenerateUniqueAssetPath(ItemSelectorAssetPath));
                EditorUtility.SetDirty(selector);

                EditorUtility.SetDirty(item);

                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();

                Log.I($"New item was created at path: {uniqueItemPath}");

                Item = item;
            }
            catch (Exception e)
            {
                Log.E(e);
            }
        }

        public void ItemDuplicate()
        {
            if (Item)
            {
                try
                {
                    ValidateDirectory();
                    var newItem = Object.Instantiate(Item);
                    var itemPath = AssetDatabase.GenerateUniqueAssetPath(ItemAssetPath);
                    AssetDatabase.CreateAsset(newItem, itemPath);

                    if (Item.ActionHandler)
                    {
                        var newHandler = Object.Instantiate(Item.ActionHandler);
                        newItem.ActionHandler = newHandler;
                        AssetDatabase.CreateAsset(newHandler, AssetDatabase.GenerateUniqueAssetPath(ItemHandlerAssetPath));
                        EditorUtility.SetDirty(newHandler);
                    }

                    if (Item.Selector)
                    {
                        var newSelector = Object.Instantiate(Item.Selector);
                        newItem.Selector = newSelector;
                        AssetDatabase.CreateAsset(newSelector, AssetDatabase.GenerateUniqueAssetPath(ItemSelectorAssetPath));
                        EditorUtility.SetDirty(newSelector);
                    }

                    EditorUtility.SetDirty(newItem);
                    AssetDatabase.SaveAssets();
                    AssetDatabase.Refresh();
                    Log.I($"Item was duplicated at path: {itemPath}");
                    Item = newItem;
                }
                catch (Exception e)
                {
                    Log.E(e);
                }
            }
        }

        public void ItemChangeActionHandler(string typeName)
        {
            var creator = ActionHandler.GetCreatorType(typeName);
            var path = AssetDatabase.GetAssetPath(CachedHandler);
            AssetDatabase.DeleteAsset(path);
            Item.ActionHandler = ((ActionHandlerCreator)Activator.CreateInstance(creator)).Create(Item, typeName);
            AssetDatabase.CreateAsset(Item.ActionHandler, AssetDatabase.GenerateUniqueAssetPath(path));
            ItemUpdate();
            EditorUtility.SetDirty(Item.ActionHandler);
            EditorUtility.SetDirty(Item);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        public void ItemChangeTargetSelector(string typeName)
        {
            var creator = TargetSelector.GetCreatorType(typeName);
            var path = AssetDatabase.GetAssetPath(CachedSelector);
            AssetDatabase.DeleteAsset(path);
            Item.Selector = ((TargetSelectorCreator)Activator.CreateInstance(creator)).Create(Item, typeName);
            AssetDatabase.CreateAsset(Item.Selector, AssetDatabase.GenerateUniqueAssetPath(path));
            ItemUpdate();
            EditorUtility.SetDirty(Item.Selector);
            EditorUtility.SetDirty(Item);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        public void UpdatePopUp()
        {
            TargetSelectorIndex = 0;
            ActionHandlerIndex = 0;
            TargetSelectors = typeof(TargetSelector).Assembly.GetTypes()
                                                 .Where(t => t.IsClass && t.IsSubclassOf(typeof(TargetSelector)))
                                                 .Select(t => t.Name)
                                                 .ToArray();
            ActionHandlers = typeof(ActionHandler).Assembly.GetTypes()
                                                           .Where(t => t.IsClass && t.IsSubclassOf(typeof(ActionHandler)))
                                                           .Select(t => t.Name)
                                                           .ToArray();
        }

        void ValidateName()
        {
            if (string.IsNullOrEmpty(ItemName))
            {
                Log.E("Can't create item with empty name. Default name will be used (Item)");
                CachedItemName = "Item";
            }
        }

        void ValidateDirectory()
        {
            if (!Directory.Exists(ItemFolderPath))
            {
                Directory.CreateDirectory(ItemFolderPath);
            }
        }
    }
}