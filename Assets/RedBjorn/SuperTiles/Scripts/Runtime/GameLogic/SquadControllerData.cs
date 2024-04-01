using System;

namespace RedBjorn.SuperTiles
{
    /// <summary>
    /// Info for Ai or Player level creation
    /// </summary>
    [Serializable]
    public class SquadControllerData
    {
        public string Name = "Player 1";
        public TeamTag Team;
        public SquadControllerType ControlledBy;

        public static string[] Types()
        {
            return Enum.GetNames(typeof(SquadControllerType));
        }

        public static int TypeIndex(SquadControllerType type)
        {
            return (int)type;
        }

        public static SquadControllerType Type(int index)
        {
            return (SquadControllerType)index;
        }
    }
}