using System.Collections.Generic;
using Data;
using GameEnvironment.GameLogic.CardFolder.SkillCards;
using GameEnvironment.UI;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace GameEnvironment.GameLogic.CardFolder
{
    public class Guard : Unit
    {
        [SerializeField] private CardData _guardUpgrade;
        [SerializeField] private Image _suitSprite;
        [SerializeField] private Button _guardButton;
        [SerializeField] private Enemy _enemy;
        [SerializeField] private SkillCard _buffCard;
        [SerializeField] private List<SkillCard> _skillCards;

        //private bool _isTired = false;
        private bool _isOnField = false;
        private ActionPointsViewer _APViewer;
        private DeckCreator _deckCreator;
        private DragController _dragController;
        //private BattleHud _battleHud;
        private EnemyGuard _enemyGuard;
        
        public bool IsOnField => _isOnField;

        public int ActionPoints { get; private set; }

        public Enemy Enemy => _enemy;

        public DeckCreator DeckCreator => _deckCreator;

        public CardData GuardUpgrade => _guardUpgrade;

        public SkillCard BuffCard => _buffCard;

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

        public void Construct(DragController dragController, DeckCreator deckCreator)
        {
            _dragController = dragController;
            _deckCreator = deckCreator;
        }

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
                //_isTired = true;
            }

            if (skill.Type == SkillType.Attack) 
                ResetDamage();
        }

        private void OnSkillEnter(int value)
        {
            switch (CurrentEnemy.IsMarked)
            {
                case false when IsCursed:
                    CurrentDamage += value / 2;
                    break;
                case true when IsCursed:
                    CurrentDamage += value;
                    break;
                case true:
                    CurrentDamage += value * 2;
                    break;
                default:
                    CurrentDamage += value;
                    break;
            }
        }

        private void OnSkillExit() =>
            ResetDamage();

        private void OnGuardDie(Unit unit)
        {
            _isOnField = false;
            unit.CardSlot.Clear();
            unit.Health.Died -= OnGuardDie;
        }
        
        private void PassGuard()
        {
            OnGuardPressed?.Invoke(this);
        }
    }
}
