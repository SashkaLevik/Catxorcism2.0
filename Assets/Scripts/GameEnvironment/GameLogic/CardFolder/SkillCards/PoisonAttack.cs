namespace GameEnvironment.GameLogic.CardFolder.SkillCards
{
    public class PoisonAttack : SkillCard
    {
        public override void UseSkill(Unit unit)
        {
            if (unit.CurrentEnemy != null) 
                unit.CurrentEnemy.Health.TakeDamage(unit.CurrentDamage);
            
            if (unit.GetComponent<Guard>()) 
                unit.GetComponent<Guard>().OnSkillPlayed(this);
        }
    }
}