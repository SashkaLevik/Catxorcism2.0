using GameEnvironment.Units;

namespace GameEnvironment.GameLogic.CardFolder.SkillCards
{
    public class RowAttack : SkillCard
    {
        public override void UseSkill(Unit unit)
        {
            if (unit.CurrentEnemy != null && unit.CurrentEnemy.UnitRow == null)
            {
                var target = unit.CurrentEnemy;
                unit.Attack(this, target, unit.CurrentDamage);
            }
            if (unit.CurrentEnemy != null && unit.UnitRow != null)
            {
                foreach (var slot in unit.CurrentEnemy.UnitRow.GuardSlots)
                {
                    if (slot.GetComponentInChildren<Unit>() != null)
                    {
                        var target = slot.GetComponentInChildren<Unit>();
                        unit.Attack(this, target, unit.CurrentDamage);
                    }
                }
            }
            
            if (unit.GetComponent<Guard>()) 
                unit.GetComponent<Guard>().OnSkillPlayed(this);
        }
    }
}