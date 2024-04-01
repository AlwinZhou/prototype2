using System;

namespace RedBjorn.SuperTiles.Editors.Effect.Submenus
{
    public interface ITab
    {
        void Draw(EffectWindow window);
    }

    [Serializable]
    public class Tab
    {
        public string Caption;
        public ITab Submenu;
    }
}