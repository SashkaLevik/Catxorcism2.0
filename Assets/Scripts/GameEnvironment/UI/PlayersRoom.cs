﻿using System.Collections.Generic;
using System.Linq;
using Data;
using GameEnvironment.GameLogic.CardFolder;
using Infrastructure.Services;
using Infrastructure.States;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GameEnvironment.UI
{
    public class PlayersRoom : BaseWindow, ISaveProgress
    {
        [SerializeField] private TMP_Text _name;
        [SerializeField] private TMP_Text _description;
        [SerializeField] private Button _next;
        [SerializeField] private Button _previous;
        [SerializeField] private Button _mapButton;
        [SerializeField] private GameObject _map;
        [SerializeField] private PlayerPreview _player;
        [SerializeField] private RectTransform _playerPos;
        [SerializeField] private List<PlayerPreview> _players;

        private int _currentPlayerIndex;
        private CardData _currentPlayerData;
        private PlayerProgress _progress;
        private IGameStateMachine _stateMachine;
        private ISaveLoadService _saveLoadService;
        private List<string> _openPlayers = new List<string>();

        public CardData PlayerData => _currentPlayerData;

        protected override void Awake()
        {
            base.Awake();
            _saveLoadService = AllServices.Container.Single<ISaveLoadService>();
            _stateMachine = AllServices.Container.Single<IGameStateMachine>();
        }

        private void OnEnable()
        {
            if (_progress.WorldData.IsNewGame)
                _openPlayers.Add(_players[0].CardData.EnName);

            _currentPlayerData = _players[0].CardData;
            SetPlayer(_currentPlayerIndex);
            _mapButton.interactable = true;
            _next.onClick.AddListener(ChooseNext);
            _previous.onClick.AddListener(ChoosePrevious);
            _mapButton.onClick.AddListener(OpenMap);
        }

        private void OnDisable() => 
            RemovePlayer();

        private void OnDestroy()
        {
            _next.onClick.RemoveListener(ChooseNext);
            _previous.onClick.RemoveListener(ChoosePrevious);
            _mapButton.onClick.RemoveListener(OpenMap);
        }
        
        private void ChooseNext()
        {
            _currentPlayerIndex++;
            _mapButton.interactable = true;

            if (_currentPlayerIndex > _players.Count - 1)
                _currentPlayerIndex = 0;

            _currentPlayerData = _players[_currentPlayerIndex].CardData;

            if (IsOpen(_currentPlayerData) == false) 
                _mapButton.interactable = false;
            
            RemovePlayer();
            SetPlayer(_currentPlayerIndex);
        }

        private void ChoosePrevious()
        {
            _currentPlayerIndex--;
            _mapButton.interactable = true;

            if (_currentPlayerIndex < 0)
                _currentPlayerIndex = _players.Count - 1;

            _currentPlayerData = _players[_currentPlayerIndex].CardData;

            if (IsOpen(_currentPlayerData) == false) 
                _mapButton.interactable = false;
            
            RemovePlayer();
            SetPlayer(_currentPlayerIndex);
        }

        private void OpenMap() => 
            _map.SetActive(true);

        private void RemovePlayer()
        {
            if (_player != null)
                Destroy(_player.gameObject);
            
            _player = null;
        }
        
        private void SetPlayer(int playerIndex)
        {
            _player = Instantiate(_players[playerIndex], _playerPos);
            /*_description.text = GetLocalizedDescription(data);
            _name.text = GetLocalizedName(data);*/
        }
        
        private string GetLocalizedName(CardData cardData)
        {
            if (Application.systemLanguage == SystemLanguage.English)
                return cardData.EnName;
            else
                return cardData.RuName;
        }
        
        private string GetLocalizedDescription(CardData cardData)
        {
            if (Application.systemLanguage == SystemLanguage.English)
                return cardData.EnDescription;
            else
                return cardData.RuDescription;
        }
        
        private bool IsOpen(CardData data)
        {
            foreach (var name in _openPlayers)
                if (data.EnName == name) return true;

            return false;
        }

        public void Load(PlayerProgress progress)
        {
            _progress = progress;

            if (progress.WorldData.IsNewGame == false)
                _openPlayers = progress.PlayerStats.OpenPlayers.ToList();
        }

        public void Save(PlayerProgress progress)
        {
            progress.PlayerStats.OpenPlayers = _openPlayers.ToList();
        }
    }
}