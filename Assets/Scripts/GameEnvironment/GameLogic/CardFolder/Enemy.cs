using GameEnvironment.Units;

namespace GameEnvironment.GameLogic.CardFolder
{
    public class Enemy : Unit
    {
        protected Health _health;
        private Player _player;
        private Guard _guard;

        protected override void Start()
        {
            _attackSystem = GetComponent<AttackSystem>();
            _health = GetComponent<Health>();
            _damage = _cardData.Damage;
            _damageAmount.text = _damage.ToString();
            _health.HealthChanged += UpdateHealth;
        }

        private void OnDestroy()
        {
            _health.HealthChanged -= UpdateHealth;
        }

        public void InitGuard(Guard guard) =>
            _guard = guard;

        public void SetPosition() =>
            _startPosition = transform.position;

        public void InitPlayer(Player player) =>
            _player = player;

        public void Attack()
        {
            if (_guard != null)
                _attackSystem.Attack(this, _guard);
            else if (_player != null)
                _attackSystem.Attack(this, _player);                       
        }                          

        private void UpdateHealth(int value)=>
            _healthAmount.text = value.ToString();        
    }
}