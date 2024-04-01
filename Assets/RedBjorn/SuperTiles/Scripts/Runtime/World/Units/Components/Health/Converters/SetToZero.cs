namespace RedBjorn.SuperTiles.Health.ValueConverters
{
    /// <summary>
    /// Converter which set input value to zero
    /// </summary>
    public class SetToZero : ValueConverter
    {
        public override float Convert(float val)
        {
            return 0f;
        }
    }
}
