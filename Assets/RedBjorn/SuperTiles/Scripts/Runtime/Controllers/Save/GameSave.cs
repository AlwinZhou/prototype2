using System;

namespace RedBjorn.SuperTiles.Saves
{
    [Serializable]
    public class GameSave
    {
        public string Version;
        public string Timestamp;
        public GameEntity State;
    }
}