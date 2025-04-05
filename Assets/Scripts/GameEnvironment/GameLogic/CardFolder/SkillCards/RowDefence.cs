using GameEnvironment.Units;

namespace GameEnvironment.GameLogic.CardFolder.SkillCards
{
    public class RowDefence : SkillCard
    {
        public override void UseSkill(Unit unit)
        {
            foreach (var slot in unit.UnitRow.GuardSlots)
            {
                if (slot.GetComponentInChildren<Unit>() != null)
                {
                    slot.GetComponentInChildren<Unit>().GetComponent<Health>().RiseDefence(_appliedValue);
                }
            }
            
            if (unit.GetComponent<Guard>()) 
                unit.GetComponent<Guard>().OnSkillPlayed(this);
        }
    }
}