using System.Collections;
using System.Collections.Generic;
using GameEnvironment.GameLogic.RowFolder;
using GameEnvironment.UI;
using GameEnvironment.Units;
using UnityEngine;
using UnityEngine.Events;

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
            Debug.Log("enemyGetLeadership");
        }

        private void OnDestroy()
        {
            _health.HealthChanged -= UpdateHealth;
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