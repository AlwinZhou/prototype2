using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

namespace RedBjorn.SuperTiles
{
    /// <summary>
    /// Nickname text world component
    /// </summary>
    public class UnitPlayerName : MonoBehaviour, IUnitInitialize
    {
        public TextMeshPro Text;
        UnitEntity Unit;

        Settings.BattleSettings.UiSettings UI => S.Battle.UI;

        void Awake()
        {
            PlayerNameUpdate();
        }

        void OnDestroy()
        {
            if (Unit != null)
            {
                Unit.Health.OnHeathChanged -= OnHealthChanged;
                Unit.Game.OnStarted -= OnStarted;
            }
        }

        public void Init(UnitEntity unit)
        {
            if (Unit != null)
            {
                Unit.Health.OnHeathChanged -= OnHealthChanged;
                Unit.Game.OnStarted -= OnStarted;
            }
            Unit = unit;
            if (Unit != null)
            {
                Unit.Health.OnHeathChanged += OnHealthChanged;
                Unit.Game.OnStarted += OnStarted;
            }
            PlayerNameUpdate();
        }

        void OnHealthChanged(HealthChangeContext context)
        {
            PlayerNameUpdate();
        }

        void OnStarted()
        {
            PlayerNameUpdate();
        }

        void PlayerNameUpdate()
        {
            if (!UI.NicknameShow || Unit == null || Unit.IsDead)
            {
                Text.gameObject.SetActive(false);
                return;
            }

            var player = Unit.Game.Battle.Players.FirstOrDefault(p => p.Squad.Contains(Unit));
            if(player == null)
            {
                Text.gameObject.SetActive(false);
                return;
            }

            Text.text = player.Nickname;
            Text.gameObject.SetActive(true);
        }
    }
}
