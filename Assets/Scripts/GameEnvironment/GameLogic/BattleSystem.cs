using System.Collections;
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
        private float _animationDelay = 0.2f;
        private RoutMap _routMap;
        private Enemy _currentEnemy;
        
        private void Start()
        {
            _startBattle.onClick.AddListener(OnBattleStart);
            _endTurn.onClick.AddListener(EnemyTurn);
            _routMap.StageButtonPressed += EnterStage;
        }

        public void Construct(RoutMap routMap)
        {
            _routMap = routMap;
        }

        private void PlayerTurn()
        {
            _deckCreator.DrawHand();
        }
        
        private void EnemyTurn()
        {
            StartCoroutine(OnEnemyTurn());
        }

        private IEnumerator OnEnemyTurn()
        {
            foreach (var card in _deckCreator.HandCards) 
                card.Disactivate();

            foreach (var guard in _deckCreator.FieldGuards) 
                guard.EffectsReceiver.ApplyReceivedEffect();

            yield return new WaitForSeconds(_animationDelay);

            foreach (var enemyGuard in _enemySpawner.SpawnedGuards)
            {
                enemyGuard.UsePreparedSkill();
                yield return new WaitForSeconds(_animationDelay);
                enemyGuard.EffectsReceiver.ApplyReceivedEffect();
                yield return new WaitForSeconds(_animationDelay);
            }

            foreach (var enemyGuard in _enemySpawner.SpawnedGuards)
            {
                enemyGuard.PrepareSkill();
                yield return new WaitForSeconds(_animationDelay);
            }
            
            PlayerTurn();
            _endTurn.interactable = true;
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
            _startBattle.interactable = false;
            _endTurn.interactable = true;
        }
    }
}