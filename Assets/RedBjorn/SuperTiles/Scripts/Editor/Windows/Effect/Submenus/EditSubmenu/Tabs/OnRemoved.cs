using RedBjorn.SuperTiles.Editors.Effect.Submenus;
using System;

namespace RedBjorn.SuperTiles.Editors.Effect.Tabs
{
    [Serializable]
    public class OnRemoved : ITab
    {
        public EffectHandlerEditor Editor;

        public OnRemoved(EffectWindow window)
        {
            Editor = new EffectHandlerEditor(window.OnRemoved, "OnRemoved");
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
