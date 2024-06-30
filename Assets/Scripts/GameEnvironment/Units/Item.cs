using UnityEngine;

namespace Assets.Scripts.GameEnvironment.Units
{
    public class Item : Card
    {
        [SerializeField] protected int _minValue;
        [SerializeField] protected int _maxValue;
        [SerializeField] protected AudioSource _clickSound;

        protected Player _player;

        public AudioSource Sound => _clickSound;

        public void Init(Player player) =>
            _player = player;
    }
}