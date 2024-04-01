using RedBjorn.SuperTiles.Editors.Unit;
using RedBjorn.SuperTiles.Editors.Unit.Submenus;
using RedBjorn.Utils;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace RedBjorn.SuperTiles.Editors
{
    public class UnitWindow : EditorWindowExtended
    {
        public SerializedObject SerializedObject;
        public SerializedObject SerializedTurn;
        public TurnResolver CachedTurn;
        public ISubmenu Submenu;
        public UnitWindowSettings Settings;
        public bool Dirty;

        public string CachedRootFolderPath;
        public string CachedUnitName;

        public float CommonHeight = 60;
        public float MenuWidth = 150;
        public float Border = 4;
        public float WindowWidth;
        public float WindowHeight;

        [SerializeField]
        UnitData CachedUnit;
        public UnitData Unit
        {
            get
            {
                return CachedUnit;
            }
            set
            {
                if (CachedUnit != value)
                {
                    CachedUnit = value;
                    OnChangedItem();
                }
            }
        }

        [SerializeField]
        int CachedTab;
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

        [MenuItem("Tools/Red Bjorn/Editors/Unit", priority = 225)]
        public static void DoShow()
        {
            DoShow(null);
        }

        public static void DoShow(UnitData unit)
        {
            var window = (UnitWindow)EditorWindow.GetWindow(typeof(UnitWindow));
            window.minSize = new Vector2(400f, 500f);
            window.Unit = unit;
            window.titleContent = new GUIContent("Unit Editor");
            if (window.Unit)
            {
                window.CachedRootFolderPath = Path.GetDirectoryName(AssetDatabase.GetAssetPath(window.Unit));
            }
            window.Show();
        }

        void OnEnable()
        {
            Settings = UnitWindowSettings.Instance;
            Submenu = new EditSubmenu(this);
            OnChangedItem();
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

            Undo.RecordObject(this, "Unit");
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

        void OnChangedItem()
        {
            if (Unit)
            {
                SerializedObject = new SerializedObject(Unit);
            }
            else
            {
                DefaultValues();
            }
        }

        void DefaultValues()
        {
            SerializedObject = null;
            CachedUnit = null;
        }
    }
}
