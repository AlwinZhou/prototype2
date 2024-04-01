using RedBjorn.SuperTiles.UI;
using RedBjorn.Utils;
using System.Collections;
using System.Linq;
using UnityEngine;

namespace RedBjorn.SuperTiles.Battle.States
{
    /// <summary>
    /// BattleView state which is a initial state for Player loop
    /// </summary>
    public class BattleFinishState : State
    {
        protected override void Enter()
        {
            CoroutineHelper.Launch(Finishing());
        }

        public override void Update()
        {

        }

        public override void Exit()
        {

        }

        public override void OnUnitChanged()
        {

        }

        public override void OnTurnStarted()
        {

        }

        public override void OnTurnFinishStarted()
        {

        }

        public override void OnBattleFinish()
        {

        }

        IEnumerator Finishing()
        {
            Controller.Status = Controller.Statuses.OnBattleFinish;
            BattleFinishUI.Show();
            yield return new WaitForSecondsRealtime(3f);
            var sb = new System.Text.StringBuilder();
            var texts = S.Text.BattleFinish;
            if (Battle.Winners.Count > 0)
            {
                sb.AppendFormat(texts.Victory, string.Join(", ", Battle.Winners.Select(w => w.Nickname)));
                sb.AppendLine();
            }
            else
            {
                sb.AppendLine(texts.Draw);
            }
            sb.AppendLine(texts.ToMenu);
            ConfirmMessageUI.Show(sb.ToString(), texts.Confirm, texts.Abort, () => { SceneLoader.Load(S.Levels.MenuSceneName); }, null);
        }
    }
}