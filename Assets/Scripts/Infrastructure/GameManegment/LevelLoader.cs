using Data;
using Infrastructure.Services;
using Infrastructure.States;
using UnityEngine;
using UnityEngine.UI;

namespace Infrastructure.GameManegment
{
    public class LevelLoader : MonoBehaviour, ISaveProgress
    {
        [SerializeField] private Button _play;
        //[SerializeField] private Map _map;

        //private string _level;

        private IGameStateMachine _stateMachine;
        private ISaveLoadService _saveLoadService;

        private void Awake()
        {
            _stateMachine = AllServices.Container.Single<IGameStateMachine>();
            _saveLoadService = AllServices.Container.Single<ISaveLoadService>();
        }

        private void OnEnable()
        {
            _play.onClick.AddListener(EnterGame);
        }

        private void EnterGame()
        {
        }

        private void OnLevelLoad(string level)
        {
            //_level = level;
            _saveLoadService.SaveProgress();
            //_stateMachine.Enter<LevelState, string>(level, _treeHouse.ToyData);
        }

        public void Save(PlayerProgress progress)
        {
        }

        public void Load(PlayerProgress progress)
        {
        }
    }
}