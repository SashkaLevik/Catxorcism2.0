using System.Collections.Generic;
using System.Linq;
using Data;
using GameEnvironment.Units;
using Infrastructure.Services;
using TMPro;
using UnityEngine;

namespace GameEnvironment.GameLogic.CardFolder
{
    public class Player : Unit, ISaveProgress
    {
        [SerializeField] private PlayerType _playerType;
        //[SerializeField] private TMP_Text _healthAmount;
        //[SerializeField] private TMP_Text _defenceAmount;
        [SerializeField] private List<Card> _startingGuards;

        private bool _isAttacking;
        private float _animationDelay = 0.3f;
        private Health _health;
        private PlayerProgress _progress;
        private List<Card> _playerDeck = new List<Card>();

        public PlayerType Type => _playerType;
        public bool IsAttacking => _isAttacking;


        protected override void Start()
        {
            /*if (_progress.WorldData.IsNewRun)
            {
                _playerDeck = _startingGuards.ToList();
            }*/
            _health = GetComponent<Health>();
            _health.HealthChanged += UpdateHealth;
            _health.Died += OnPlayerDie;
            _health.DefenceChanged += UpdateDefence;
            _startPosition = transform.position;
        }

        private void OnPlayerDie()
        {
            _health.HealthChanged -= UpdateHealth;
            _health.DefenceChanged -= UpdateDefence;
        }

        private void OnDestroy()
        {
           
        }
        

        private void UpdateDefence(int value)=>
            _defenceAmount.text = value.ToString();

        private void UpdateHealth(int value)=>
            _healthAmount.text = value.ToString();

        public void Load(PlayerProgress progress)
        {
            
        }

        public void Save(PlayerProgress progress)
        {
        }
    }
}