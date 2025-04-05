using System.Collections.Generic;
using Data;
using GameEnvironment.UI;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace GameEnvironment.GameLogic.CardFolder
{
    public class Guard : Unit
    {
        [SerializeField] private SuitType _suit;
        [SerializeField] private CardData _guardUpgrade;
        [SerializeField] private Image _suitSprite;
        [SerializeField] private Button _guardButton;
        [SerializeField] private Enemy _enemy;
        [SerializeField] private List<SkillCard> _skillCards;

        private bool _isTired = false;
        private bool _isOnField = false;
        private ActionPointsViewer _APViewer;
        private DragController _dragController;
        private BattleHud _battleHud;
        private EnemyGuard _enemyGuard;
        
        public bool IsOnField => _isOnField;

        public int ActionPoints { get; private set; }

        public Enemy Enemy => _enemy;

        public CardData GuardUpgrade => _guardUpgrade;

        public List<SkillCard> SkillCards => _skillCards;

        public event UnityAction<Guard> OnGuardPressed;

        public event UnityAction<int> APChanged;

        protected override void Start()
        {
            base.Start();
            ActionPoints = _cardData.ActionPoints;
            _APViewer = GetComponent<ActionPointsViewer>();
            Health.Died += OnGuardDie;
        }

        private void OnEnable()
        {
            _guardButton.onClick.AddListener(PassGuard);
        }
        
        public void AddOnField() => 
            _isOnField = true;

        public void InitEnemy(Enemy enemy) =>
            _enemy = enemy;

        public void Init(DragController dragController) => 
            _dragController = dragController;

        public void TryGetEnemy(BattleHud battleHud)
        {
            if (GetEnemy(battleHud.EnemyFrontRow)){}
            else if (GetEnemy(battleHud.EnemyBackRow)){}
            else
                _currentEnemy = _enemy;
        }

        public void ResetAP()
        {
            ActionPoints = _cardData.ActionPoints;
            _APViewer.ResetAP();
        }

        protected override void OnMouseEnter()
        {
            base.OnMouseEnter();

            if (_isOnField && _dragController.CurrentSkill != null)
            {
                if (_dragController.CurrentSkill.Type == SkillType.Attack)
                    OnSkillEnter(_dragController.CurrentSkill.AppliedValue);
            }
        }

        protected override void OnMouseExit()
        {
            base.OnMouseExit();

            if (_isOnField && _dragController.CurrentSkill != null)
            {
                if (_dragController.CurrentSkill.Type == SkillType.Attack) 
                    OnSkillExit();
            }
        }

        public override void Activate()
        {
            base.Activate();
            gameObject.layer = Layer.Draggable;
        }

        public override void Disactivate()
        {
            base.Disactivate();
            gameObject.layer = Layer.UI;
        }

        public void OnSkillPlayed(SkillCard skill)
        {
            ActionPoints -= skill.RequiredAP;
            APChanged?.Invoke(skill.RequiredAP);

            if (ActionPoints < 0)
            {
                ActionPoints = 0;
                _isTired = true;
            }

            if (skill.Type == SkillType.Attack) 
                OnSkillExit();
        }

        private void OnSkillEnter(int value)
        {
            _damage = _defaultDamage;
            
            switch (CurrentEnemy.IsMarked)
            {
                case false when IsCursed:
                    _damage += value / 2;
                    break;
                case true when IsCursed:
                    _damage += value;
                    break;
                case true:
                    _damage += value * 2;
                    break;
                default:
                    _damage += value;
                    break;
            }

            _damageAmount.text = _damage.ToString();
        }

        private void OnSkillExit() => 
            _damageAmount.text = _defaultDamage.ToString();

        private void OnGuardDie(Unit unit)
        {
            _isOnField = false;
            //_row.GuardSlots[_slotIndex].Activate();
        }
        
        private void PassGuard()
        {
            OnGuardPressed?.Invoke(this);
        }
    }
}
