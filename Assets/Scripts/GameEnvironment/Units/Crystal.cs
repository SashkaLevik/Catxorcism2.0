using TMPro;
using UnityEngine;

namespace Assets.Scripts.GameEnvironment.Units
{
    public class Crystal : Item
    {

        [SerializeField] private TMP_Text _crystalAmount;

        private int _randomAmount;

        public int Amount => _randomAmount;

        protected override void Start()
        {
            _randomAmount = Random.Range(_minValue, _maxValue);
            _crystalAmount.text = _randomAmount.ToString();
        }
    }
}