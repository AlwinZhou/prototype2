namespace RedBjorn.SuperTiles.Health.ValueConverters
{
    /// <summary>
    /// Converter which substract value from input value
    /// </summary>
    public class Substract : ValueConverter
    {
        public float Value;

        public override float Convert(float val)
        {
            return val - Value;
        }
    }
}
