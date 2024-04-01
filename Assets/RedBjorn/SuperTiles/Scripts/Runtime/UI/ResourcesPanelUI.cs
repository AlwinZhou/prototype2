using RedBjorn.Utils;
using TMPro;
using UnityEngine;

namespace RedBjorn.SuperTiles.UI
{
    public class ResourcesPanelUI : MonoBehaviour
    {
        public GameObject Panel;
        public GameObject ResourceRef;

        void Awake()
        {
            ResourceRef.SetActive(false);
        }

        public void Show(SquadControllerEntity.ResourcesDictionary resources)
        {
            for (int i = Panel.transform.childCount - 1; i >= 0; i--)
            {
                var child = Panel.transform.GetChild(i).gameObject;
                if (child.activeSelf)
                {
                    Spawner.Despawn(child);
                }
            }
            
            if (resources != null)
            {
                foreach (var resource in resources)
                {
                    var resourceGo = Spawner.Spawn(ResourceRef, ResourceRef.transform.parent);
                    resourceGo.GetComponentInChildren<TMP_Text>(true).text = $"{resource.Key.name}: {resource.Value.ToString()}";
                    resourceGo.SetActive(true);
                }
            }
        }
    }
}
