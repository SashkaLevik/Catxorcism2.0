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
        private ActionPointsViewer _APViewer;
        private BattleHud _battleHud;
        private EnemyGuard _enemyGuard;
        
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
        }

        private void OnEnable()
        {
            _guardButton.onClick.AddListener(PassGuard);
        }

        public void InitRow(Row row, int index, BattleHud battleHud)
        {
            _row = row;
            _slotIndex = index;
            _battleHud = battleHud;
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
        
        public void OnSkillUsed(int requiredAP)
        {
            _actionPoints -= requiredAP;
            APChanged?.Invoke(requiredAP);

            if (_actionPoints < 0)
            {
                _isTired = true;
                _actionPoints = 0;
            }
        }

        public void ResetAP()
        {
            _actionPoints = _cardData.ActionPoints;
            _APViewer.ResetAP();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.TryGetComponent(out SkillCard skillCard))
            {
                OnSkillEnter(skillCard.AppliedValue);
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.TryGetComponent(out SkillCard skillCard))
            {
                OnSkillExit(skillCard.AppliedValue);
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

        public override void Activate()
        {
            base.Activate();
            _collider.isTrigger = true;
        }

        public override void Disactivate()
        {
            base.Disactivate();
            _collider.isTrigger = false;
        }

        private void PassGuard()
        {
            OnGuardPressed?.Invoke(this);
        }
    }
}
