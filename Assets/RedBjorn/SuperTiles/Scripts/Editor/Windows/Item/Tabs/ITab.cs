

namespace RedBjorn.SuperTiles.Editors.Item
{
    [System.Serializable]
    public class Tab
    {
        public string Caption;
        public ITab Submenu;
    }

    public interface ITab
    {
        void Draw(ItemWindow window);
    }
}










