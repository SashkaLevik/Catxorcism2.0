using Assets.Scripts.Data;
using Assets.Scripts.GameEnvironment.UI;
using Assets.Scripts.Infrastructure.Services;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.GameEnvironment.Units
{
    public class Player : Unit
    {
        [SerializeField] private PlayerType _playerType;

        private bool _isAttacking;
        private float _animationDelay = 0.3f;
        private Health _health;
        private List<GuardSpawner> _guardSlots;

        public PlayerType Type => _playerType;
        public bool IsAttacking => _isAttacking;

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

        public void InitSlots(List<GuardSpawner> guardSlots) =>
            _guardSlots = guardSlots.ToList();

        public void RiseDamage(int value)
        {
            _damage += value;
            _damageAmount.text = _damage.ToString();
        }

        public void ReduceDamage()
        {
            _damage = _cardData.Damage;
            _damageAmount.text = _damage.ToString();
        }                        

        public void Attack(Enemy enemy)
        {
            StartCoroutine(AttackEnemy(enemy));           
        }               

        private IEnumerator AttackEnemy(Enemy enemy)
        {
            _isAttacking = true;
            EnemyAttacked?.Invoke(_isAttacking);

            foreach (var pos in _guardSlots)
            {
                if (pos.GetComponentInChildren<Guard>() != null)
                {
                    var guard = pos.GetComponentInChildren<Guard>();

                    if (guard.Enemy != null && guard.Enemy == enemy)
                    {
                        guard.GetComponent<AttackSystem>().Attack(guard, enemy);
                        yield return new WaitForSeconds(_animationDelay);
                    }
                }
            }

            yield return new WaitForSeconds(_animationDelay);

            if (enemy != null)
                _attackSystem.Attack(this, enemy);

            yield return new WaitForSeconds(_animationDelay);
            _isAttacking = false;
            EnemyAttacked?.Invoke(_isAttacking);           
        }      

        private void UpdateDefence(int value)=>
            _defenceAmount.text = value.ToString();

        private void UpdateHealth(int value)=>
            _healthAmount.text = value.ToString();       
    }
}