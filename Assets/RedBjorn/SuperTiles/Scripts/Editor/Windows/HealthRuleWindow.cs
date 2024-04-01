using RedBjorn.SuperTiles.Editors.HealthRule;
using RedBjorn.SuperTiles.Editors.HealthRule.Submenus.Tabs.Converters;
using RedBjorn.SuperTiles.Health;
using RedBjorn.Utils;
using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace RedBjorn.SuperTiles.Editors
{
    public class HealthRuleWindow : EditorWindowExtended
    {
        public SerializedObject SerializedObject;
        public SerializedObject SerializedCondition;
        public Health.Condition CachedCondition;
        public SerializedObject SerializedConverter;
        public ValueConverter CachedConverter;
        public IConverterSubmenu ConverterSubmenu;
        public ISubmenu Submenu;
        public HealthRuleWindowSettings Settings;

        public Vector2 ScrollPos;
        public int ConditionIndex;
        public string[] Conditions;
        public int ConverterIndex;
        public string[] Converters;

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

        public ConvertRule CachedRule;
        public ConvertRule Rule
        {
            get
            {
                return CachedRule;
            }
            set
            {
                if (CachedRule != value)
                {
                    CachedRule = value;
                    RuleUpdate();
                }
            }
        }

        public string CachedRuleName;
        public string RuleName { get { return string.IsNullOrEmpty(CachedRuleName) ? Settings.DefaultRuleName : CachedRuleName; } }

        public string CachedRootFolderPath;
        public string RuleFolderPath
        {
            get
            {
                ValidateName();
                return Path.Combine(CachedRootFolderPath, RuleName);
            }
        }

        public string RuleAssetPath
        {
            get
            {
                ValidateName();
                return Path.Combine(RuleFolderPath, string.Concat(RuleName, FileFormat.Asset));
            }
        }

        public string RuleConverterName { get { return string.Concat(RuleName, Settings.ConverterSuffix); } }
        public string RuleConverterAssetPath { get { return Path.Combine(RuleFolderPath, string.Concat(RuleConverterName, FileFormat.Asset)); } }
        public string RuleConditionName { get { return string.Concat(RuleName, Settings.ConditionSuffix); } }
        public string RuleConditionAssetPath { get { return Path.Combine(RuleFolderPath, string.Concat(RuleConditionName, FileFormat.Asset)); } }

        [MenuItem("Tools/Red Bjorn/Editors/Health Rule", priority = 240)]
        public static void DoShow()
        {
            DoShow(null);
        }

        public static void DoShow(ConvertRule rule)
        {
            var window = (HealthRuleWindow)GetWindow(typeof(HealthRuleWindow));
            window.minSize = new Vector2(400f, 500f);
            window.titleContent = new GUIContent("Health Rule Editor");
            window.Rule = rule;
            window.Show();
        }

        void OnEnable()
        {
            Settings = HealthRuleWindowSettings.Instance;
            Submenu = new HealthRule.Submenus.Edit();
            ConverterSubmenu = new HealthRule.Submenus.Tabs.Converters.Edit();
            RuleUpdate();
            if (string.IsNullOrEmpty(CachedRootFolderPath))
            {
                CachedRootFolderPath = Settings.DefaultRuleFolder;
            }
            if (string.IsNullOrEmpty(CachedRuleName))
            {
                CachedRuleName = Settings.DefaultRuleName;
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

            Undo.RecordObject(this, "Rule");
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
            SerializedConverter = null;
            SerializedCondition = null;
            CachedCondition = null;
            CachedConverter = null;
        }

        void OnUndoRedoPerformed()
        {
            Repaint();
        }

        public void RuleUpdate()
        {
            if (Rule)
            {
                CachedCondition = Rule.Condition;
                CachedConverter = Rule.Converter;
                SerializedObject = new SerializedObject(Rule);
                if (CachedConverter != null)
                {
                    SerializedConverter = new SerializedObject(CachedConverter);
                }
                if (CachedCondition != null)
                {
                    SerializedCondition = new SerializedObject(CachedCondition);
                }
            }
            else
            {
                DefaultValues();
            }
        }

        public void RuleCreateNew(string conditionType, string converterType)
        {
            try
            {
                var typeCondition = typeof(Health.Condition);
                var conditionName = string.Concat(typeCondition.Namespace, ".", nameof(Health.Conditions), ".", conditionType, "Creator");
                var conditionCreator = typeCondition.Assembly.GetType(conditionName);
                if (conditionCreator == null)
                {
                    Log.E($"No creator class ({conditionName}) for Condition ({conditionType})");
                    return;
                }

                var typeConverter = typeof(ValueConverter);
                var converterName = string.Concat(typeConverter.Namespace, ".", nameof(Health.ValueConverters), ".", converterType, "Creator");
                var converterCreator = typeConverter.Assembly.GetType(converterName);
                if (converterCreator == null)
                {
                    Log.E($"No creator class ({converterName}) for target Converter ({converterType})");
                    return;
                }

                ValidateDirectory();

                var item = CreateInstance<ConvertRule>();
                var uniqueRulePath = AssetDatabase.GenerateUniqueAssetPath(RuleAssetPath);
                AssetDatabase.CreateAsset(item, uniqueRulePath);

                var converter = ((ValueConverterCreator)Activator.CreateInstance(converterCreator)).Create(item, converterType);
                item.Converter = converter;
                AssetDatabase.CreateAsset(converter, AssetDatabase.GenerateUniqueAssetPath(RuleConverterAssetPath));
                EditorUtility.SetDirty(converter);

                var condition = ((ConditionCreator)Activator.CreateInstance(conditionCreator)).Create(item, conditionType);
                item.Condition = condition;
                AssetDatabase.CreateAsset(condition, AssetDatabase.GenerateUniqueAssetPath(RuleConditionAssetPath));
                EditorUtility.SetDirty(condition);

                EditorUtility.SetDirty(item);

                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();

                Log.I($"New item was created at path: {uniqueRulePath}");

                Rule = item;
            }
            catch (Exception e)
            {
                Log.E(e);
            }
        }

        public void RuleDuplicate()
        {
            if (Rule)
            {
                try
                {
                    ValidateDirectory();

                    var rulePath = AssetDatabase.GenerateUniqueAssetPath(RuleAssetPath);
                    var newRule = Object.Instantiate(Rule);
                    AssetDatabase.CreateAsset(newRule, rulePath);

                    if (Rule.Condition)
                    {
                        var condition = Object.Instantiate(Rule.Condition);
                        newRule.Condition = condition;
                        AssetDatabase.CreateAsset(condition, AssetDatabase.GenerateUniqueAssetPath(RuleConditionAssetPath));
                        EditorUtility.SetDirty(condition);
                    }

                    if (Rule.Converter)
                    {
                        var converter = Object.Instantiate(Rule.Converter);
                        newRule.Converter = converter;
                        AssetDatabase.CreateAsset(converter, AssetDatabase.GenerateUniqueAssetPath(RuleConverterAssetPath));
                        EditorUtility.SetDirty(converter);
                    }

                    EditorUtility.SetDirty(newRule);
                    AssetDatabase.SaveAssets();
                    AssetDatabase.Refresh();
                    Log.I($"Rule was duplicated at path: {rulePath}");
                    Rule = newRule;
                }
                catch (Exception e)
                {
                    Log.E(e);
                }
            }
        }

        public void RuleChangeConverter(string typeName)
        {
            var creator = ValueConverter.GetCreatorType(typeName);
            var path = AssetDatabase.GetAssetPath(CachedConverter);
            AssetDatabase.DeleteAsset(path);
            Rule.Converter = ((ValueConverterCreator)Activator.CreateInstance(creator)).Create(Rule, typeName);
            AssetDatabase.CreateAsset(Rule.Converter, AssetDatabase.GenerateUniqueAssetPath(path));
            RuleUpdate();
            EditorUtility.SetDirty(Rule.Converter);
            EditorUtility.SetDirty(Rule);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        public void RuleChangeCondition(string typeName)
        {
            var creator = Condition.GetCreatorType(typeName);
            var path = AssetDatabase.GetAssetPath(CachedCondition);
            AssetDatabase.DeleteAsset(path);
            Rule.Condition = ((ConditionCreator)Activator.CreateInstance(creator)).Create(Rule, typeName);
            AssetDatabase.CreateAsset(Rule.Condition, AssetDatabase.GenerateUniqueAssetPath(path));
            RuleUpdate();
            EditorUtility.SetDirty(Rule.Condition);
            EditorUtility.SetDirty(Rule);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        public void UpdatePopUp()
        {
            Conditions = typeof(Condition).Assembly.GetTypes()
                                                 .Where(t => t.IsClass && !t.IsAbstract && t.IsSubclassOf(typeof(Condition)))
                                                 .Select(t => t.Name)
                                                 .ToArray();
            ConditionIndex = 0;
            Converters = typeof(ValueConverter).Assembly.GetTypes()
                                                           .Where(t => t.IsClass && !t.IsAbstract && t.IsSubclassOf(typeof(ValueConverter)))
                                                           .Select(t => t.Name)
                                                           .ToArray();
            ConverterIndex = 0;
        }

        void ValidateName()
        {
            if (string.IsNullOrEmpty(RuleName))
            {
                Log.E("Can't create item with empty name. Default name will be used (Rule)");
                CachedRuleName = "Rule";
            }
        }

        void ValidateDirectory()
        {
            if (!Directory.Exists(RuleFolderPath))
            {
                Directory.CreateDirectory(RuleFolderPath);
            }
        }
    }
}