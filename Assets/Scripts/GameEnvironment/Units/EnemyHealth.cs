using System.Collections;
using UnityEngine;

namespace Assets.Scripts.GameEnvironment.Units
{
    public class EnemyHealth : Health
    {
        private Enemy _enemy;        

        private void Start()
        {
            _enemy = GetComponent<Enemy>();
            CurrentHP = _enemy.CardData.Health;
            MaxHP = _enemy.CardData.Health;
        }

        protected override void Die()
        {
            base.Die();
            Destroy(gameObject);
        }
    }
}