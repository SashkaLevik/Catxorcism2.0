using GameEnvironment.Units;

namespace GameEnvironment.GameLogic.CardFolder.SkillCards
{
    public class AttackSkill : SkillCard
    {
        public override void UseOnGuard(Guard guard)
        {
            guard.OnSkillUsed(_requiredAP);
            if (guard.TryGetEnemy(_battleHud.EnemyFrontRow))
                guard.EnemyGuard.GetComponent<Health>().TakeDamage(guard.Damage);
            else if (guard.TryGetEnemy(_battleHud.EnemyBackRow))
                guard.EnemyGuard.GetComponent<Health>().TakeDamage(guard.Damage);
            else
                guard.Enemy.GetComponent<Health>().TakeDamage(guard.Damage);
        }
    }
}