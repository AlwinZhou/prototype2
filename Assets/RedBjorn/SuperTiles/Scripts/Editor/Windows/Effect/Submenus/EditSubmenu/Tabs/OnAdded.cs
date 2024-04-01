using RedBjorn.SuperTiles.Editors.Effect.Submenus;
using System;

namespace RedBjorn.SuperTiles.Editors.Effect.Tabs
{
    [Serializable]
    public class OnAdded : ITab
    {
        public EffectHandlerEditor Editor;

        public OnAdded(EffectWindow window)
        {
            Editor = new EffectHandlerEditor(window.OnAdded, "OnAdded");
        }

        public void Draw(EffectWindow window)
        {
            if (Editor != null)
            {
                Editor.Draw(window);
            }
        }
    }
}

