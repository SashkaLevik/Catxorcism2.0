using Assets.Scripts.Data;
using System.Collections;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.GameEnvironment.Units
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