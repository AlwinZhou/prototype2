namespace RedBjorn.SuperTiles.Editors.HealthRule.Submenus
{
    public interface ITab
    {
        void Draw(HealthRuleWindow window);
    }

    [System.Serializable]
    public class Tab
    {
        public string Caption;
        public ITab Submenu;
    }
}
