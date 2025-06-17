using System.Collections.Generic;
using System.Linq;
using Data;
using GameEnvironment.GameLogic.CardFolder;
using Infrastructure.Services;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GameEnvironment.UI
{
    public class PlayersRoom : BaseWindow, ISaveProgress
    {
        [SerializeField] private TMP_Text _name;
        [SerializeField] private Button _next;
        [SerializeField] private Button _previous;
        [SerializeField] private Button _hireGuards;
        [SerializeField] private GameObject _startDeckCreator;
        [SerializeField] private RectTransform _playerPos;
        [SerializeField] private List<CardData> _players;
        [SerializeField] private List<RectTransform> _descriptionSlots;

        private GameObject _player;
        private int _currentPlayerIndex;
        private CardData _currentPlayerData;
        private PlayerProgress _progress;
        private List<string> _openPlayers = new List<string>();

        public CardData PlayerData => _currentPlayerData;

        public List<CardData> Players => _players;

        private void OnEnable()
        {
            if (_progress.WorldData.IsNewGame)
                _openPlayers.Add(_players[0].EnName);

            _currentPlayerData = _players[0];
            SetPlayer(_currentPlayerIndex);
            _hireGuards.interactable = true;
            _next.onClick.AddListener(ChooseNext);
            _previous.onClick.AddListener(ChoosePrevious);
            _hireGuards.onClick.AddListener(OpenStartDeckCreator);
        }

        private void OnDisable() => 
            RemovePlayer();

        protected void OnDestroy()
        {
            _next.onClick.RemoveListener(ChooseNext);
            _previous.onClick.RemoveListener(ChoosePrevious);
            _hireGuards.onClick.RemoveListener(OpenStartDeckCreator);
        }

        public void AddPlayer(string ID)
        {
            foreach (var playerID in _openPlayers)
            {
                if (!_openPlayers.Contains(ID))
                {
                    _openPlayers.Add(ID);
                }
            }
        }
            

        private void ChooseNext()
        {
            _currentPlayerIndex++;
            _hireGuards.interactable = true;

            if (_currentPlayerIndex > _players.Count - 1)
                _currentPlayerIndex = 0;

            _currentPlayerData = _players[_currentPlayerIndex];

            if (IsOpen(_currentPlayerData) == false) 
                _hireGuards.interactable = false;
            
            RemovePlayer();
            SetPlayer(_currentPlayerIndex);
        }

        private void ChoosePrevious()
        {
            _currentPlayerIndex--;
            _hireGuards.interactable = true;

            if (_currentPlayerIndex < 0)
                _currentPlayerIndex = _players.Count - 1;

            _currentPlayerData = _players[_currentPlayerIndex];

            if (IsOpen(_currentPlayerData) == false) 
                _hireGuards.interactable = false;
            
            RemovePlayer();
            SetPlayer(_currentPlayerIndex);
        }

        private void OpenStartDeckCreator() => 
            _startDeckCreator.SetActive(true);

        private void RemovePlayer()
        {
            if (_player != null)
                Destroy(_player);
            
            _player = null;
        }
        
        private void SetPlayer(int playerIndex)
        {
            _player = Instantiate(_players[playerIndex].CardPrefab, _playerPos);
            _player.GetComponent<Unit>().ConstructUnit(_players[playerIndex]);
            _name.text = GetLocalizedName(_players[playerIndex]);
            ShowSkillsDescription();
        }

        private void ShowSkillsDescription()
        {
            Player currentPlayer = _player.GetComponent<Player>();
            
            for (int i = 0; i < currentPlayer.PlayerSkills.Count; i++)
            {
                Instantiate(currentPlayer.PlayerSkills[i].SkillDescription, _descriptionSlots[i]);
            }
        }

        private string GetLocalizedName(CardData cardData)
        {
            if (Application.systemLanguage == SystemLanguage.English)
                return cardData.EnName;
            else
                return cardData.RuName;
        }

        private bool IsOpen(CardData data)
        {
            foreach (var playerName in _openPlayers)
                if (data.EnName == playerName) return true;

            return false;
        }

        public void Load(PlayerProgress progress)
        {
            _progress = progress;

            if (progress.WorldData.IsNewGame == false)
                _openPlayers = progress.WorldData.OpenPlayers.ToList();
        }

        public void Save(PlayerProgress progress)
        {
            progress.WorldData.OpenPlayers = _openPlayers.ToList();

            if (_currentPlayerData != null) 
                progress.WorldData.CurrentPlayer = _currentPlayerData.EnName;
        }
    }
}