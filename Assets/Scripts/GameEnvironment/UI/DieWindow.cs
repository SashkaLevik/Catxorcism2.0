using GameEnvironment.GameLogic.CardFolder;
using GameEnvironment.Units;
using Infrastructure.Services;
using Infrastructure.States;
using UnityEngine;
using UnityEngine.UI;

namespace GameEnvironment.UI
{
    public class DieWindow : MonoBehaviour
    {
        private const string MenuScene = "MenuScene";

        [SerializeField] private Sprite _knight;
        [SerializeField] private Sprite _barbarian;
        [SerializeField] private Sprite _mage;
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

            if (_player.Type == PlayerType.Knight)
                GetComponent<Image>().sprite = _knight;
            else if (_player.Type == PlayerType.Barbarian)
                GetComponent<Image>().sprite = _barbarian;
            else if (_player.Type == PlayerType.Mage)
                GetComponent<Image>().sprite = _mage;
        }
    }
}