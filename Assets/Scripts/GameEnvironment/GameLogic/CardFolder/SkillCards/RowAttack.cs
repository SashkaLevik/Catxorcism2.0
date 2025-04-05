using GameEnvironment.Units;

namespace GameEnvironment.GameLogic.CardFolder.SkillCards
{
    public class RowAttack : SkillCard
    {
        public override void UseSkill(Unit unit)
        {
            if (unit.CurrentEnemy != null && unit.CurrentEnemy.UnitRow == null)
            {
                unit.CurrentEnemy.Health.TakeDamage(unit.Damage);
            }
            else if (unit.CurrentEnemy != null && unit.UnitRow != null)
            {
                foreach (var slot in unit.CurrentEnemy.UnitRow.GuardSlots)
                {
                    if (slot.GetComponentInChildren<Unit>() != null)
                    {
                        slot.GetComponentInChildren<Unit>().Health.TakeDamage(unit.Damage);
                    }
                }
            }
            
            if (unit.GetComponent<Guard>()) 
                unit.GetComponent<Guard>().OnSkillPlayed(this);
        }
    }
}