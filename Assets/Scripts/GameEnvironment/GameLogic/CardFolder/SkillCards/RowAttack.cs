using GameEnvironment.Units;

namespace GameEnvironment.GameLogic.CardFolder.SkillCards
{
    public class RowAttack : SkillCard
    {
        public override void UseOnGuard(Guard guard)
        {
            //guard.OnSkillUsed(_requiredAP);
            if (guard.TryGetEnemy(_battleHud.EnemyFrontRow))
            {
                foreach (var slot in _battleHud.EnemyFrontRow.GuardSlots)
                {
                    if (slot.GetComponentInChildren<EnemyGuard>() != null)
                    {
                        slot.GetComponentInChildren<EnemyGuard>().GetComponent<Health>().TakeDamage(guard.Damage);
                    }
                }
            }
            else if (guard.TryGetEnemy(_battleHud.EnemyBackRow))
            {
                foreach (var slot in _battleHud.EnemyBackRow.GuardSlots)
                {
                    if (slot.GetComponentInChildren<EnemyGuard>() != null)
                    {
                        slot.GetComponentInChildren<EnemyGuard>().GetComponent<Health>().TakeDamage(guard.Damage);
                    }
                }
            }
            else
                guard.Enemy.GetComponent<Health>().TakeDamage(guard.Damage);
        }
    }
}