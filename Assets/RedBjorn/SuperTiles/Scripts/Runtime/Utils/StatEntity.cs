using System;
using UnityEngine;

namespace RedBjorn.SuperTiles
{
    /// <summary>
    /// State of statistic following the rule: a * x + b
    /// </summary>
    [Serializable]
    public struct StatEntity
    {
        public float Multiplicator;
        public float Additive;
        public float DataValue;

        public float Result => Multiplicator * DataValue + Additive;

        public StatEntity(float dataValue)
        {
            DataValue = dataValue;
            Multiplicator = 1f;
            Additive = 0f;
        }

        public StatEntity(StatEntity stat)
        {
            DataValue = stat.DataValue;
            Multiplicator = stat.Multiplicator;
            Additive = stat.Additive;
        }

        public static implicit operator float(StatEntity stat)
        {
            return stat.Result;
        }

        public static implicit operator int(StatEntity stat)
        {
            return Mathf.RoundToInt(stat.Result);
        }
    }
}