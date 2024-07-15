using TMPro;
using UnityEngine;

namespace Assets.Scripts.GameEnvironment.Units
{
    public class Shield : Item
    {
        [SerializeField] private TMP_Text _shieldAmount;
        [SerializeField] private GameObject _shield;
        [SerializeField] private GameObject _scroll;
        [SerializeField] private GameObject _potion;

        private int _randomAmount;

        public int Amount => _randomAmount;

        protected override void Start()
        {
            _randomAmount = Random.Range(_minValue, _maxValue);
            _shieldAmount.text = _randomAmount.ToString();
        }

        public void ChangeSprite(PlayerType playerType)
        {
            _scroll.SetActive(false);
            _shield.SetActive(false);
            _potion.SetActive(false);

            if (playerType == PlayerType.Knight)
                _shield.SetActive(true);
            else if (playerType == PlayerType.Mage)
                _scroll.SetActive(true);
            else if (playerType == PlayerType.Barbarian)
                _potion.SetActive(true);
        }
    }
}