using System.Collections.Generic;
using System.Linq;
using Data;
using GameEnvironment.GameLogic.CardFolder;
using GameEnvironment.UI.PlayerWallet;
using Infrastructure.Services;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GameEnvironment.UI
{
    public class Academy : BaseWindow, ISaveProgress
    {
        [SerializeField] private PlayerMoney _playerMoney;
        [SerializeField] private TMP_Text _materialsAmount;

        private int _playersLeadership;
        private int _playersHandCapacity;
        private List<CardData> _availableGuards = new List<CardData>();
        private List<string> _restoredBuildings = new List<string>();

        public PlayerMoney PlayerMoney => _playerMoney;

        public TMP_Text MaterialsAmount => _materialsAmount;

        public List<CardData> AvailableGuards => _availableGuards;

        public List<string> RestoredBuildings => _restoredBuildings;

        private void Start()
        {
            _materialsAmount.text = _playerMoney.Materials.ToString();
        }

        public void AddHiredGuards(List<CardData> guardDatas, string buildName)
        {
            _availableGuards.AddRange(guardDatas);
            _restoredBuildings.Add(buildName);
        }

        public void IncreaseHandCapacity() => 
            _playersHandCapacity++;

        public void IncreaseLeadership() => 
            _playersLeadership++;

        public void Load(PlayerProgress progress)
        {
            _availableGuards = progress.WorldData.AvailableGuards.ToList();
            _restoredBuildings = progress.WorldData.RestoredBuildings.ToList();
            _playersLeadership = progress.PlayerStats.Leadership;
            _playersHandCapacity = progress.PlayerStats.HandCapacity;
        }

        public void Save(PlayerProgress progress)
        {
            progress.WorldData.AvailableGuards = _availableGuards.ToList();
            progress.WorldData.RestoredBuildings = _restoredBuildings.ToList();
            progress.PlayerStats.Leadership = _playersLeadership;
            progress.PlayerStats.HandCapacity = _playersHandCapacity;
        }
    }
}