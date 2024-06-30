namespace Assets.Scripts.GameEnvironment.Units
{
    public class GuardHealth : Health
    {
        private Guard _guard;

        private void Start()
        {
            _guard = GetComponent<Guard>();
            CurrentHP = _guard.CardData.Health;
            MaxHP = _guard.CardData.Health;
        }
    }
}