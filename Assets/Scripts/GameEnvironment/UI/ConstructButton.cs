using System.Collections.Generic;
using Data;
using GameEnvironment.UI.PlayerWallet;
using UnityEngine;
using UnityEngine.UI;

namespace GameEnvironment.UI
{
    public class ConstructButton : MonoBehaviour
    {
        [SerializeField] private BuildingType _buildingType;
        [SerializeField] private string _buildName;
        [SerializeField] private int _requiredMaterials;
        [SerializeField] private Sprite _restoredSprite;
        [SerializeField] private List<CardData> _guards;

        private Button _button;
        private Academy _academy;
        private PlayerMoney _playerMoney;
        private RectTransform _buttonPosition;

        private void Awake()
        {
            _button = GetComponent<Button>();
            _academy = GetComponentInParent<Academy>();
            _playerMoney = _academy.PlayerMoney;
            _buttonPosition = _button.GetComponent<RectTransform>();

            foreach (var buildName in _academy.RestoredBuildings)
            {
                if (buildName == _buildName) 
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
                _academy.OnConstruct(_requiredMaterials, _buttonPosition);
                _button.image.sprite = _restoredSprite;
                _button.interactable = false;

                if (_buildingType == BuildingType.Guard) 
                    _academy.RestoreBuilding(_guards, _buildName);
                else if (_buildingType == BuildingType.GateHouse) 
                    _academy.IncreaseHandCapacity();
                else if (_buildingType == BuildingType.Amphitheater) 
                    _academy.IncreaseLeadership();
                
                _playerMoney.RemoveMaterials(_requiredMaterials, _academy.MaterialsAmount);
            }
        }
    }
}