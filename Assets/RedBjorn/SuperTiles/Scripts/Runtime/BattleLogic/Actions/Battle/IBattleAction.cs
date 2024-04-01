using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RedBjorn.SuperTiles
{
    public interface IBattleAction
    {
        void Handle(BattleEntity battle);
    }
}
