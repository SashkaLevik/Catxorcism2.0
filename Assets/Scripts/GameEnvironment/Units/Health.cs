using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.GameEnvironment.Units
{
    public class Health : MonoBehaviour, IHealth
    {
        protected int _currentHealth;
        protected int _maxHealth;
        protected int _maxDefence;
        protected int _defence;
        protected int _defendingDamage;
        protected bool _isDefending;
        private Unit _unit;

        public event UnityAction Died;
        public event UnityAction<int> HealthChanged;
        public event UnityAction<int> DefenceChanged;

        private void Start()
        {
            _unit = GetComponent<Unit>();
            _maxDefence = _unit.CardData.Health;
            CurrentHP = _unit.CardData.Health;
            MaxHP = _unit.CardData.Health;
        }

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

            if (Defence > _maxDefence)
                Defence = _maxDefence;

            DefenceChanged?.Invoke(Defence);
        }            

        protected virtual void Die()
        {
            Died?.Invoke();            
            Destroy(gameObject, 0.2f);
        }
    }
}