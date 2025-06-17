using System.Collections.Generic;
using System.Linq;
using Data;
using GameEnvironment.UI.PlayerWallet;
using Infrastructure.Services;
using Infrastructure.States;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GameEnvironment.UI
{
    public class MenuHud : MonoBehaviour, ISaveProgress
    {
        private const string BattleScene = "BattleScene";
        
        [SerializeField] private TMP_Text _crystals;
        [SerializeField] private Canvas _canvas;
        [SerializeField] private AudioSource _mainTheme;
        [SerializeField] private PlayerMoney _playerMoney;
        [SerializeField] private Button _play;
        [SerializeField] private Button _continue;
        [SerializeField] private Button _academyButton;
        [SerializeField] private Button _openSettings;
        [SerializeField] private GameObject _playersRoom;
        [SerializeField] private GameObject _academy;
        [SerializeField] private GameObject _startDeckCreator;
        [SerializeField] private GameObject _settings;
        [SerializeField] private BuyButton _buyButton;
        [SerializeField] private Warning _warning;
        [SerializeField] private List<CardData> _allPlayers;

        private string _level;
        private string _currentPlayerName;
        private CardData _currentPlayerData;
        private IGameStateMachine _stateMachine;
        private ISaveLoadService _saveLoadService;
        private PlayerProgress _progress;

        public PlayerMoney PlayerMoney => _playerMoney;
        
        public GameObject PlayersRoom => _playersRoom;

        public GameObject Academy => _academy;

        public GameObject Settings => _settings;

        public GameObject StartDeckCreator => _startDeckCreator;

        private void Awake()
        {
            _saveLoadService = AllServices.Container.Single<ISaveLoadService>();
            _stateMachine = AllServices.Container.Single<IGameStateMachine>();
            _canvas.worldCamera = Camera.main;
        }

        private void Start()
        {
            _mainTheme.Play();

            if (_progress.WorldData.IsNewRun == false)
            {
                _continue.interactable = true;
                
                foreach (var player in _allPlayers.Where(player => player.EnName.Contains(_currentPlayerName)))
                    _currentPlayerData = player;
            }
        }

        private void OnEnable()
        {
            _play.onClick.AddListener(OpenPlayersRoom);
            _continue.onClick.AddListener(LoadGame);
            _academyButton.onClick.AddListener(EnterAcademy);
            _openSettings.onClick.AddListener(OpenSettings);
        }

        private void LoadGame() => 
            _stateMachine.Enter<LevelState, string>(_level, _currentPlayerData);

        private void EnterAcademy() => 
            _academy.SetActive(true);

        private void OpenPlayersRoom() => 
            _playersRoom.SetActive(true);

        private void OnDestroy()
        {
            _play.onClick.RemoveListener(OpenPlayersRoom);
            _continue.onClick.RemoveListener(LoadGame);
            _academyButton.onClick.RemoveListener(EnterAcademy);
            _openSettings.onClick.RemoveListener(OpenSettings);
        }

        private void OpenSettings() =>
            _settings.SetActive(true);

        public void Save(PlayerProgress progress)
        {
        }

        public void Load(PlayerProgress progress)
        {
            _progress = progress;
            _level = progress.WorldData.Level;
            _currentPlayerName = progress.WorldData.CurrentPlayer;
        }
    }
}