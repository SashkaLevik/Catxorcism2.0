using UnityEngine;

namespace GameEnvironment.GameLogic.CardFolder.SkillCards
{
    public class Mark : SkillCard
    {
        public override void UseOnEnemy(EnemyGuard enemyGuard)
        {
            Debug.Log(enemyGuard);
        }
    }
}