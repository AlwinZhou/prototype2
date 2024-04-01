using RedBjorn.Utils;
using System;
using System.IO;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace RedBjorn.SuperTiles.Editors.Unit
{
    public interface ISubmenu
    {
        void Draw(UnitWindow window);
    }
}
