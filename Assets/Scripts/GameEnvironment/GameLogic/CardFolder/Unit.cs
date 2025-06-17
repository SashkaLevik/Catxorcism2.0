using Data;
using GameEnvironment.GameLogic.CardFolder.SkillCards;
using GameEnvironment.GameLogic.RowFolder;
using GameEnvironment.GameLogic.SkillEffects;
using GameEnvironment.GameLogic.SuitBuff;
using GameEnvironment.UI;
using GameEnvironment.Units;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace GameEnvironment.GameLogic.CardFolder
{
    public class Unit : Card
    {
        [SerializeField] private EffectsReceiver _effectsReceiver;
        [SerializeField] protected SuitType _suit;
        [SerializeField] protected RowType _rowType;
        [SerializeField] protected CardData _cardData;
        [SerializeField] protected TMP_Text _healthAmount;
        [SerializeField] protected TMP_Text _defenceAmount;
        [SerializeField] protected TMP_Text _damageAmount;
        [SerializeField] protected Transform _firePos;
        [SerializeField] protected GameObject _shield;
        [SerializeField] protected bool _canIgnoreObstacle;

        protected int _defaultdamage;
        protected int _currentDamage;
        protected int _slotIndex;
        protected bool _isStunned;
        protected bool _isMarked;
        protected bool _isCursed;
        protected bool _isBuffApplied;
        protected bool _canCounter;
        protected bool _canAttackTwice;
        private Buff _buff;
        protected Row _row;
        protected Unit _currentEnemy;
        protected BattleHud _battleHud;
        private RowCardSlot _cardSlot;

        public int SlotIndex => _slotIndex;

        public RowCardSlot CardSlot => _cardSlot;

        public CardData CardData => _cardData;
        public Transform FirePos => _firePos;
        public Row UnitRow => _row;

        public BattleHud BattleHud => _battleHud;

        public RowType RowType => _rowType;
        public Unit CurrentEnemy => _currentEnemy;
        public EffectsReceiver EffectsReceiver => _effectsReceiver;

        public Health Health { get; private set; }

        public int CurrentDamage
        {
            get => _currentDamage;
            protected set
            {
                _currentDamage = value;
                DamageChanged?.Invoke(CurrentDamage);
            }
        }

        public bool IsBuffApplied
        {
            get => _isBuffApplied;
            set => _isBuffApplied = value;
        }
        
        public bool IsStunned
        {
            get => _isStunned;
            set => _isStunned = value;
        }

        public bool IsMarked
        {
            get => _isMarked;
            set => _isMarked = value;
        }

        public bool IsCursed
        {
            get => _isCursed;
            set => _isCursed = value;
        }

        public bool CanCounter
        {
            get => _canCounter;
            set => _canCounter = value;
        }

        public bool CanAttackTwice
        {
            get => _canAttackTwice;
            set => _canAttackTwice = value;
        }
        
        public event UnityAction<int> DamageChanged;

        protected override void Awake()
        {
            base.Awake();
            Health = GetComponent<Health>();
            _buff = GetComponent<Buff>();
            Health.HealthChanged += UpdateHealth;
            Health.DefenceChanged += UpdateDefence;
            DamageChanged += UpdateDamage;
        }

        protected override void Start()
        {
            base.Start();
            /*_defaultdamage = _cardData.Damage;
            CurrentDamage = _defaultdamage;*/
        }

        private void OnDestroy()
        {
            Health.HealthChanged -= UpdateHealth;
            Health.DefenceChanged -= UpdateDefence;
            DamageChanged -= UpdateDamage;
        }

        public virtual void ConstructUnit(CardData data)
        {
            _cardData = data;
            _defaultdamage = _cardData.Damage;
            CurrentDamage = _defaultdamage;
            Health.SetHealth(data);
        }
        
        public void SetRow(BattleHud battleHud, Row row, RowCardSlot cardSlot)
        {
            _battleHud = battleHud;
            _row = row;
            _cardSlot = cardSlot;
            _slotIndex = row.GuardSlots.IndexOf(cardSlot);
        }

        public void Attack(SkillCard skill, Unit target, int damage)
        {
            ObstacleSkill currentObstacle = null;
            
            if (_battleHud.MiddleRow.RowSlots[_slotIndex].GetComponentInChildren<ObstacleSkill>() != null)
                currentObstacle = _battleHud.MiddleRow.RowSlots[_slotIndex].GetComponentInChildren<ObstacleSkill>();

            if (currentObstacle != null)
            {
                if (currentObstacle.IsHarmful && !_canIgnoreObstacle) 
                    Health.TakeDamage(currentObstacle.AppliedValue);

                switch (currentObstacle.IsBlocked)
                {
                    case true when _canIgnoreObstacle:
                    case false:
                        AttackEnemy(skill, target, damage);
                        break;
                    default:
                        currentObstacle.TakeDamage();
                        break;
                }
            }
            else
                AttackEnemy(skill, target, damage);
        }

        private void AttackEnemy(SkillCard skill, Unit target, int damage)
        {
            if (target != null) 
                target.OnAttackSkill(damage, skill);

            if (CanAttackTwice && target != null)
                target.OnAttackSkill(Mathf.RoundToInt(damage / 2), skill);
            
            if (target.CanCounter) 
                OnAttackSkill(Mathf.RoundToInt(damage / 2), skill);
        }
        
        public void OnAttackSkill(int value, SkillCard skill)
        {
            switch (skill.Type)
            {
                case SkillType.Attack when IsMarked:
                    Health.TakeDamage(value * 2);
                    break;
                case SkillType.Attack:
                    Health.TakeDamage(value);
                    break;
                case SkillType.Defence:
                    Health.RiseDefence(value);
                    break;
            }

            _effectsReceiver.TryApplyEffect(skill);
        }

        public void RiseDamage(int value)
        {
            CurrentDamage += value;
        }

        public void ResetDamage()
        {
            CurrentDamage = _defaultdamage;
        }

        public void OnDefence(int value) => 
            Health.RiseDefence(value);

        public void CheckSuitMatch(Row row)
        {
            if (row.RowSuit == _suit)
            {
                if (!IsBuffApplied) _buff.ApplyBuff();
            }
            else
            {
                _buff.ResetBuff();
            }
        }
        
        protected Unit GetEnemy(Row row)
        {
            if (row.GuardSlots[_slotIndex].GetComponentInChildren<Unit>() != null)
            {
                _currentEnemy = row.GuardSlots[_slotIndex].GetComponentInChildren<Unit>();
                return _currentEnemy;
            }

            return null;
        }

        protected void UpdateDefence(int value)
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
        
        protected void UpdateHealth(int value) =>
            _healthAmount.text = value.ToString();
        
        private void UpdateDamage(int value) => 
            _damageAmount.text = value.ToString();
    }
}