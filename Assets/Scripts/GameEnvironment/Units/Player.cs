using Assets.Scripts.Data;
using Assets.Scripts.Infrastructure.Services;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.GameEnvironment.Units
{
    public class Player : Unit, ISaveProgress
    {
        [SerializeField] private PlayerType _playerType;

        private Health _health;
        private List<Guard> _guards = new List<Guard>();

        public PlayerType Type => _playerType;

        public event UnityAction<bool> EnemyAttacked;        

        protected override void Start()
        {
            _attackSystem = GetComponent<AttackSystem>();
            _health = GetComponent<Health>();
            _damage = _cardData.Damage;
            _damageAmount.text = _damage.ToString();
            _health.HealthChanged += UpdateHealth;
            _health.DefenceChanged += UpdateDefence;
            _startPosition = transform.position;
        }        

        private void OnDestroy()
        {
            _health.HealthChanged -= UpdateHealth;
            _health.DefenceChanged -= UpdateDefence;
        }

        public void RemoveGuard(Guard guard)=>
            _guards.Remove(guard);

        public void AddGuard(Guard guard)=>
            _guards.Add(guard);

        public void Attack(Enemy enemy)
        {
            StartCoroutine(AttackEnemy(enemy));           
        }               

        private IEnumerator AttackEnemy(Enemy enemy)
        {
            EnemyAttacked?.Invoke(true);
            
            foreach (var guard in _guards)
            {
                if (guard.Enemy != null && guard.Enemy == enemy)
                {
                    guard.GetComponent<AttackSystem>().Attack(guard, enemy);
                    yield return new WaitForSeconds(0.5f);
                }
            }

            if (enemy != null)
            {
                _attackSystem.Attack(this, enemy);                
            }
            
            EnemyAttacked?.Invoke(false);
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