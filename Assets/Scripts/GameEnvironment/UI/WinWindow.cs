using Assets.Scripts.GameEnvironment.GameLogic;
using Assets.Scripts.GameEnvironment.Units;
using Assets.Scripts.Infrastructure.Services;
using Assets.Scripts.States;
using CrazyGames;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.GameEnvironment.UI
{
    public class WinWindow : MonoBehaviour
    {
        private const string MenuScene = "MenuScene";

        [SerializeField] private PlayerMoney _playerMoney;
        [SerializeField] private BattleHud _battleHud;
        [SerializeField] private DeckSpawner _deckSpawner;
        [SerializeField] private TMP_Text _winGameMesage;
        [SerializeField] private Button _toMenu;
        [SerializeField] private Button _nextArea;
        [SerializeField] private GameObject _window;

        private int _maxWave = 2;
        private int _crystalsPrize = 10;
        private int _coinsPrize = 20;
        private IGameStateMachine _stateMachine;

        private void Awake()=>
            _stateMachine = AllServices.Container.Single<IGameStateMachine>();

        private void OnEnable()
        {
            _toMenu.onClick.AddListener(ShowAdd);
            _nextArea.onClick.AddListener(SetNewWave);
            _deckSpawner.BossDied += OpenWinWindow;
        }
       
        private void OnDestroy()
        {
            _toMenu.onClick.RemoveListener(ShowAdd);
            _nextArea.onClick.AddListener(SetNewWave);
            _deckSpawner.BossDied -= OpenWinWindow;
        }

        private void OpenWinWindow(int waveNumber)
        {
            _window.SetActive(true);
            _playerMoney.AddCrystal(_crystalsPrize, _battleHud.Crystals);
            _playerMoney.AddCoin(_coinsPrize, _battleHud.Coins);
            _playerMoney.SaveMoney();

            if (_maxWave == waveNumber)
            {
                _nextArea.gameObject.SetActive(false);
                _winGameMesage.gameObject.SetActive(true);
            }
        }

        private void SetNewWave()
        {
            _window.SetActive(false);                
        }

        private void ShowAdd() =>
            CrazySDK.Ad.RequestAd(CrazyAdType.Midgame, null, OnAdError, OnAdFinished);

        private void OnAdFinished() =>
            _stateMachine.Enter<MenuState, string>(MenuScene);

        private void OnAdError(SdkError obj) =>
            OnAdFinished();
    }
}