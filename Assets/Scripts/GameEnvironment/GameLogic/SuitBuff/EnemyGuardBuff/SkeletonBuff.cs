using UnityEngine;

namespace GameEnvironment.GameLogic.SuitBuff.EnemyGuardBuff
{
    public class SkeletonBuff : Buff
    {
        public override void ApplyBuff()
        {
            base.ApplyBuff();
            Debug.Log("skeletonBuff");
        }

        public override void ResetBuff()
        {
            base.ResetBuff();
            Debug.Log("ResetSkeletonBuff");
        }
    }
}