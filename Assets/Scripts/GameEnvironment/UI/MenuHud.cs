using Assets.Scripts.Data;
using Assets.Scripts.GameEnvironment.Units;
using Assets.Scripts.Infrastructure.Services;
using Assets.Scripts.States;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Agava.YandexGames;
using Assets.Scripts.Infrastructure.GameManegment;

namespace Assets.Scripts.GameEnvironment.UI
{
    public class MenuHud : MonoBehaviour, ISaveProgress
    {
        private const string BattleScene = "BattleScene";

        [SerializeField] private TMP_Text _name;
        [SerializeField] private TMP_Text _description;
        [SerializeField] private TMP_Text _crystals;
        [SerializeField] private Canvas _canvas;
        [SerializeField] private AudioSource _mainTheme;
        [SerializeField] private PlayerMoney _playerMoney;
        [SerializeField] private Button _rewardAd;
        [SerializeField] private Button _openSettings;
        [SerializeField] private Button _closeSettings;
        [SerializeField] private Button _play;
        [SerializeField] private Button _next;
        [SerializeField] private Button _previous;
        [SerializeField] private Button _openGuardShop;
        [SerializeField] private Button _closeGuardShop;
        [SerializeField] private RectTransform _playerPos;
        [SerializeField] private Player _player;
        [SerializeField] private GameObject _settingsWindow;
        [SerializeField] private GameObject _guardShop;
        [SerializeField] private BuyButton _buyButton;
        [SerializeField] private Warning _warning;
        [SerializeField] private WebFocus _webFocus;
        [SerializeField] private List<CardData> _playerDatas;

        private int _currentPlayerIndex;
        private CardData _currentData;
        private IGameStateMachine _stateMachine;
        private ISaveLoadService _saveLoadService;
        private PlayerProgress _progress;
        private List<string> _openPlayers = new List<string>();

        public PlayerMoney PlayerMoney => _playerMoney;

        private void Awake()
        {
            _saveLoadService = AllServices.Container.Single<ISaveLoadService>();
            _stateMachine = AllServices.Container.Single<IGameStateMachine>();
            _canvas.worldCamera = Camera.main;
            YandexGamesSdk.GameReady();
        }

        private void Start()
        {
            _rewardAd.interactable = true;
            _crystals.text = _playerMoney.Crystals.ToString();            

            if (_progress.WorldData.IsNewGame == true)
                _openPlayers.Add(_playerDatas[0].EnName);

            _currentData = _playerDatas[0];
            SetPlayer(_currentData);
            _mainTheme.Play();
        }

        private void OnEnable()
        {
            _rewardAd.onClick.AddListener(ShowRewarded);
            _next.onClick.AddListener(ChooseNext);
            _previous.onClick.AddListener(ChoosePrevious);
            _openSettings.onClick.AddListener(OpenSettings);
            _closeSettings.onClick.AddListener(CloseSettings);
            _openGuardShop.onClick.AddListener(OpenGuardShop);
            _closeGuardShop.onClick.AddListener(CloseGuardShop);
            _buyButton.GetComponent<Button>().onClick.AddListener(BuyPlayer);
            _play.onClick.AddListener(() => EnterGame(BattleScene, _currentData));
        }

        private void OnDestroy()
        {
            _rewardAd.onClick.RemoveListener(ShowRewarded);
            _next.onClick.RemoveListener(ChooseNext);
            _previous.onClick.RemoveListener(ChoosePrevious);
            _openSettings.onClick.RemoveListener(OpenSettings);
            _closeSettings.onClick.RemoveListener(CloseSettings);
            _openGuardShop.onClick.RemoveListener(OpenGuardShop);
            _closeGuardShop.onClick.RemoveListener(CloseGuardShop);
            _buyButton.GetComponent<Button>().onClick.RemoveListener(BuyPlayer);
            _play.onClick.RemoveListener(() => EnterGame(BattleScene, _currentData));
        }

        private void EnterGame(string sceneName, CardData cardData)
        {
            _progress.WorldData.IsNewGame = false;
            _saveLoadService.SaveProgress();
            _stateMachine.Enter<LevelState, string>(sceneName, cardData);
        }     

        private void SetPlayer(CardData data)
        {
            _player = (Player)Instantiate(data.CardPrefab, _playerPos);
            _description.text = GetLocalizedDescription(data);
            _name.text = GetLocalizedName(data);
        }

        private void ChooseNext()
        {
            _currentPlayerIndex++;
            _play.interactable = true;
            _buyButton.gameObject.SetActive(false);

            if (_currentPlayerIndex > _playerDatas.Count - 1)
                _currentPlayerIndex = 0;

            _currentData = _playerDatas[_currentPlayerIndex];

            if (IsOpen(_currentData) == false)
            {
                _buyButton.gameObject.SetActive(true);
                _buyButton.GetComponentInChildren<TMP_Text>().text = _currentData.ActivatePrice.ToString();
                _buyButton.GetCard(_currentData);
                _play.interactable = false;
            }

            if (_player != null)
                Destroy(_player.gameObject);

            SetPlayer(_currentData);
        }

        private void ChoosePrevious()
        {
            _currentPlayerIndex--;
            _play.interactable = true;
            _buyButton.gameObject.SetActive(false);

            if (_currentPlayerIndex < 0)
                _currentPlayerIndex = _playerDatas.Count - 1;

            _currentData = _playerDatas[_currentPlayerIndex];

            if (IsOpen(_currentData) == false)
            {
                _buyButton.gameObject.SetActive(true);
                _buyButton.GetComponentInChildren<TMP_Text>().text = _currentData.ActivatePrice.ToString();
                _buyButton.GetCard(_currentData);
                _play.interactable = false;
            }

            if (_player != null)
                Destroy(_player.gameObject);

            SetPlayer(_currentData);
        }

        private void BuyPlayer()
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
        }

        private bool IsOpen(CardData data)
        {
            foreach (var name in _openPlayers)
                if (data.EnName == name) return true;

            return false;
        }

        private void OpenSettings() =>
            _settingsWindow.SetActive(true);

        private void CloseSettings() =>
            _settingsWindow.SetActive(false);

        private void OpenGuardShop() =>
            _guardShop.SetActive(true);

        private void CloseGuardShop() =>
            _guardShop.SetActive(false);

        private void ShowRewarded()
        {
            Agava.YandexGames.VideoAd.Show(OnOpenCallback, OnRewardedCallback, OnCloseCallback);
            _rewardAd.interactable = false;
        }

        private void OnOpenCallback()
        {
            _webFocus.MuteAudio(true);
            _webFocus.PauseGame(true);
        }

        private void OnCloseCallback()
        {
            _webFocus.MuteAudio(false);
            _webFocus.PauseGame(false);
        }

        private void OnRewardedCallback() =>
            _playerMoney.AddCrystal(10, _crystals);

        private string GetLocalizedDescription(CardData cardData)
        {
            if (Application.systemLanguage == SystemLanguage.English)
                return cardData.EnDescription;
            else
                return cardData.RuDescription;
        }

        private string GetLocalizedName(CardData cardData)
        {
            if (Application.systemLanguage == SystemLanguage.English)
                return cardData.EnName;
            else
                return cardData.RuName;
        }

        public void Save(PlayerProgress progress)
        {
            progress.OpenPlayers = _openPlayers.ToList();
        }

        public void Load(PlayerProgress progress)
        {
            _progress = progress;

            if (progress.WorldData.IsNewGame == false)
                _openPlayers = progress.OpenPlayers.ToList();
        }
    }
}