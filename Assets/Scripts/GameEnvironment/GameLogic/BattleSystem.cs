using GameEnvironment.GameLogic.CardFolder;
using GameEnvironment.GameLogic.DiceFolder;
using GameEnvironment.LevelRoutMap;
using GameEnvironment.UI;
using UnityEngine;
using UnityEngine.UI;

namespace GameEnvironment.GameLogic
{
    public class BattleSystem : MonoBehaviour
    {
        [SerializeField] private DeckCreator _deckCreator;
        [SerializeField] private BattleHud _battleHud;
        [SerializeField] private EnemySpawner _enemySpawner;
        [SerializeField] private Button _startBattle;
        [SerializeField] private Button _endTurn;

        private int _stageNumber;
        private RoutMap _routMap;
        private Enemy _currentEnemy;
        
        private void Start()
        {
            _startBattle.onClick.AddListener(OnBattleStart);
            _routMap.StageButtonPressed += EnterStage;
        }

        public void Construct(RoutMap routMap)
        {
            _routMap = routMap;
        }
        
        private void EnterStage()
        {
            _battleHud.RollDices();
            _enemySpawner.SpawnEnemy(_stageNumber);
            _battleHud.PlayerFrontDice.OnDiceResult += PrepareForBattle;
        }

        private void PrepareForBattle(DiceFace arg0)
        {
            _deckCreator.DrawHand();
        }

        private void CompleteStage()
        {
            //_routMap.OpenNextEvents(_stageNumber);            
            _stageNumber++;
        }
        
        private void OnBattleStart()
        {
            _deckCreator.DrawHand();
        }
    }
}