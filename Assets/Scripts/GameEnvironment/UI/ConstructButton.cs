using System;
using System.Collections.Generic;
using Data;
using GameEnvironment.GameLogic.CardFolder;
using GameEnvironment.UI.PlayerWallet;
using UnityEngine;
using UnityEngine.UI;

namespace GameEnvironment.UI
{
    public class ConstructButton : MonoBehaviour
    {
        [SerializeField] private BuildingType _buildingType;
        [SerializeField] private string _name;
        [SerializeField] private int _requiredMaterials;
        [SerializeField] private Sprite _restoredSprite;
        [SerializeField] private List<CardData> _guardDatas;

        private Button _button;
        private Academy _academy;
        private PlayerMoney _playerMoney;

        private void Awake()
        {
            _button = GetComponent<Button>();
            _academy = GetComponentInParent<Academy>();
            _playerMoney = _academy.PlayerMoney;

            foreach (var buildName in _academy.RestoredBuildings)
            {
                if (buildName == _name) 
                    SetRestored();
            }
        }
        
        private void OnEnable()
        {
            _button.onClick.AddListener(OnConstructButton);
        }

        private void OnDisable()
        {
            _button.onClick.RemoveListener(OnConstructButton);
        }

        private void SetRestored()
        {
            _button.image.sprite = _restoredSprite;
            _button.interactable = false;
        }
        
        private void OnConstructButton()
        {
            if (_requiredMaterials <= _playerMoney.Materials)
            {
                _button.image.sprite = _restoredSprite;
                _button.interactable = false;
                
                if (_buildingType == BuildingType.Guard) 
                    RestoreGuardBuilding();
                else if (_buildingType == BuildingType.GateHouse) 
                    _academy.IncreaseHandCapacity();
                else if (_buildingType == BuildingType.Amphitheater) 
                    _academy.IncreaseLeadership();
                
                _playerMoney.RemoveMaterials(_requiredMaterials, _academy.MaterialsAmount);
            }
        }

        private void RestoreGuardBuilding() => 
            _academy.AddHiredGuards(_guardDatas, _name);
    }
}