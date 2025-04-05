using System;
using Data;
using GameEnvironment.GameLogic.RowFolder;
using GameEnvironment.GameLogic.SkillEffects;
using GameEnvironment.Units;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace GameEnvironment.GameLogic.CardFolder
{
    public class Unit : Card
    {
        [SerializeField] private EffectsReceiver _effectsReceiver;
        [SerializeField] protected RowType _rowType;
        [SerializeField] protected CardData _cardData;
        [SerializeField] protected TMP_Text _healthAmount;
        [SerializeField] protected TMP_Text _defenceAmount;
        [SerializeField] protected TMP_Text _damageAmount;
        [SerializeField] protected Transform _firePos;
        [SerializeField] protected GameObject _shield;

        protected int _damage;
        protected int _defaultDamage;
        protected int _slotIndex;
        protected bool _isStunned;
        protected bool _isMarked;
        protected bool _isCursed;
        protected Unit _currentEnemy;
        protected Row _row;

        public CardData CardData => _cardData;
        public Transform FirePos => _firePos;
        public int Damage => _damage;
        public int SlotIndex => _slotIndex;
        public Row UnitRow => _row;
        public RowType RowType => _rowType;
        public Unit CurrentEnemy => _currentEnemy;
        public EffectsReceiver EffectsReceiver => _effectsReceiver;

        public Health Health { get; private set; }

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

        private void Awake()
        {
            Health = GetComponent<Health>();
            Health.HealthChanged += UpdateHealth;
            Health.DefenceChanged += UpdateDefence;
        }

        protected override void Start()
        {
            base.Start();
        }

        public void InitRow(Row row, int index)
        {
            _row = row;
            _slotIndex = index;
        }

        public void GetSkillEffect(int value, SkillCard skill)
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

        public void OnDefence(int value) => 
            Health.RiseDefence(value);

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
    }
}