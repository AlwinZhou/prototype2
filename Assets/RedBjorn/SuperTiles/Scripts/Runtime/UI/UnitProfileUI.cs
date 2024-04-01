using RedBjorn.Utils;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RedBjorn.SuperTiles.UI
{
    /// <summary>
    /// Class which represents unit inside UI
    /// </summary>
    public class UnitProfileUI : MonoBehaviour
    {
        public Image Avatar;
        public Button AvatarButton;
        public Image EffectTemplate;
        public Transform EffectParent;
        public GameObject Panel;

        List<Image> Effects = new List<Image>();

        void Awake()
        {
            Panel.SetActive(false);
            EffectTemplate.gameObject.SetActive(false);
        }

        public void Show(UnitEntity unit)
        {
            foreach (var s in Effects)
            {
                Spawner.Despawn(s.gameObject);
            }
            Effects.Clear();

            AvatarButton.onClick.RemoveAllListeners();

            if (unit != null)
            {
                for (int i = 0; i < unit.Effects.Count; i++)
                {
                    var effectUI = Spawner.Spawn(EffectTemplate, EffectParent);
                    effectUI.sprite = unit.Effects[i].Data.Icon;
                    effectUI.gameObject.SetActive(true);
                    Effects.Add(effectUI);
                }
                Avatar.sprite = unit.Data.Avatar;
                Avatar.enabled = true;
                AvatarButton.onClick.AddListener(() =>
                {
                    if (Camera.main.TryGetComponent(out CameraController camera))
                    {
                        camera.MoveTo(unit.WorldPosition);
                    }
                });
                Panel.SetActive(true);
            }
            else
            {
                foreach (var icon in Effects)
                {
                    icon.enabled = false;
                }
                Avatar.enabled = false;
                Panel.SetActive(false);
            }
        }
    }
}