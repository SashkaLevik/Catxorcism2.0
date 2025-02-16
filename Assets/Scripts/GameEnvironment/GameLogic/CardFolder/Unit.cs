using Data;
using GameEnvironment.Units;
using TMPro;
using UnityEngine;

namespace GameEnvironment.GameLogic.CardFolder
{
    public class Unit : Card
    {
        [SerializeField] protected CardData _cardData;
        [SerializeField] protected TMP_Text _healthAmount;
        [SerializeField] protected TMP_Text _defenceAmount;
        [SerializeField] protected TMP_Text _damageAmount;
        [SerializeField] protected Transform _firePos;
        [SerializeField] protected GameObject _shield;

        protected int _damage;
        protected AttackSystem _attackSystem;

        public CardData CardData => _cardData;
        public Transform FirePos => _firePos;
        public int Damage => _damage;
        
    }
}