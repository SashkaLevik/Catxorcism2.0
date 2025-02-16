using Data;
using GameEnvironment.Units;
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
        [SerializeField] private Button _openSettings;
        [SerializeField] private Button _play;
        [SerializeField] private GameObject _settingsWindow;
        [SerializeField] private GameObject _playersRoom;
        [SerializeField] private GameObject _settings;
        [SerializeField] private BuyButton _buyButton;
        [SerializeField] private Warning _warning;

        private IGameStateMachine _stateMachine;
        private ISaveLoadService _saveLoadService;
        private PlayerProgress _progress;

        public PlayerMoney PlayerMoney => _playerMoney;

        public GameObject PlayersRoom => _playersRoom;

        public GameObject Settings => _settings;

        private void Awake()
        {
            _saveLoadService = AllServices.Container.Single<ISaveLoadService>();
            _stateMachine = AllServices.Container.Single<IGameStateMachine>();
            _canvas.worldCamera = Camera.main;
        }

        private void Start()
        {
            _crystals.text = _playerMoney.Crystals.ToString();
            _mainTheme.Play();
        }

        private void OnEnable()
        {
            _openSettings.onClick.AddListener(OpenSettings);
            _play.onClick.AddListener(OpenPlayersRoom);
        }

        private void OpenPlayersRoom() => 
            _playersRoom.SetActive(true);

        private void OnDestroy()
        {
            
            _openSettings.onClick.RemoveListener(OpenSettings);
            _play.onClick.RemoveListener(OpenPlayersRoom);
        }

        private void EnterGame(string sceneName, CardData cardData)
        {
            _progress.WorldData.IsNewGame = false;
            _saveLoadService.SaveProgress();
            _stateMachine.Enter<LevelState, string>(sceneName, cardData);
        }

        /*private void BuyPlayer()
        {
            if (_playerMoney.Crystals >= _currentData.ActivatePrice)
            {
                _openPlayers.Add(_currentData.EnName);

                _playerMoney.RemoveCrystal(_currentData.ActivatePrice, _crystals);
                _buyButton.gameObject.SetActive(false);
                _play.interactable = true;
            }
            else
                _warning.Show();
        }*/

       

        private void OpenSettings() =>
            _settingsWindow.SetActive(true);

        public void Save(PlayerProgress progress)
        {
        }

        public void Load(PlayerProgress progress)
        {
            
        }
    }
}