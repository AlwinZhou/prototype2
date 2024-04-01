using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RedBjorn.SuperTiles.UnitChangeControllerActions
{
    public class Multiple : IUnitChangeControllerAction
    {
        public List<IUnitChangeControllerAction> List;

        public void Handle(UnitEntity unit, SquadControllerEntity newController, BattleEntity battle)
        {
            if (List != null)
            {
                foreach (var action in List)
                {
                    if (action != null)
                    {
                        action.Handle(unit, newController, battle);
                    }
                }
            }
        }
    }

    public class ModelColorChange : IUnitChangeControllerAction
    {
        [Serializable]
        public class Data
        {
            public TeamTag Team;
            public Color Color;
        }

        public List<Data> Info;

        void IUnitChangeControllerAction.Handle(UnitEntity unit, SquadControllerEntity newController, BattleEntity battle)
        {
            if (newController != null)
            {
                var info = Info.FirstOrDefault(i => i.Team == newController.Team);
                if (info != null)
                {
                    foreach (var renderer in unit.View.Model.GetComponentsInChildren<Renderer>())
                    {
                        renderer.material.color = info.Color;
                    }
                }
            }
        }
    }
}
