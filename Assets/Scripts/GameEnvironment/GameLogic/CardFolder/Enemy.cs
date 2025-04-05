using System.Collections.Generic;
using GameEnvironment.UI;
using UnityEngine;

namespace GameEnvironment.GameLogic.CardFolder
{
    public class Enemy : Unit
    {
        [SerializeField] private List<EnemyGuard> _enemyGuards;

        private int _leadership;
        private Player _player;
        private BattleHud _battleHud;
        private EnemyGuard _spawnedGuard;
        private RectTransform _guardSlot;

        public int Leadership => _leadership;

        public List<EnemyGuard> Guards => _enemyGuards;
        
        protected override void Start()
        {
            base.Start();
            _leadership = _cardData.ActionPoints;
        }

        private void OnDestroy()
        {
            Health.HealthChanged -= UpdateHealth;
        }

        public void InitBattle(BattleHud battleHud, Player player)
        {
            _battleHud = battleHud;
            _player = player;
        }
            

        public void Attack()
        {
                                  
        }
    }
}