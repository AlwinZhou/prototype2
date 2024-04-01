using UnityEditor;

namespace RedBjorn.SuperTiles.Editors.Setup
{
    [InitializeOnLoad]
    public class Setup
    {
        static Setup()
        {
            EditorApplication.delayCall -= OnDelayCall;
            EditorApplication.delayCall += OnDelayCall;
        }

        static void OnDelayCall()
        {
            if (EditorPrefs.HasKey(SetupWindowSettings.SetupKeyOld))
            {
                var existed = EditorPrefs.GetBool(SetupWindowSettings.SetupKeyOld);
                EditorPrefs.SetBool(SetupWindowSettings.SetupKey, existed);
                EditorPrefs.DeleteKey(SetupWindowSettings.SetupKeyOld);
            }

            var versionCurrent = SetupWindowSettings.GetVersion();
            var versionSaved = EditorPrefs.GetString(SetupWindowSettings.VersionKey, null);
            if (SetupWindowSettings.DoNeedSetup)
            {
                SetupWindow.DoShow(versionSaved, versionCurrent);
            }
            if (!string.IsNullOrEmpty(versionCurrent) && versionCurrent != versionSaved)
            {
                EditorPrefs.SetString(SetupWindowSettings.VersionKey, versionCurrent);
            }
        }
    }
}
