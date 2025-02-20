using System;
using GameEnvironment.UI;
using UnityEngine;
using UnityEngine.UI;

namespace GameEnvironment.GameLogic
{
    public class BattleSystem : MonoBehaviour
    {
        [SerializeField] private DeckCreator _deckCreator;
        [SerializeField] private BattleHud _battleHud;
        [SerializeField] private Button _startBattle;
        [SerializeField] private Button _endTurn;

        private void Start()
        {
            _startBattle.onClick.AddListener(OnBattleStart);
        }

        private void OnBattleStart()
        {
            _deckCreator.DrawNextHand();
        }
    }
}