using GameEnvironment.GameLogic.CardFolder;
using GameEnvironment.UI;
using GameEnvironment.UI.PlayerWallet;
using UnityEngine;

namespace GameEnvironment.LevelRoutMap.RoutEventWindows
{
    public class RoutEvent : BaseWindow
    {
        [SerializeField] protected Warning _warning;
        
        public RoutEventType routEventType;
        protected Player _player;
        protected PlayerMoney _playerMoney;
        protected BattleHud _battleHud;
        
        public void Construct(Player player, BattleHud battleHud)
        {
            _player = player;
            _battleHud = battleHud;
            _playerMoney = _battleHud.PlayerMoney;
        }
    }
}