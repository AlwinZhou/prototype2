using System;

namespace RedBjorn.SuperTiles.Editors.Unit.Submenus.Edit
{
    [Serializable]
    public class Tab
    {
        public string Caption;
        public ITab Drawer;
    }
}
