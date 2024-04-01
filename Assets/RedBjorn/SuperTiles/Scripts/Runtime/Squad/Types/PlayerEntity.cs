using System;

namespace RedBjorn.SuperTiles.Squad
{
    /// <summary>
    /// Real player
    /// </summary>
    [Serializable]
    public class PlayerEntity : SquadControllerEntity
    {
        public PlayerEntity(int id, TeamTag team, string nickname, GameEntity game) : base(id, team, game)
        {
            Nickname = nickname;
        }

        public override string ToString()
        {
            return string.Format("Player (id: {0})", Id);
        }
    }
}

