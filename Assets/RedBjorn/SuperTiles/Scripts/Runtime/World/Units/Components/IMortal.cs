namespace RedBjorn.SuperTiles
{
    public interface IMortal
    {
        bool IsDead { get; }
        StatEntity Max { get; }
        void OnMinValueReached();
    }
}
