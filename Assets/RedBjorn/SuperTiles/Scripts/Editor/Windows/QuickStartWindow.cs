using RedBjorn.SuperTiles.Editors.QuickStart;
using RedBjorn.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace RedBjorn.SuperTiles.Editors
{
    [Serializable]
    public class QuickStartWindow : EditorWindowExtended
    {
        public List<ISubmenu> Submenu;
        public TeamTag CachedTeam;
        public UnitData CachedUnit;
        public UnitAiData CachedUnitAi;
        public QuickStartWindowSettings Settings;

        public int Tab;
        public int GridType;
        public int Edge;
        public string SceneName;
        public string FolderPath;

        public float TabsHeight = 20;
        public float NavigationWidth = 200;
        public float NavigationHeight = 40;
        public float Border = 4;

        public float WindowHeight;
        public float WindowWidth;

        public Rect Tabs;
        public Rect Common;
        public Rect CommonContent;
        public Rect Navigation;
        public LevelData Level;

        public string UnitFolderPath { get { return Path.Combine(FolderPath, "Units"); } }
        public string UnitAiFolderPath { get { return Path.Combine(FolderPath, "Ai"); } }
        public string ScenePath { get { return Path.Combine(FolderPath, string.Concat(SceneName, FileFormat.Scene)); } }

        [MenuItem("Tools/Red Bjorn/Editors/Quick Start", priority = 160)]
        static void DoShow()
        {
            var window = (QuickStartWindow)GetWindow(typeof(QuickStartWindow));
            window.titleContent = new GUIContent("Quick Start");
            window.minSize = new Vector2(200f, 350f);
            window.CenterOnMainWin();
            window.Show();
        }

        void OnEnable()
        {
            Settings = QuickStartWindowSettings.Instance;
            DefaultValues();
            Undo.undoRedoPerformed += OnUndoRedoPerformed;
        }

        void OnDisable()
        {
            Undo.undoRedoPerformed -= OnUndoRedoPerformed;
            Undo.ClearAll();
        }

        void OnGUI()
        {
            var scale = EditorGUIUtility.pixelsPerPoint;
            WindowWidth = Screen.width / scale;
            WindowHeight = Screen.height / scale;

            Undo.RecordObject(this, "Quick Start");
            var labelWidth = EditorGUIUtility.labelWidth;
            EditorGUIUtility.labelWidth = 90;
            var gui = GUI.enabled;
            Submenu[Tab].Show(this);
            EditorGUIUtility.labelWidth = labelWidth;
            GUI.enabled = gui;
        }

        void DefaultValues()
        {
            Edge = 1;

            Submenu = new List<ISubmenu>()
            {
                new SceneSubmenu(),
                new Multiple(new List<ISubmenu> { new MapSubmenu(), new UnitSpawnSubmenu(), new LevelSubmenu()})
            };

            if (string.IsNullOrEmpty(SceneName))
            {
                SceneName = Settings.SceneNameDefault;
            }
            if (string.IsNullOrEmpty(FolderPath))
            {
                FolderPath = Settings.FolderRootDefault + "/" + SceneName;
            }

            if (!CachedTeam)
            {
                CachedTeam = Tag.Find<TeamTag>("1");
            }
        }

        void OnUndoRedoPerformed()
        {
            Repaint();
        }
    }
}