using Assets.Scripts.Data;
using Assets.Scripts.GameEnvironment.UI;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Assets.Scripts.GameEnvironment.Units
{
    public class Guard : Unit
    {
        [SerializeField] private CardData _guardUpgrade;
        [SerializeField] private Button _guardButton;
        [SerializeField] private Enemy _enemy;

        private Health _health;
        private Health _enemyHealth;

        public Enemy Enemy => _enemy;
        public CardData GuardApgrade => _guardUpgrade;

        public event UnityAction<Guard, GuardSpawner> OnGuardPressed;       

        protected override void Start()
        {
            _health = GetComponent<Health>();
            _health.HealthChanged += UpdateHealth;
            _damage = _cardData.Damage;
            _damageAmount.text = _damage.ToString();
            _startPosition = transform.position;
        }               

        private void OnEnable()
        {
            _guardButton.onClick.AddListener(PassGuard);
        }        

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.TryGetComponent(out Enemy enemy))
            {
                _enemy = enemy;
            }
        }

        private void UpdateHealth(int value) =>
            _healthAmount.text = value.ToString();

        private void PassGuard()
        {
            OnGuardPressed?.Invoke(this, GetComponentInParent<GuardSpawner>());
        }
    }
}
