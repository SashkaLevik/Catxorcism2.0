using System.Collections.Generic;
using Data;
using GameEnvironment.GameLogic.RowFolder;
using GameEnvironment.UI;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace GameEnvironment.GameLogic.CardFolder
{
    public class Guard : Unit
    {
        //[SerializeField] private RowType _rowType;
        [SerializeField] private LayerMask _guardTrigger;
        [SerializeField] private SuitType _suit;
        [SerializeField] private CardData _guardUpgrade;
        [SerializeField] private Image _suitSprite;
        [SerializeField] private Button _guardButton;
        [SerializeField] private Enemy _enemy;
        [SerializeField] private List<SkillCard> _skillCards;

        private int _slotIndex;
        private Row _row;
        private int _actionPoints;
        private bool _isTired = false;
        private bool _isOnField = false;
        private ActionPointsViewer _APViewer;
        private DragController _dragController;
        private EnemyGuard _enemyGuard;

        public bool IsOnField => _isOnField;

        public int ActionPoints => _actionPoints;

        public Enemy Enemy => _enemy;

        public EnemyGuard EnemyGuard => _enemyGuard;

        public CardData GuardUpgrade => _guardUpgrade;
        
        public List<SkillCard> SkillCards => _skillCards;

        public event UnityAction<Guard> OnGuardPressed;

        public event UnityAction<int> APChanged;

        protected override void Start()
        {
            base.Start();
            _actionPoints = _cardData.ActionPoints;
            _APViewer = GetComponent<ActionPointsViewer>();
            _health.Died += OnGuardDie;
        }

        private void OnEnable()
        {
            _guardButton.onClick.AddListener(PassGuard);
        }
        
        public void AddOnField() => 
            _isOnField = true;

        public void InitRow(Row row, int index, DragController dragController)
        {
            _row = row;
            _slotIndex = index;
            _dragController = dragController;
            _dragController.SkillPlayed += OnSkillPlayed;
        }

        public void InitEnemy(Enemy enemy) =>
            _enemy = enemy;

        public EnemyGuard TryGetEnemy(Row row)
        {
            if (row.GuardSlots[_slotIndex].GetComponentInChildren<EnemyGuard>() != null)
            {
                _enemyGuard = row.GuardSlots[_slotIndex].GetComponentInChildren<EnemyGuard>();
                return _enemyGuard;
            }

            return null;
        }

        public void ResetAP()
        {
            _actionPoints = _cardData.ActionPoints;
            _APViewer.ResetAP();
        }

        protected override void OnMouseEnter()
        {
            base.OnMouseEnter();
            if (_isOnField && _dragController.CurrentSkill != null)
            {
                if (_dragController.CurrentSkill.Type == SkillType.Attack)
                {
                    OnSkillEnter(_dragController.CurrentSkill.AppliedValue);
                }
            }
        }

        protected override void OnMouseExit()
        {
            base.OnMouseExit();
            if (_isOnField && _dragController.CurrentSkill != null)
            {
                if (_dragController.CurrentSkill.Type == SkillType.Attack)
                {
                    OnSkillExit(_dragController.CurrentSkill.AppliedValue);
                }
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

        private void OnSkillPlayed(SkillCard skill)
        {
            _actionPoints -= skill.RequiredAP;
            APChanged?.Invoke(skill.RequiredAP);

            if (_actionPoints < 0)
            {
                _isTired = true;
                _actionPoints = 0;
            }

            if (skill.Type == SkillType.Attack)
            {
                OnSkillExit(skill.AppliedValue);
            }
        }

        private void OnSkillEnter(int value)
        {
            _damage += value;
            _damageAmount.text = _damage.ToString();
        }

        private void OnSkillExit(int value)
        {
            _damage -= value;
            _damageAmount.text = _damage.ToString();
        }

        private void OnGuardDie()
        {
            _isOnField = false;
            _row.GuardSlots[_slotIndex].Activate();
            _dragController.SkillPlayed -= OnSkillPlayed;
        }
        
        private void PassGuard()
        {
            OnGuardPressed?.Invoke(this);
        }
    }
}
