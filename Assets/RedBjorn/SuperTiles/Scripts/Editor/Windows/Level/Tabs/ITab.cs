using System;

namespace RedBjorn.SuperTiles.Editors.Level.Submenus
{
    public interface ITab
    {
        void Draw(LevelWindow window);
    }

    [Serializable]
    public class Tab
    {
        public string Caption;
        public ITab Submenu;
    }
}
