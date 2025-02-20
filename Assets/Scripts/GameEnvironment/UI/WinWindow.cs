using GameEnvironment.GameLogic;
using GameEnvironment.GameLogic.CardFolder;
using GameEnvironment.Units;
using Infrastructure.Services;
using Infrastructure.States;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GameEnvironment.UI
{
    public class WinWindow : MonoBehaviour
    {
        private const string MenuScene = "MenuScene";

        [SerializeField] private Sprite _knight;
        [SerializeField] private Sprite _barbarian;
        [SerializeField] private Sprite _mage;
        [SerializeField] private PlayerMoney _playerMoney;
        [SerializeField] private BattleHud _battleHud;
        [SerializeField] private TMP_Text _winGameMesage;
        [SerializeField] private Button _toMenu;
        [SerializeField] private Button _nextArea;
        [SerializeField] private GameObject _window;

        private int _maxWave = 2;
        private IGameStateMachine _stateMachine;
        private Player _player;

        private void Awake()=>
            _stateMachine = AllServices.Container.Single<IGameStateMachine>();

        private void Start() =>
            _player = _battleHud.Player;

        private void OnEnable()
        {
            _toMenu.onClick.AddListener(ReturnToMenu);
            _nextArea.onClick.AddListener(SetNewWave);
        }
       
        private void OnDestroy()
        {
            _toMenu.onClick.RemoveListener(ReturnToMenu);
            _nextArea.onClick.AddListener(SetNewWave);
        }

        private void OpenWinWindow(int waveNumber)
        {
            if (_player == null) return;

            _window.SetActive(true);
            //_battleHud.ResetCooldown();

            if (_player.Type == PlayerType.Knight)
                _window.GetComponent<Image>().sprite = _knight;
            else if (_player.Type == PlayerType.Barbarian)
                _window.GetComponent<Image>().sprite = _barbarian;
            else if (_player.Type == PlayerType.Mage)
                _window.GetComponent<Image>().sprite = _mage;
            
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

        private void ReturnToMenu() =>
            _stateMachine.Enter<MenuState, string>(MenuScene);

        //private void ShowAdd() =>
        //    CrazySDK.Ad.RequestAd(CrazyAdType.Midgame, null, OnAdError, OnAdFinished);

        //private void OnAdFinished() =>
        //    _stateMachine.Enter<MenuState, string>(MenuScene);

        //private void OnAdError(SdkError obj) =>
        //    OnAdFinished();
    }
}