using RedBjorn.SuperTiles.Editors.Effect.Submenus;
using System;

namespace RedBjorn.SuperTiles.Editors.Effect.Tabs
{
    [Serializable]
    public class Handler : ITab
    {
        public EffectHandlerEditor Editor;

        public Handler(EffectWindow window)
        {
            Editor = new EffectHandlerEditor(window.Handler, "Handler");
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
