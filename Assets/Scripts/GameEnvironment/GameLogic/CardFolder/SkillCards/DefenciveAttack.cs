using GameEnvironment.Units;

namespace GameEnvironment.GameLogic.CardFolder.SkillCards
{
    public class DefenciveAttack : SkillCard
    {
        public override void UseSkill(Unit unit)
        {
            if (unit.CurrentEnemy != null) 
                unit.CurrentEnemy.OnAttackSkill(unit.CurrentDamage, this);

            unit.OnDefence(_appliedValue / 2);
            
            if (unit.GetComponent<Guard>()) 
                unit.GetComponent<Guard>().OnSkillPlayed(this);
        }
    }
}