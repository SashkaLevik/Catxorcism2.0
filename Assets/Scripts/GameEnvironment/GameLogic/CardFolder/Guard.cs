using System.Collections.Generic;
using Data;
using GameEnvironment.GameLogic.RowFolder;
using GameEnvironment.UI;
using GameEnvironment.Units;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace GameEnvironment.GameLogic.CardFolder
{
    public class Guard : Unit
    {
        [SerializeField] private SkillCard _skillPrefab;
        [SerializeField] private RowType _rowType;
        [SerializeField] private SuitType _suit;
        [SerializeField] private CardData _guardUpgrade;
        [SerializeField] private Sprite _suitSprite;
        [SerializeField] private Button _guardButton;
        [SerializeField] private Enemy _enemy;
        [SerializeField] private List<SkillData> _skills;

        private int _slotIndex;
        private int _actionPoints;
        private bool _isTired = false;
        private Health _health;
        private ActionPointsViewer _APViewer;
        private SkillCard _currentSkill;

        public Enemy Enemy => _enemy;
        public CardData GuardApgrade => _guardUpgrade;

        public RowType RowType => _rowType;

        public event UnityAction<Guard> OnGuardPressed;       

        protected override void Start()
        {
            _APViewer = GetComponent<ActionPointsViewer>();
            _APViewer.UpdateAP(_cardData.ActionPoints);
            _actionPoints = _cardData.ActionPoints;
            _health = GetComponent<Health>();
            _health.HealthChanged += UpdateHealth;
            _health.DefenceChanged += UpdateDefence;
            _damage = _cardData.Damage;
            _damageAmount.text = _damage.ToString();
            _startPosition = transform.position;
        }

        public void GetSlotIndex(int index) => 
            _slotIndex = index;

        public void CreateSkillCards()
        {
            foreach (var skill in _skills)
            {
                _currentSkill = Instantiate(_skillPrefab, transform.position, Quaternion.identity);
                _currentSkill.Init(skill);
            }
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

        public void InitEnemy(Enemy enemy) =>
            _enemy = enemy;

        public void OnSkillUsed(int requiredAP)
        {
            _actionPoints -= requiredAP;

            if (_actionPoints < 0)
            {
                _isTired = true;
                _actionPoints = 0;
            }
            
            _APViewer.UpdateAP(_actionPoints);
        }

        private void Reset()
        {
            _actionPoints = _cardData.ActionPoints;
            _APViewer.UpdateAP(_actionPoints);
        }
        
        private void UpdateHealth(int value) =>
            _healthAmount.text = value.ToString();

        private void PassGuard()
        {
            OnGuardPressed?.Invoke(this);
        }
    }
}
