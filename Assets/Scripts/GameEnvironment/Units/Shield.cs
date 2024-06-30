using TMPro;
using UnityEngine;

namespace Assets.Scripts.GameEnvironment.Units
{
    public class Shield : Item
    {
        [SerializeField] private TMP_Text _shieldAmount;
        [SerializeField] private GameObject _shield;
        [SerializeField] private GameObject _coin;

        private int _randomAmount;

        public int Amount => _randomAmount;

        protected override void Start()
        {
            _randomAmount = Random.Range(_minValue, _maxValue);
            _shieldAmount.text = _randomAmount.ToString();
        }

        public void ChangeSprite()
        {
            _shield.SetActive(false);
            _coin.SetActive(true);
        }
    }
}