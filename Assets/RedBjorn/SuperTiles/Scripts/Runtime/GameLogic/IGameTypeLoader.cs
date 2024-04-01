using System.Collections;

namespace RedBjorn.SuperTiles
{
    public interface IGameTypeLoader
    {
        IEnumerator Load(GameEntity game);
    }
}
