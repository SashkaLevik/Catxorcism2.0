namespace Assets.Scripts.GameEnvironment.Units
{
    public class Boss : Enemy
    {
        protected override void Start()
        {
            base.Start();
            _health.CurrentHP = _cardData.Health;
        }
    }
}