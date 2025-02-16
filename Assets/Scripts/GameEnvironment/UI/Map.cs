using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace GameEnvironment.UI
{
    public class Map : BaseWindow
    {
        public const string MarketDistrict = "MarketDistrict";
        public const string Port = "PortDistrict";
        public const string Mage = "MageDistrict";

        [SerializeField] private Button _market;
        [SerializeField] private Button _port;

        public event UnityAction<string> LevelLoaded;

        private void OnEnable()
        {
            _market.onClick.AddListener(LoadMarketDistrict);
            //_jungle.onClick.AddListener(LoadJungle);
        }             


        private void LoadMarketDistrict()
            => LevelLoaded?.Invoke(MarketDistrict); 
    }
}