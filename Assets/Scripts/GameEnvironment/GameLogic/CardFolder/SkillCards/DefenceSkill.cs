using GameEnvironment.Units;

namespace GameEnvironment.GameLogic.CardFolder.SkillCards
{
    public class DefenceSkill : SkillCard
    {
        public override void UseSkill(Unit unit)
        {
            unit.GetSkillEffect(_appliedValue, this);
            
            if (unit.GetComponent<Guard>()) 
                unit.GetComponent<Guard>().OnSkillPlayed(this);
        }
    }
}