namespace RedBjorn.SuperTiles
{
    public interface IItemCondition
    {
        bool IsMet(ItemEntity item, UnitEntity owner);
    }
}