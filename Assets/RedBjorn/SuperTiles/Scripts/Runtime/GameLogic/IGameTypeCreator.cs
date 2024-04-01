using System.Collections;

namespace RedBjorn.SuperTiles
{
    public interface IGameTypeCreator
    {
        IEnumerator Create(GameEntity game);
    }
}
