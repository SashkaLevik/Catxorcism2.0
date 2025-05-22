using GameEnvironment.Units;
using TMPro;
using UnityEngine;

namespace GameEnvironment.UI.PlayerWallet
{
    public class Coin : Item
    {
        [SerializeField] private TMP_Text _coinAmount;        

        private int _randomAmount;

        public int Amount => _randomAmount;        

        protected override void Start()
        {
            _randomAmount = Random.Range(_minValue, _maxValue);
            _coinAmount.text = _randomAmount.ToString();
        }        
    }
}