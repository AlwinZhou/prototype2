namespace RedBjorn.SuperTiles.Paths
{
    /// <summary>
    /// Predefine paths for ScriptableObject creation
    /// </summary>
    public class ScriptablePath
    {
        public const string Root = RedBjorn.Utils.Paths.ScriptablePath.Root + "/" + Asset + "/";

        public const string Asset = nameof(SuperTiles);

        public class Level
        {
            public const string Asset = Root + "Level" + "/" + "Level";

            public class ActionRules
            {
                public const string HitAndRun = Root + "Level" + "/" + "Action Rules/" + "Hit & Run";
                public const string HitAfterRun = Root + "Level" + "/" + "Action Rules/" + "Hit After Run";
                public const string ActionPointsTwo = Root + "Level" + "/" + "Action Rules/" + "Action Points = 2";
                public const string ActionPointsCustom = Root + "Level" + "/" + "Action Rules/" + "Action Points Custom";
            }

            public class TurnResolver
            {
                public const string MoveRange = Root + "Level" + "/" + "Turn Resolver/" + "Move Range";
                public const string Squad = Root + "Level" + "/" + "Turn Resolver/" + "Squad";
            }
        }

        public class Unit
        {
            public const string Asset = Root + "Unit/Unit";
            public const string Tag = Root + "Unit/" + "Tag";
            public const string StatTag = Root + "Unit/" + "Stat tag";
        }

        public class Item
        {
            public const string ItemMenu = Root + "Item" + "/";
            public const string Selector = ItemMenu + "Selector" + "/";
            public const string Handler = ItemMenu + "Action Handler" + "/";
            public const string Asset = ItemMenu + "Item";
            public const string ItemStatTag = ItemMenu + "Stat Tag";
            public const string ItemTag = ItemMenu + "Tag";

            public class Selectors
            {
                public const string Direction = Selector + "Direction";
                public const string Range = Selector + "Range";
            }

            public class Handlers
            {
                public const string Heal = Handler + "Heal";
                public const string Bullet = Handler + "Bullet";
                public const string Melee = Handler + "Melee";
                public const string Grenade = Handler + "Grenade";
                public const string Laser = Handler + "Laser";
                public const string Teleport = Handler + "Teleport";
                public const string EffectAdd = Handler + "Effect Add";
                public const string EffectRemove = Handler + "Effect Remove";
            }
        }

        public class Effect
        {
            public const string ItemMenu = Root + "Effect" + "/";
            public const string Asset = ItemMenu + "Effect";
            public const string StatTag = ItemMenu + "Stat Tag";
            public const string Handler = ItemMenu + "Handlers" + "/";
            public class Handlers
            {
                public const string Damage = Handler + "Damage";
                public const string FxPlay = Handler + "FxPlay";
                public const string FxAddDefault = Handler + "Fx Add (default)";
                public const string FxRemoveDefault = Handler + "Fx Remove (default)";
                public const string UnitStatChange = Handler + "UnitStat change";
            }
        }

        public class Health
        {
            public const string HealthMenu = Root + "Health" + "/";
            public const string RulesAsset = HealthMenu + "Rules Data";
            public const string RulesRoot = HealthMenu + "Rules" + "/";
            public const string ConditionsRoot = HealthMenu + "Conditions" + "/";

            public class Conditions
            {
                public const string HaveEffect = ConditionsRoot + "Have effect";
            }

            public class Rules
            {
                public const string Rule = RulesRoot + "Rule";
            }
        }

        public class Tags
        {
            public const string Team = Root + "Tags/" + "Team";
            public const string Transform = Root + "Tags/" + "Transform";
            public const string Resource = Root + "Tags/" + "Resource";
        }

        public class Ai
        {
            public const string Simple = Root + "Ai" + "/" + "Simple";
            public const string Universal = Root + "Ai" + "/" + "Universal";
        }

        public class Settings
        {
            public const string Sound = Root + "Settings" + "/" + "Sound";
            public const string Game = Root + "Settings" + "/" + "Game";
            public const string Prefabs = Root + "Settings" + "/" + "Prefabs";
            public const string Battle = Root + "Settings" + "/" + "Battle";
            public const string Input = Root + "Settings" + "/" + "Input";
            public const string Levels = Root + "Settings" + "/" + "Levels";
        }

        public class Window
        {
            public const string QuickStart = Root + "Window/Quick Start Settings";
            public const string Level = Root + "Window/Level Settings";
            public const string Unit = Root + "Window/Unit Settings";
            public const string Item = Root + "Window/Item Settings";
        }
    }
}

