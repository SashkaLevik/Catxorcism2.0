using Assets.Scripts.Data;
using Assets.Scripts.Infrastructure.GameManegment;
using Assets.Scripts.Infrastructure.Services;

namespace Assets.Scripts.States
{
    public class ProgressState : IState
    {
        private const string MenuScene = "MenuScene";
        private const string Intro = "Intro";

        private readonly GameStateMachine _gameStateMachine;
        private readonly IPersistentProgressService _progressService;
        private readonly ISaveLoadService _saveLoadService;
        private readonly SceneLoader _sceneLoader;        

        public ProgressState(GameStateMachine gameStateMachine, SceneLoader sceneLoader, IPersistentProgressService progressService, ISaveLoadService saveLoadService)
        {
            _gameStateMachine = gameStateMachine;
            _sceneLoader = sceneLoader;
            _progressService = progressService;
            _saveLoadService = saveLoadService;
        }

        public void Enter()
        {
            LoadProgressOrInitNew();
            _gameStateMachine.Enter<MenuState, string>(MenuScene);
            //CrazySDK.Init(OnInitialize);
        }        

        private void OnInitialize()=>
            _gameStateMachine.Enter<MenuState, string>(MenuScene);

        public void Exit() { }

        private void LoadProgressOrInitNew()
        {
            _progressService.Progress =
                _saveLoadService.LoadProgress()
                ?? NewProgress();
        }

        private PlayerProgress NewProgress()
        {
            var progress = new PlayerProgress(initialLevel: MenuScene);

            return progress;
        }
    }
}
