using TMPro;
using UnityEngine;

namespace GameEnvironment.Units
{
    public class HealPotion : Item
    {
        [SerializeField] private TMP_Text _healAmount;

        private int _randomAmount;

        public int Amount => _randomAmount;

        protected override void Start()
        {
            _randomAmount = Random.Range(_minValue, _maxValue);
            _healAmount.text = _randomAmount.ToString();
        }
    }
}