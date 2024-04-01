using RedBjorn.Utils;
using System;

namespace RedBjorn.SuperTiles.Settings
{
    public class TextSettings : ScriptableObjectExtended
    {
        [Serializable]
        public class BattleFinishText
        {
            public string Victory;
            public string Draw;
            public string ToMenu;
            public string Confirm;
            public string Abort;
        }

        [Serializable]
        public class BattleStatusText
        {
            public string TurnMy;
            public string TurnOther;
            public string TurnFinish;
            public string OnStart;
            public string OnBattleStart;
            public string OnBattleFinish;
        }

        [Serializable]
        public class TurnStartText
        {
            public string My;
            public string Ally;
            public string Enemy;
        }

        [Serializable]
        public class TurnFinishText
        {
            public string CompleteQuestion;
            public string CompleteConfirm;
            public string CompleAbort;
        }

        public BattleStatusText Status;
        public TurnStartText TurnStart;
        public TurnFinishText TurnFinish;
        public BattleFinishText BattleFinish;
    }
}
