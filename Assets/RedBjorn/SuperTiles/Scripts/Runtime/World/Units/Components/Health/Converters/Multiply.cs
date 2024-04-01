namespace RedBjorn.SuperTiles.Health.ValueConverters
{
    /// <summary>
    /// Converter which multiply input value by value
    /// </summary>
    public class Multiply : ValueConverter
    {
        public float Value;

        public override float Convert(float val)
        {
            return val * Value;
        }
    }
}
