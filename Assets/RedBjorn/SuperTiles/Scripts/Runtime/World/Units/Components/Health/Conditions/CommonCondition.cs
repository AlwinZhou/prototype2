namespace RedBjorn.SuperTiles.Health
{
    /// <summary>
    /// Intermediate Condition with preliminary checks: wich value should be converted: damage or heal?
    /// </summary>
    public abstract class CommonCondition : Condition
    {
        public bool ConvertHeal;
        public bool ConvertDamage;

        public override bool IsMet(float delta, UnitEntity victim, UnitEntity damager, ItemEntity item)
        {
            if (!ConvertHeal && delta > 0)
            {
                return false;
            }

            if (!ConvertDamage && delta <= 0)
            {
                return false;
            }
            return CheckIsMet(delta, victim, damager, item);
        }

        protected abstract bool CheckIsMet(float delta, UnitEntity victim, UnitEntity damager, ItemEntity item);
    }
}
