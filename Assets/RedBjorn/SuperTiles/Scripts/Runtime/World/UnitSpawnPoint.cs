using RedBjorn.Utils;
using UnityEngine;

namespace RedBjorn.SuperTiles
{
    /// <summary>
    /// Helper class for generating unit spawn data
    /// </summary>
    [ExecuteInEditMode]
    public class UnitSpawnPoint : MonoBehaviour
    {
        public bool Incorporeal;
        public TeamTag Team;
        public UnitData Data;
        public UnitAiData Ai;

        public override string ToString()
        {
            return string.Format("{0}. {1}. {2}. {3}", transform.position, Data ? Data.name : "No Unit", Team ? Team.name : "No Team", Ai ? Ai.name : "No Ai");
        }

#if UNITY_EDITOR

        public static UnitSpawnPoint Create(string name,
                                            Transform parent,
                                            Vector3 position,
                                            Quaternion rotation,
                                            TeamTag teamTag,
                                            UnitData data,
                                            UnitAiData ai)
        {
            var go = new GameObject(name);
            go.transform.SetParent(parent);
            go.transform.position = position;
            go.transform.rotation = rotation;

            var spawn = go.AddComponent<UnitSpawnPoint>();
            spawn.Team = teamTag;
            spawn.Data = data;
            spawn.Ai = ai;
            return spawn;
        }

        GameObject Preview;

        void Update()
        {
            if (!Application.isPlaying)
            {
                var destroyChild = Data == null ? transform.childCount > 0 : Preview != Data.Model;
                if (destroyChild)
                {
                    for (int i = 0; i < transform.childCount; i++)
                    {
                        DestroyImmediate(transform.GetChild(i).gameObject);
                    }
                    if (Data != null)
                    {
                        Preview = Data.Model;
                        if (Preview != null)
                        {
                            var model = GameObject.Instantiate(Preview, transform);
                            model.transform.localPosition = Vector3.zero;
                            model.hideFlags = HideFlags.HideAndDontSave;
                        }
                    }
                }

                var active = UnityEditor.Selection.activeTransform;
                if (active && active.IsChildOf(transform))
                {
                    UnityEditor.Selection.activeGameObject = gameObject;
                }
            }
        }
#endif
    }
}
