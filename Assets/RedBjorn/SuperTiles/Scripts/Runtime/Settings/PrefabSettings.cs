using RedBjorn.SuperTiles.Items.TargetSelectors;
using RedBjorn.SuperTiles.UI;
using RedBjorn.SuperTiles.Utils;
using RedBjorn.Utils;
using UnityEngine;

namespace RedBjorn.SuperTiles.Settings
{
    /// <summary>
    /// Settings which contains reference to prefabs which do not have own reference holders
    /// </summary>
    public class PrefabSettings : ScriptableObjectExtended
    {
        public GameObject LoadingScreen;
        public TurnPlayerMyUI TurnStartMyPanel;
        public TurnPlayerOtherUI TurnStartOtherPanel;
        public BattleFinishUI BattleFinishPanel;
        public ConfirmMessageUI ConfirmMessage;
        public UnitView UnitView;
        public TileMarker TileInvalid;
        public AreaOutline AreaOutline;
        public PathDrawer Path;
        public RangeTargetSelectorView RangeMode;
        public DirectionTargetSelectorView DirectionMode;
        public MenuLoader MenuLoader;
        public BarSprite HealthBar;
        public EffectUnitWorldUI EffectUI;
        public HealthChangeView HealthChange;
    }
}
