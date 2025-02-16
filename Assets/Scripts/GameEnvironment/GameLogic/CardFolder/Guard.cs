using Data;
using GameEnvironment.UI;
using GameEnvironment.Units;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace GameEnvironment.GameLogic.CardFolder
{
    public class Guard : Unit
    {
        [SerializeField] private RowType _rowType;
        [SerializeField] private SuitType _suit;
        [SerializeField] private CardData _guardUpgrade;
        [SerializeField] private Sprite _suitSprite;
        [SerializeField] private Button _guardButton;
        [SerializeField] private Enemy _enemy;

        private Health _health;

        public Enemy Enemy => _enemy;
        public CardData GuardApgrade => _guardUpgrade;

        public RowType RowType => _rowType;

        public event UnityAction<Guard, GuardSpawner> OnGuardPressed;       

        protected override void Start()
        {
            _health = GetComponent<Health>();
            _health.HealthChanged += UpdateHealth;
            _health.DefenceChanged += UpdateDefence;
            _damage = _cardData.Damage;
            _damageAmount.text = _damage.ToString();
            _startPosition = transform.position;
        }

        private void UpdateDefence(int value)
        {
            if (value > 0) 
                _shield.SetActive(true);

            if (value < 0)
            {
                value = 0;
                _shield.SetActive(false);
            }
            
            _defenceAmount.text = value.ToString();

        }

        private void OnEnable()
        {
            _guardButton.onClick.AddListener(PassGuard);
        }

        public void ActivateGuard()
        {
            GetComponent<Button>().interactable = true;
        }

        public void InitEnemy(Enemy enemy) =>
            _enemy = enemy;        

        private void UpdateHealth(int value) =>
            _healthAmount.text = value.ToString();

        private void PassGuard()
        {
            OnGuardPressed?.Invoke(this, GetComponentInParent<GuardSpawner>());
        }
    }
}
