using Assets.Scripts.GameEnvironment.Units;
using Assets.Scripts.Infrastructure.GameManegment;
using Assets.Scripts.Infrastructure.Services;
using Assets.Scripts.States;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.GameEnvironment.UI
{
    public class DieWindow : MonoBehaviour
    {
        private const string MenuScene = "MenuScene";

        [SerializeField] private Sprite _knight;
        [SerializeField] private Sprite _barbarian;
        [SerializeField] private Sprite _mage;
        [SerializeField] private BattleHud _battleHud;
        [SerializeField] private Button _toMenu;
        [SerializeField] private WebFocus _webFocus;

        private Player _player;
        private IGameStateMachine _stateMachine;

        private void Awake()
        {
            _stateMachine = AllServices.Container.Single<IGameStateMachine>();
        }

        private void Start()
        {
            _player = _battleHud.Player;

            if (_player.Type == PlayerType.Knight)
                GetComponent<Image>().sprite = _knight;
            else if (_player.Type == PlayerType.Barbarian)
                GetComponent<Image>().sprite = _barbarian;
            else if (_player.Type == PlayerType.Mage)
                GetComponent<Image>().sprite = _mage;
        }

        private void OnEnable() =>
            _toMenu.onClick.AddListener(ShowAdd);

        private void OnDestroy() =>
            _toMenu.onClick.RemoveListener(ShowAdd);

        private void ShowAdd()
        {
            Agava.YandexGames.InterstitialAd.Show(ShowAd, CloseAd);
        }

        private void ShowAd()
        {
            _webFocus.MuteAudio(true);
            _webFocus.PauseGame(true);
        }

        private void CloseAd(bool value)
        {
            _webFocus.MuteAudio(false);
            _webFocus.PauseGame(false);
            _stateMachine.Enter<MenuState, string>(MenuScene);
        }
    }
}