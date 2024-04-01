using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace RedBjorn.SuperTiles.UI
{
    public interface ITooltip
    {
        string Text();
    }

    public class SessionTooltipUI : MonoBehaviour
    {
        public TextMeshProUGUI Text;
        public GameObject Panel;

        List<ITooltip> Tooltips = new List<ITooltip>();

        void Awake()
        {
            UpdateTooltip();
        }

        public void Register(ITooltip tooltip)
        {
            if (!Tooltips.Contains(tooltip))
            {
                Tooltips.Add(tooltip);
            }
            UpdateTooltip();
        }

        public void Unregister(ITooltip tooltip)
        {
            Tooltips.Remove(tooltip);
            UpdateTooltip();
        }

        void UpdateTooltip()
        {
            if (Tooltips.Count == 0)
            {
                Text.text = string.Empty;
                Panel.SetActive(false);
                return;
            }
            var t = Tooltips[0];
            Text.text = t.Text();
            Panel.SetActive(true);
        }
    }
}
