using RedBjorn.Utils;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace RedBjorn.SuperTiles
{
    /// <summary>
    /// Entity which contains a list of units
    /// </summary>
    [Serializable]
    public abstract class SquadControllerEntity
    {
        [Serializable]
        public class ResourcesDictionary : SerializableDictionary<ResourceTag, float> { } // Hack to serialize dictionary

        public int Id;
        public string Nickname;
        public TeamTag Team;
        [SerializeReference]
        public List<UnitEntity> Squad;
        public ResourcesDictionary Resources;

        [NonSerialized]
        protected GameEntity Game;

        public float this[ResourceTag stat] => Resources.TryGetOrDefault(stat);

        public SquadControllerEntity(int id, TeamTag team, GameEntity game)
        {
            Id = id;
            Team = team;
            Game = game;
            Squad = new List<UnitEntity>();
            Resources = new ResourcesDictionary();
        }

        public void Load(GameEntity game)
        {
            Game = game;
        }

        public virtual void AddUnit(UnitEntity unit)
        {
            Squad.Add(unit);
        }

        public virtual void RemoveUnit(UnitEntity unit)
        {
            Squad.Remove(unit);
        }

        public virtual void OnMyTurnstarted() { }
    }
}