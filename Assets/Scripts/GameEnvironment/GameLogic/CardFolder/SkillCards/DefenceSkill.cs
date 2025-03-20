using GameEnvironment.Units;

namespace GameEnvironment.GameLogic.CardFolder.SkillCards
{
    public class DefenceSkill : SkillCard
    {
        public override void UseOnGuard(Guard guard)
        {
            guard.GetComponent<Health>().RiseDefence(_appliedValue);
            //guard.OnSkillUsed(_requiredAP);
        }
    }
}