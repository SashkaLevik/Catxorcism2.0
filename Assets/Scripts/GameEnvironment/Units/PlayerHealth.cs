using System.Collections;
using UnityEngine;

namespace Assets.Scripts.GameEnvironment.Units
{
    public class PlayerHealth : Health
    {
        private Player _player;        

        private void Start()
        {
            _player = GetComponent<Player>();
            CurrentHP = _player.CardData.Health;
            MaxHP = _player.CardData.Health;
        }

        protected override void Die()
        {
            base.Die();
            Destroy(gameObject, 0.5f);
        }
    }
}