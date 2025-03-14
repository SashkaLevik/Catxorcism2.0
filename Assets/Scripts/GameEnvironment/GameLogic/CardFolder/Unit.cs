using Data;
using GameEnvironment.GameLogic.RowFolder;
using GameEnvironment.Units;
using TMPro;
using UnityEngine;

namespace GameEnvironment.GameLogic.CardFolder
{
    public class Unit : Card
    {
        [SerializeField] protected RowType _rowType;
        [SerializeField] protected CardData _cardData;
        [SerializeField] protected TMP_Text _healthAmount;
        [SerializeField] protected TMP_Text _defenceAmount;
        [SerializeField] protected TMP_Text _damageAmount;
        [SerializeField] protected Transform _firePos;
        [SerializeField] protected GameObject _shield;

        protected int _damage;
        protected Health _health;

        public CardData CardData => _cardData;
        public Transform FirePos => _firePos;
        public int Damage => _damage;
        public RowType RowType => _rowType;

        
        protected override void Start()
        {
            base.Start();
            _health = GetComponent<Health>();
            _health.HealthChanged += UpdateHealth;
            _health.DefenceChanged += UpdateDefence;
            //_damage = _cardData.Damage;
            //_damageAmount.text = _damage.ToString();
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