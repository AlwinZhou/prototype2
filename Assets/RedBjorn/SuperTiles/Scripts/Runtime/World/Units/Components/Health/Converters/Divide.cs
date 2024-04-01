namespace RedBjorn.SuperTiles.Health.ValueConverters
{
    /// <summary>
    /// Converter which divied input value by value
    /// </summary>
    public class Divide : ValueConverter
    {
        public float Value;

        public override float Convert(float val)
        {
            return val / Value;
        }
    }
}