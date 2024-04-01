using RedBjorn.Utils;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace RedBjorn.SuperTiles.UI
{
    public class TeamPanelUI : MonoBehaviour
    {
        public Transform TeamSelectorParent;
        public UnitSelectorUI UnitSelector;
        public UnitProfileUI UnitProfile;
        public GameObject Panel;

        List<UnitSelectorUI> Selectors = new List<UnitSelectorUI>();

        void Awake()
        {
            UnitSelector.gameObject.SetActive(false);
            Panel.SetActive(false);
        }

        public void Init(List<UnitEntity> squad, UnitEntity current, Action<UnitEntity> onSelectUnit)
        {
            Panel.SetActive(true);
            foreach (var s in Selectors)
            {
                Spawner.Despawn(s.gameObject);
            }
            Selectors.Clear();

            var index = 1;
            for (int i = 0; i < squad.Count; i++)
            {
                if (!squad[i].IsDead)
                {
                    var s = Spawner.Spawn(UnitSelector, TeamSelectorParent);
                    s.Init(onSelectUnit, squad[i], index, squad[i] == current);
                    Selectors.Add(s);
                    index++;
                }
            }

            UnitProfile.Show(current);
        }
    }
}