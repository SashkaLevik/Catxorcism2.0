using Assets.Scripts.GameEnvironment.Units;
using Assets.Scripts.Infrastructure.Services;
using Assets.Scripts.States;
using CrazyGames;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Assets.Scripts.GameEnvironment.UI
{
    public class DieWindow : MonoBehaviour
    {
        private const string MenuScene = "MenuScene";

        [SerializeField] private BattleHud _battleHud;
        [SerializeField] private Button _toMenu;

        private Player _player;
        private IGameStateMachine _stateMachine;

        private void Awake()
        {
            _stateMachine = AllServices.Container.Single<IGameStateMachine>();
        }

        private void Start()
        {
            _player = _battleHud.Player;
        }

        private void OnEnable() =>
            _toMenu.onClick.AddListener(ShowAdd);

        private void OnDestroy()=>
            _toMenu.onClick.RemoveListener(ShowAdd);

        private void ShowAdd()=>
            CrazySDK.Ad.RequestAd(CrazyAdType.Midgame, null, OnAdError, OnAdFinished);

        private void OnAdFinished()=>
            _stateMachine.Enter<MenuState, string>(MenuScene);

        private void OnAdError(SdkError obj)=>
            OnAdFinished();             
    }
}