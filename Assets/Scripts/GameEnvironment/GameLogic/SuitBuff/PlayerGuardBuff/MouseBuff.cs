namespace GameEnvironment.GameLogic.SuitBuff.PlayerGuardBuff
{
    public class MouseBuff : Buff
    {
        public override void ApplyBuff()
        {
            base.ApplyBuff();
            _unitToBuff.CanAttackTwice = true;
        }

        public override void ResetBuff()
        {
            base.ResetBuff();
            _unitToBuff.CanAttackTwice = false;
        }
    }
}