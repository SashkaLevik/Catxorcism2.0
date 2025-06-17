using System;
using Data;
using GameEnvironment.GameLogic.CardFolder;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace GameEnvironment.Units
{
    public class Health : MonoBehaviour, IHealth
    {
        private int _currentHealth;
        private int _maxHealth;
        private int _defence;
        private int _defendingDamage;
        private float _animationDelay = 0.2f;
        protected bool _isDefending;
        private Unit _unit;
        
        public int Defence
        {
            get => _defence;
            set
            {
                _defence = value;
                DefenceChanged?.Invoke(Defence);
            }
        }

        public int CurrentHP
        {
            get => _currentHealth;
            set
            {
                _currentHealth = value;
                HealthChanged?.Invoke(CurrentHP);
            }
        }

        public int MaxHP
        {
            get => _maxHealth;
            set
            {
                _maxHealth = value;
            }
        } 

        public event UnityAction<Unit> Died;
        public event UnityAction<int> HealthChanged;
        public event UnityAction<int> DefenceChanged;

        private void Start()
        {
            _unit = GetComponent<Unit>();
            /*CurrentHP = _unit.CardData.Health;
            MaxHP = _unit.CardData.Health;*/
        }

        public void SetHealth(CardData data)
        {
            CurrentHP = data.Health;
            MaxHP = CurrentHP;
        }
        
        public virtual void TakeDamage(int damage)
        {
            _defendingDamage = damage - _defence;

            if (_defendingDamage < 0) _defendingDamage = 0;

            Defence -= damage;
            if (Defence < 0) Defence = 0;

            CurrentHP -= _defendingDamage;

            if (CurrentHP < 0) CurrentHP = 0;
            if (CurrentHP <= 0) Die();
        }

        public void TakeDirectDamage(int damage)
        {
            CurrentHP -= damage;

            if (CurrentHP < 0) CurrentHP = 0;
            if (CurrentHP <= 0) Die();
        }

        public void BreakDefence() => 
            Defence = 0;

        public void Heal(int value)
        {
            CurrentHP += value;

            if (CurrentHP > MaxHP)
                CurrentHP = MaxHP;

            HealthChanged?.Invoke(CurrentHP);
        }

        public void HealBarbarian(int value)
        {
            CurrentHP += value;          
            HealthChanged?.Invoke(CurrentHP);
        }          

        public void RiseDefence(int amount)
        {
            Defence += amount;
            DefenceChanged?.Invoke(Defence);
        }            

        protected virtual void Die()
        {
            Died?.Invoke(_unit);
            Destroy(gameObject);
        }
    }
}