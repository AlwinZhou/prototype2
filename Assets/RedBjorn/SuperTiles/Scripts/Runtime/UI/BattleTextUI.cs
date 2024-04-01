using System.Linq;
using System.Text;
using TMPro;
using UnityEngine;

namespace RedBjorn.SuperTiles.UI
{
    public class BattleTextUI : MonoBehaviour
    {
        public TextMeshProUGUI Text;
        public GameObject Background;

        StringBuilder Info = new StringBuilder();

        BattleView Controller { get { return ObjectExtensions.FindObjectOfType<BattleView>(); } }
        GameEntity Game { get { return Controller.Game; } }

        void Awake()
        {
            Text.enabled = false;
            Background.SetActive(false);
        }

        void Update()
        {
            if (InputController.GetGameHotkeyUp(S.Input.DebugUi))
            {
                Switch();
            }
            Show();
        }

        void Show()
        {
            Info.Clear();
            if (Controller)
            {
                Info.AppendLine(string.Format("State:\n{0}\n", Controller.State));
            }

            if (Game != null && Game.Battle != null)
            {
                foreach (var p in Game.Battle.Players.OrderBy(pl => pl.Id))
                {

                    Info.Append(p);
                    if (Controller.Owners.Contains(p))
                    {
                        Info.Append(" (me)");
                    }
                    Info.AppendLine("\nSquad:\n");
                    foreach (var s in p.Squad)
                    {
                        var state = s.IsDead ? "Dead" : "Alive";
                        Info.AppendLine($"{s} ({state}): {string.Join(",", s.Effects)}");
                    }
                    Info.AppendLine();
                }

                Info.AppendLine("Timeline:");
                foreach (var u in Game.Battle.UnitsTimeline)
                {
                    Info.AppendLine(u.ToString());
                }

            }
            Text.text = Info.ToString();
        }

        void Switch()
        {
            Text.enabled = !Text.enabled;
            Background.SetActive(Text.enabled);
        }
    }
}