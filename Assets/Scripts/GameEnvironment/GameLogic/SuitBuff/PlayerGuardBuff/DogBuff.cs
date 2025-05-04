namespace GameEnvironment.GameLogic.SuitBuff.PlayerGuardBuff
{
    public class DogBuff : Buff
    {
        public override void ApplyBuff()
        {
            //base.ApplyBuff();
            //_unitToBuff.CanCounter = true;
            _unitToBuff.IsBuffApplied = true;
            _guard.DeckCreator.DrawBuffCard(_guard);
        }

        public override void ResetBuff()
        {
            //base.ResetBuff();
            //_unitToBuff.CanCounter = false;
        }
    }
}