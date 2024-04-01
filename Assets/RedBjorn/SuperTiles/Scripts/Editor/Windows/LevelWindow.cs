using RedBjorn.ProtoTiles;
using RedBjorn.SuperTiles.Editors.Level;
using RedBjorn.Utils;
using System;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace RedBjorn.SuperTiles.Editors
{
    public class LevelWindow : EditorWindowExtended
    {
        public SerializedObject SerializedObject;
        public TurnResolver CachedTurn;
        public SerializedObject SerializedTurn;
        public ActionRules CachedActions;
        public SerializedObject SerializedActions;
        public BattleFinishHandler CachedBattleFinish;
        public SerializedObject SerializedBattleFinish;
        public HealthConvertRules CachedHealth;
        public SerializedObject SerializedHealth;
        public RelationshipTable CachedRelationship;
        public SerializedObject SerializedRelationship;
        public ISubmenu Submenu;
        public LevelWindowSettings Settings;

        public Vector2 PlayersScroll;
        public string CachedRootFolderPath;
        public string CachedLevelName;
        public bool Dirty;

        public float CommonHeight = 100;
        public float MenuWidth = 140;
        public float Border = 4;
        public float WindowWidth;
        public float WindowHeight;
        public float ButtonHeight = 25f;
        public int Tab;

        public Color CommonColor => Settings.CommonColor;
        public Color MenuColor => Settings.MenuColor;
        public Color WorkAreaColor => Settings.WorkAreaColor;

        [SerializeField]
        LevelData CachedLevel;
        public LevelData Level
        {
            get
            {
                return CachedLevel;
            }
            set
            {
                if (CachedLevel != value)
                {
                    CachedLevel = value;
                    LevelUpdate();
                }
            }
        }

        [MenuItem("Tools/Red Bjorn/Editors/Level", priority = 220)]
        public static void DoShow()
        {
            DoShow(null);
        }

        public static void DoShow(LevelData level)
        {
            var window = (LevelWindow)EditorWindow.GetWindow(typeof(LevelWindow));
            window.minSize = new Vector2(400f, 500f);
            window.titleContent = new GUIContent("Level Editor");
            if (!level)
            {
                level = LevelData.Find(UnityEditor.SceneManagement.EditorSceneManager.GetActiveScene().name);
            }
            window.Level = level;
            window.Show();
        }

        void OnEnable()
        {
            Settings = LevelWindowSettings.Instance;
            LevelUpdate();
            Submenu = new Level.Submenus.Edit(this);

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

            Undo.RecordObject(this, "Level");
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

            if (Dirty)
            {
                Dirty = false;
                Repaint();
            }
            EditorGUIUtility.labelWidth = label;
        }

        void OnUndoRedoPerformed()
        {
            Repaint();
        }

        public void LevelUpdate()
        {
            if (Level)
            {
                SerializedObject = new SerializedObject(Level);
                CachedTurn = Level.Turn;
                SerializedTurn = CachedTurn ? new SerializedObject(CachedTurn) : null;
                CachedActions = Level.Actions;
                SerializedActions = CachedActions ? new SerializedObject(CachedActions) : null;
                CachedHealth = Level.Health;
                SerializedHealth = CachedHealth ? new SerializedObject(CachedHealth) : null;
                CachedBattleFinish = Level.BattleFinish;
                SerializedBattleFinish = CachedBattleFinish ? new SerializedObject(CachedBattleFinish) : null;
                CachedRelationship = Level.Relationship;
                SerializedRelationship = CachedRelationship ? new SerializedObject(CachedRelationship) : null;
            }
            else
            {
                DefaultValues();
            }
        }

        public void Duplicate()
        {
            var newLevel = LevelData.Duplicate(Level, CachedRootFolderPath, CachedLevelName);
            if (newLevel)
            {
                Level = newLevel;
            }
        }

        public void Create(string grid, string axis, string actionRulesType, string resolverType, string finisherType)
        {
            try
            {
                if (!Directory.Exists(CachedRootFolderPath))
                {
                    Directory.CreateDirectory(CachedRootFolderPath);
                }
                var mapSettings = MapWindowSettings.Instance;
                var map = MapSettings.Create(CachedRootFolderPath,
                                             CachedLevelName,
                                             (GridType)Enum.Parse(typeof(GridType), grid),
                                             (GridAxis)Enum.Parse(typeof(GridAxis), axis),
                                             mapSettings.Rules,
                                             mapSettings.CellBorder);
                var turn = TurnResolver.Create(CachedRootFolderPath, CachedLevelName, resolverType);
                var actions = ActionRules.Create(CachedRootFolderPath, CachedLevelName, actionRulesType);
                var health = HealthConvertRules.Create(CachedRootFolderPath, CachedLevelName);
                var newLevel = LevelData.Create(CachedRootFolderPath, CachedLevelName, map, turn, actions, health, Settings.TileIsItemAvailableRule);
                newLevel.BattleFinish = BattleFinishHandler.Create(CachedRootFolderPath, CachedLevelName, finisherType);
                newLevel.Relationship = RelationshipTable.Create(CachedRootFolderPath, CachedLevelName);
                if (newLevel.Relationship)
                {
                    newLevel.Relationship.SetDefault(newLevel.Players);
                }
                if (newLevel)
                {
                    Level = newLevel;
                }
            }
            catch (Exception e)
            {
                Log.E($"Level creation failed. {e.Message}");
            }
            finally
            {
                AssetDatabase.Refresh();
            }
        }

        public void ChangeTurnResolver(string typeName)
        {
            var creator = TurnResolver.GetCreatorType(typeName);
            var path = AssetDatabase.GetAssetPath(CachedTurn);
            AssetDatabase.DeleteAsset(path);
            Level.Turn = ((TurnResolverCreator)Activator.CreateInstance(creator)).Create(typeName);
            AssetDatabase.CreateAsset(Level.Turn, AssetDatabase.GenerateUniqueAssetPath(path));
            LevelUpdate();
            EditorUtility.SetDirty(Level.Turn);
            EditorUtility.SetDirty(Level);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        public void ChangeActionRules(string typeName)
        {
            var creator = ActionRules.GetCreatorType(typeName);
            var path = AssetDatabase.GetAssetPath(CachedActions);
            AssetDatabase.DeleteAsset(path);
            Level.Actions = ((ActionRulesCreator)Activator.CreateInstance(creator)).Create(typeName);
            AssetDatabase.CreateAsset(Level.Actions, AssetDatabase.GenerateUniqueAssetPath(path));
            LevelUpdate();
            EditorUtility.SetDirty(Level.Actions);
            EditorUtility.SetDirty(Level);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        public void ChangeBattleFinish(string typeName)
        {
            var creator = BattleFinishHandler.GetCreatorType(typeName);
            var path = AssetDatabase.GetAssetPath(CachedBattleFinish);
            AssetDatabase.DeleteAsset(path);
            Level.BattleFinish = ((BattleFinishHandlerCreator)Activator.CreateInstance(creator)).Create(typeName);
            AssetDatabase.CreateAsset(Level.BattleFinish, AssetDatabase.GenerateUniqueAssetPath(path));
            LevelUpdate();
            EditorUtility.SetDirty(Level.BattleFinish);
            EditorUtility.SetDirty(Level);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        void DefaultValues()
        {
            SerializedObject = null;
            SerializedTurn = null;
            SerializedActions = null;
            SerializedHealth = null;
            SerializedBattleFinish = null;
            SerializedRelationship = null;
            CachedLevel = null;
            CachedTurn = null;
            CachedActions = null;
            CachedHealth = null;
            CachedBattleFinish = null;
            CachedRelationship = null;
        }
    }
}
