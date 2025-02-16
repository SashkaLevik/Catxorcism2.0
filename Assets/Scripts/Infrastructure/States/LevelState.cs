using Data;
using GameEnvironment.GameLogic;
using GameEnvironment.GameLogic.CardFolder;
using GameEnvironment.UI;
using GameEnvironment.Units;
using Infrastructure.Factory;
using Infrastructure.GameManegment;
using Infrastructure.Services;
using UnityEngine;

namespace Infrastructure.States
{
    class LevelState : IPayloadedState1<string, CardData>, IState
    {
        private const string PlayerSpawner = "PlayerSpawner";

        private readonly GameStateMachine _stateMachine;
        private readonly SceneLoader _sceneLoader;
        private readonly LoadingCurtain _loadingCurtain;
        private readonly IGameFactory _gameFactory;
        private readonly IPersistentProgressService _progressService;
        private CardData _cardData;

        public LevelState(GameStateMachine stateMachine, SceneLoader sceneLoader, LoadingCurtain loadingCurtain,
            IGameFactory gameFactory, IPersistentProgressService progressService)
        {
            _stateMachine = stateMachine;
            _sceneLoader = sceneLoader;
            _loadingCurtain = loadingCurtain;
            _gameFactory = gameFactory;
            _progressService = progressService;
        }

        public void Enter(string sceneName)
        {
            _loadingCurtain.Show();
            _sceneLoader.Load(sceneName);
        }

        public void Enter(string sceneName, CardData cardData)
        {
            _loadingCurtain.Show();
            _cardData = cardData;
            _sceneLoader.Load(sceneName, onLoaded: OnSceneLoad);
        }

        public void Exit()
            => _loadingCurtain.Hide();

        private void OnSceneLoad()
        {
            InitGameWorld();
            InformProgressReaders();
            _stateMachine.Enter<LoopState>();
        }

        private void InitGameWorld()
        {
            GameObject battleHud = _gameFactory.CreateBattleHud();
            GameObject spawnPoint = GameObject.FindWithTag(PlayerSpawner);
            var playerSpawner = spawnPoint.GetComponent<PlayerSpawnPoint>();
            GameObject player = _gameFactory.CreatePlayer(_cardData, spawnPoint);
            battleHud.GetComponent<BattleHud>().Construct(player.GetComponent<Player>());                       
        }      

        private void InformProgressReaders()
        {
            foreach (ILoadProgress progressReader in _gameFactory.ProgressReaders)
                progressReader.Load(_progressService.Progress);
        }

        public void Enter()
        {

        }
    }
}
