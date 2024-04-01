using RedBjorn.SuperTiles.Settings;
using UnityEngine;

namespace RedBjorn.SuperTiles
{
    /// <summary>
    /// Short access for game settings 
    /// </summary>
    public partial class S
    {
        static Settings.GameSettings CachedGame;
        static Settings.GameSettings Game
        {
            get
            {
                if (CachedGame == null)
                {
                    CachedGame = Resources.Load<Settings.GameSettings>("GameSettings");
                }
                return CachedGame;
            }
        }

        public static LevelSettings Levels { get { return Game.Levels; } }
        public static InputSettings Input { get { return Game.Input; } }
        public static PrefabSettings Prefabs { get { return Game.Prefabs; } }
        public static SoundSettings Sound { get { return Game.Sound; } }
        public static BattleSettings Battle { get { return Game.Battle; } }
        public static TextSettings Text { get { return Game.Text; } }
        public static LogSettings Log { get { return Game.Log; } }

        public static void HealthInit(GameObject holder, UnitEntity unit)
        {
            if (!holder)
            {
                return;
            }
            if (holder.TryGetComponent(out UnitHealthView view))
            {
                view.Init(unit,
                    Battle.Tags.Unit.Health,
                    Battle.ShowHealthChange,
                    Prefabs.HealthChange,
                    Battle.Tags.Transform.UiHolder,
                    Prefabs.HealthBar,
                    Battle.Health.BarSpeedFill,
                    Battle.Health.BarOnDeathColor,
                    Battle.Health.BarOnDeathHide);
            }
        }
    }
}
