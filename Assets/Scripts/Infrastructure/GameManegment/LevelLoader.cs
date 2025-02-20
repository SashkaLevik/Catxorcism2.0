using Data;
using GameEnvironment.UI;
using Infrastructure.Services;
using Infrastructure.States;
using UnityEngine;

namespace Infrastructure.GameManegment
{
    public class LevelLoader : MonoBehaviour, ISaveProgress
    {
        [SerializeField] private PlayersRoom _playersRoom;
        [SerializeField] private Map _map;
        
        private string _level;

        private IGameStateMachine _stateMachine;
        private ISaveLoadService _saveLoadService;

        private void Awake()
        {
            _stateMachine = AllServices.Container.Single<IGameStateMachine>();
            _saveLoadService = AllServices.Container.Single<ISaveLoadService>();
        }

        private void OnEnable()
        {
            _map.LevelLoaded += OnLevelLoad;
        }

        private void OnLevelLoad(string level)
        {
            _level = level;
            //_saveLoadService.SaveProgress();
            _stateMachine.Enter<LevelState, string>(level, _playersRoom.PlayerData);
        }

        public void Load(PlayerProgress progress)
        {
        }

        public void Save(PlayerProgress progress)
        {
            progress.WorldData.Level = _level;
        }
    }
}