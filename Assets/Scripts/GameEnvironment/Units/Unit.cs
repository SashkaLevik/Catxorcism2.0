using Assets.Scripts.Data;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.GameEnvironment.Units
{
    public class Unit : Card
    {
        [SerializeField] protected CardData _cardData;
        [SerializeField] protected TMP_Text _healthAmount;
        [SerializeField] protected TMP_Text _defenceAmount;
        [SerializeField] protected TMP_Text _damageAmount;
        [SerializeField] protected AttackType _attackType;
        [SerializeField] protected Transform _firePos;

        protected int _damage;
        protected AttackSystem _attackSystem;

        public AttackType AttackType => _attackType;
        public CardData CardData => _cardData;
        public Transform FirePos => _firePos;
        public int Damage => _damage;
    }
}