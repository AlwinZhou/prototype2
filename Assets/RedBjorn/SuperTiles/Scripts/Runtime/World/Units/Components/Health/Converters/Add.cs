namespace RedBjorn.SuperTiles.Health.ValueConverters
{
    /// <summary>
    /// Converter which add value to input value
    /// </summary>
    public class Add : ValueConverter
    {
        public float Value;

        public override float Convert(float val)
        {
            return val + Value;
        }
    }
}
