using Data;
using GameEnvironment.GameLogic;
using GameEnvironment.GameLogic.CardFolder;
using GameEnvironment.LevelRoutMap;
using GameEnvironment.UI;
using Infrastructure.Factory;
using Infrastructure.GameManegment;
using Infrastructure.Services;
using UnityEngine;

namespace Infrastructure.States
{
    class LevelState : IPayloadedState1<string, CardData>, IState
    {
        private const string PlayerSpawner = "PlayerSpawner";
        private const string RoutMap = "RoutMap";

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
            //GameObject routMap = GameObject.FindWithTag(RoutMap);
            GameObject routMapObj = _gameFactory.CreateRoutMap();
            GameObject battleHud = _gameFactory.CreateBattleHud();
            BattleHud hud = battleHud.GetComponent<BattleHud>();
            RoutMap routMap = routMapObj.GetComponent<RoutMap>();
            GameObject playerSpawner = hud.PlayerSpawnPoint.gameObject;
            GameObject currentPlayer = _gameFactory.CreatePlayer(_cardData, playerSpawner);
            BattleSystem battleSystem = hud.GetComponent<BattleSystem>();
            DeckCreator deckCreator = hud.GetComponent<DeckCreator>();
            DragController dragController = hud.GetComponent<DragController>();
            Player player = currentPlayer.GetComponent<Player>();
            routMap.Construct(player, hud);
            hud.Construct(player);
            battleSystem.Construct(routMap);
            deckCreator.Construct(player);
            player.Construct(dragController);
            dragController.Construct(player);

            foreach (var skill in player.PlayerSkills) 
                skill.Construct(hud, deckCreator);
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
