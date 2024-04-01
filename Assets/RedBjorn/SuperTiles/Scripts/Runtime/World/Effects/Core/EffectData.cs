using RedBjorn.SuperTiles.Effects;
using RedBjorn.Utils;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace RedBjorn.SuperTiles
{
    /// <summary>
    /// Effect informational storage
    /// </summary>
    [CreateAssetMenu(menuName = Paths.ScriptablePath.Effect.Asset)]
    public class EffectData : ScriptableObjectExtended
    {
        [Serializable]
        public class StatData
        {
            public EffectStatTag Stat;
            public float Value;
        }

        public Sprite Icon;
        public string Caption;
        [Space]
        public List<StatData> Stats = new List<StatData>();
        [Space]
        public bool FxAddShow;
        [Tooltip("If equal None  than default fx will be selected")]
        public EffectHandler FxAddHandler;
        public bool FxRemoveShow;
        [Tooltip("If equal None  than default fx will be selected")]
        public EffectHandler FxRemoveHandler;
        [Space]
        public EffectHandler OnAdded;
        public EffectHandler Handler;
        public EffectHandler OnRemoved;
    }
}

