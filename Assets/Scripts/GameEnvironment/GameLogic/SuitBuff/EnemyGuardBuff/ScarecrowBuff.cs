using UnityEngine;

namespace GameEnvironment.GameLogic.SuitBuff.EnemyGuardBuff
{
    public class ScarecrowBuff : Buff
    {
        public override void ApplyBuff()
        {
            base.ApplyBuff();
            Debug.Log("ScarecrowBuff");
        }

        public override void ResetBuff()
        {
            base.ResetBuff();
            Debug.Log("ResetScarecrowBuff");
        }
    }
}