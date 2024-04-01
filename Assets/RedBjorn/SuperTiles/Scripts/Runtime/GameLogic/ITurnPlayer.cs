using RedBjorn.SuperTiles.Battle;
using System;

namespace RedBjorn.SuperTiles
{
    public interface ITurnPlayer
    {
        float TimePassed { get; }
        void Start();
        void Destroy();
        void TurnStart();
        void Play(BaseAction action, Action onCompleted);
        void TurnFinish();
    }
}
