using RedBjorn.SuperTiles.Editors.Effect;
using RedBjorn.SuperTiles.Editors.Effect.Submenus;
using RedBjorn.SuperTiles.Effects;
using RedBjorn.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace RedBjorn.SuperTiles.Editors
{
    [Serializable]
    public class EffectHandlerInfo
    {
        public EffectHandler Effect;
        public SerializedObject Serialized;
    }

    public class EffectWindow : EditorWindowExtended
    {
        public SerializedObject SerializedObject;
        public EffectHandlerInfo OnAdded;
        public EffectHandlerInfo OnRemoved;
        public EffectHandlerInfo Handler;
        public ISubmenu Submenu;
        public EffectWindowSettings Settings;

        public Vector2 ScrollPos;
        public int EffectOnAddedIndex;
        public int EffectHandlerIndex;
        public int EffectOnRemovedIndex;
        public string[] EffectHandlers;

        public float CommonHeight = 60;
        public float MenuWidth = 150;
        public float Border = 4;
        public float WindowWidth;
        public float WindowHeight;
        public int Tab;

        public Color CommonColor => Settings.CommonColor;
        public Color MenuColor => Settings.MenuColor;
        public Color WorkAreaColor => Settings.WorkAreaColor;

        public EffectData CachedEffect;
        public EffectData Effect
        {
            get
            {
                return CachedEffect;
            }
            set
            {
                if (CachedEffect != value)
                {
                    CachedEffect = value;
                    EffectUpdate();
                }
            }
        }

        public string CachedEffectName;
        public string EffectName { get { return string.IsNullOrEmpty(CachedEffectName) ? Settings.DefaultEffectName : CachedEffectName; } }
        public string CachedRootFolderPath;
        public string EffectFolderPath
        {
            get
            {
                ValidateName();
                return Path.Combine(CachedRootFolderPath, EffectName);
            }
        }
        public string EffectAssetPath
        {
            get
            {
                ValidateName();
                return Path.Combine(EffectFolderPath, string.Concat(EffectName, FileFormat.Asset));
            }
        }
        public string EffectOnAddedName { get { return string.Concat(EffectName, Settings.OnAddedSuffix); } }
        public string EffectOnAddedAssetPath { get { return Path.Combine(EffectFolderPath, string.Concat(EffectOnAddedName, FileFormat.Asset)); } }
        public string EffectHandlerName { get { return string.Concat(EffectName, Settings.HandlerSuffix); } }
        public string EffectHandlerAssetPath { get { return Path.Combine(EffectFolderPath, string.Concat(EffectHandlerName, FileFormat.Asset)); } }
        public string EffectOnRemovedName { get { return string.Concat(EffectName, Settings.OnRemovedSuffix); } }
        public string EffectOnRemovedAssetPath { get { return Path.Combine(EffectFolderPath, string.Concat(EffectOnRemovedName, FileFormat.Asset)); } }

        public const string HandlerNone = "None";

        [MenuItem("Tools/Red Bjorn/Editors/Effect", priority = 240)]
        public static void DoShow()
        {
            DoShow(null, null);
        }

        public static void DoShow(EffectData item, string folderPath)
        {
            var window = (EffectWindow)GetWindow(typeof(EffectWindow));
            window.minSize = new Vector2(400f, 500f);
            window.titleContent = new GUIContent("Effect Editor");
            window.CachedRootFolderPath = folderPath;
            window.Effect = item;
            window.Show();
        }

        void OnEnable()
        {
            Settings = EffectWindowSettings.Instance;
            OnAdded = new EffectHandlerInfo();
            OnRemoved = new EffectHandlerInfo();
            Handler = new EffectHandlerInfo();
            Submenu = new Edit(this);
            EffectUpdate();
            if (string.IsNullOrEmpty(CachedRootFolderPath))
            {
                CachedRootFolderPath = Settings.DefaultEffectFolder;
            }
            if (string.IsNullOrEmpty(CachedEffectName))
            {
                CachedEffectName = Settings.DefaultEffectName;
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

            Undo.RecordObject(this, "Effect");
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
        }

        public void EffectCreateNew(string onAddedType, string handlerType, string onRemovedType)
        {
            try
            {
                ValidateDirectory();

                var effect = CreateInstance<EffectData>();
                effect.Caption = EffectName;
                effect.FxAddShow = true;
                effect.FxRemoveShow = true;
                var uniqueEffectPath = AssetDatabase.GenerateUniqueAssetPath(EffectAssetPath);
                AssetDatabase.CreateAsset(effect, uniqueEffectPath);

                Type addedCreator = null;
                if (onAddedType != HandlerNone)
                {
                    addedCreator = EffectHandler.GetCreatorType(onAddedType);
                }
                if (addedCreator != null)
                {
                    var onAdded = ((EffectHandlerCreator)Activator.CreateInstance(addedCreator)).Create(effect, onAddedType);
                    effect.OnAdded = onAdded;
                    AssetDatabase.CreateAsset(onAdded, AssetDatabase.GenerateUniqueAssetPath(EffectOnAddedAssetPath));
                    EditorUtility.SetDirty(onAdded);
                }

                Type handlerCreator = null;
                if (handlerType != HandlerNone)
                {
                    handlerCreator = EffectHandler.GetCreatorType(handlerType);
                }
                if (handlerCreator != null)
                {
                    var handler = ((EffectHandlerCreator)Activator.CreateInstance(handlerCreator)).Create(effect, handlerType);
                    effect.Handler = handler;
                    AssetDatabase.CreateAsset(handler, AssetDatabase.GenerateUniqueAssetPath(EffectHandlerAssetPath));
                    EditorUtility.SetDirty(handler);
                }

                Type removeCreator = null;
                if (onRemovedType != HandlerNone)
                {
                    removeCreator = EffectHandler.GetCreatorType(onRemovedType);
                }
                if (removeCreator != null)
                {
                    var onRemoved = ((EffectHandlerCreator)Activator.CreateInstance(removeCreator)).Create(effect, onRemovedType);
                    effect.OnRemoved = onRemoved;
                    AssetDatabase.CreateAsset(onRemoved, AssetDatabase.GenerateUniqueAssetPath(EffectOnRemovedAssetPath));
                    EditorUtility.SetDirty(onRemoved);
                }

                EditorUtility.SetDirty(effect);

                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();

                Log.I($"New item was created at path: {uniqueEffectPath}");

                Effect = effect;
            }
            catch (Exception e)
            {
                Log.E(e);
            }
        }

        public void EffectDuplicate()
        {
            if (Effect)
            {
                try
                {
                    ValidateDirectory();
                    if (Effect)
                    {
                        var newEffect = Object.Instantiate(Effect);

                        var uniqueEffectPath = AssetDatabase.GenerateUniqueAssetPath(EffectAssetPath);
                        AssetDatabase.CreateAsset(newEffect, uniqueEffectPath);

                        if (Effect.OnAdded)
                        {
                            var onAdded = Object.Instantiate(Effect.OnAdded);
                            AssetDatabase.CreateAsset(onAdded, AssetDatabase.GenerateUniqueAssetPath(EffectOnAddedAssetPath));
                            newEffect.OnAdded = onAdded;
                            EditorUtility.SetDirty(onAdded);
                        }

                        if (Effect.Handler)
                        {
                            var handler = Object.Instantiate(Effect.Handler);
                            AssetDatabase.CreateAsset(handler, AssetDatabase.GenerateUniqueAssetPath(EffectHandlerAssetPath));
                            newEffect.Handler = handler;
                            EditorUtility.SetDirty(handler);
                        }

                        if (Effect.OnRemoved)
                        {
                            var onRemoved = Object.Instantiate(Effect.OnRemoved);
                            AssetDatabase.CreateAsset(onRemoved, AssetDatabase.GenerateUniqueAssetPath(EffectOnRemovedAssetPath));
                            newEffect.OnRemoved = onRemoved;
                            EditorUtility.SetDirty(onRemoved);
                        }

                        EditorUtility.SetDirty(newEffect);
                        AssetDatabase.SaveAssets();
                        AssetDatabase.Refresh();
                        Log.I($"Effect was duplicated at path: {uniqueEffectPath}");
                        Effect = newEffect;
                    }
                }
                catch (Exception e)
                {
                    Log.E(e);
                }
            }
        }

        public void EffectHandlerChangeType(EffectHandlerInfo info, string handlerType, string suffix = null)
        {
            var effectPath = AssetDatabase.GetAssetPath(Effect);
            var path = Path.Combine(Path.GetDirectoryName(effectPath), string.Concat(Path.GetFileNameWithoutExtension(effectPath), suffix, Path.GetExtension(effectPath)));
            if (info.Effect)
            {
                path = AssetDatabase.GetAssetPath(info.Effect);
                AssetDatabase.DeleteAsset(path);
            }

            Type creator = null;
            if (handlerType != HandlerNone)
            {
                creator = EffectHandler.GetCreatorType(handlerType);
            }
            if (creator != null)
            {
                info.Effect = ((EffectHandlerCreator)Activator.CreateInstance(creator)).Create(Effect, handlerType);
                AssetDatabase.CreateAsset(info.Effect, AssetDatabase.GenerateUniqueAssetPath(path));
            }
            else
            {
                info.Effect = null;
            }

            EffectSave();
            if (info.Effect)
            {
                EditorUtility.SetDirty(info.Effect);
            }
            EditorUtility.SetDirty(Effect);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        public void UpdatePopUp()
        {
            EffectHandlers = new List<string> { HandlerNone }.Concat(typeof(EffectHandler).Assembly.GetTypes()
                                                 .Where(t => t.IsClass && t.IsSubclassOf(typeof(EffectHandler)))
                                                 .Select(t => t.Name))
                                                 .ToArray();
            EffectOnAddedIndex = 0;
            EffectHandlerIndex = 0;
            EffectOnRemovedIndex = 0;
        }

        void ValidateDirectory()
        {
            if (!Directory.Exists(EffectFolderPath))
            {
                Directory.CreateDirectory(EffectFolderPath);
            }
        }

        void OnUndoRedoPerformed()
        {
            Repaint();
        }

        void DefaultValues()
        {
            SerializedObject = null;
            OnAdded.Serialized = null;
            OnAdded.Effect = null;
            OnRemoved.Serialized = null;
            OnRemoved.Effect = null;
            Handler.Serialized = null;
            Handler.Effect = null;
        }

        void ValidateName()
        {
            if (string.IsNullOrEmpty(EffectName))
            {
                Log.E("Can't create item with empty name. Default name will be used (Effect)");
                CachedEffectName = "Effect";
            }
        }

        void EffectSave()
        {
            Effect.OnAdded = OnAdded.Effect;
            Effect.OnRemoved = OnRemoved.Effect;
            Effect.Handler = Handler.Effect;
            EffectUpdate();
        }

        void EffectUpdate()
        {
            if (Effect)
            {
                SerializedObject = new SerializedObject(Effect);

                OnAdded.Effect = Effect.OnAdded;
                OnAdded.Serialized = OnAdded.Effect ? new SerializedObject(OnAdded.Effect) : null;

                Handler.Effect = Effect.Handler;
                Handler.Serialized = Handler.Effect ? new SerializedObject(Handler.Effect) : null;

                OnRemoved.Effect = Effect.OnRemoved;
                OnRemoved.Serialized = OnRemoved.Effect ? new SerializedObject(OnRemoved.Effect) : null;
            }
            else
            {
                DefaultValues();
            }
        }
    }
}