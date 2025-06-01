using System.Collections;
using System.Linq;
using GameEnvironment.GameLogic.CardFolder;
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
            _startBattle.onClick.AddListener(StartBattle);
            _endTurn.onClick.AddListener(EnemyTurn);
            _routMap.BattleEntered += EnterStage;
        }

        public void Construct(RoutMap routMap)
        {
            _routMap = routMap;
        }

        private void PlayerTurn()
        {
            StartCoroutine(OnPlayerTurn());
        }
        
        private void EnemyTurn()
        {
            StartCoroutine(OnEnemyTurn());
        }

        private IEnumerator OnPlayerTurn()
        {
            foreach (var guard in _deckCreator.FieldGuards.Where(guard => guard != null))
                guard.EffectsReceiver.ApplyReceivedEffect();

            _deckCreator.Player.EffectsReceiver.ApplyReceivedEffect();
            yield return new WaitForSeconds(_animationDelay);
            _deckCreator.DrawHand();
        }

        private IEnumerator OnEnemyTurn()
        {
            foreach (var card in _deckCreator.HandCards) 
                card.Inactivate();

            yield return new WaitForSeconds(_animationDelay);

            foreach (var enemyGuard in _enemySpawner.SpawnedGuards.Where(enemyGuard => enemyGuard != null))
            {
                enemyGuard.EffectsReceiver.ApplyReceivedEffect();
                yield return new WaitForSeconds(_animationDelay);
            }
            
            foreach (var enemyGuard in _enemySpawner.SpawnedGuards.Where(enemyGuard => enemyGuard != null))
            {
                enemyGuard.UsePreparedSkill();
                yield return new WaitForSeconds(_animationDelay);
            }

            foreach (var enemyGuard in _enemySpawner.SpawnedGuards.Where(enemyGuard => enemyGuard != null))
            {
                enemyGuard.PrepareSkill();
                yield return new WaitForSeconds(_animationDelay);
            }

            PlayerTurn();
            _endTurn.interactable = true;
        }

        private void EnterStage()
        {
            _routMap.gameObject.SetActive(false);
            //_battleHud.RollDices();
            //StartCoroutine(OnStageEnter());
            PrepareForBattle();
        }

        private IEnumerator OnBattleStart()
        {
            _startBattle.interactable = false;
            _battleHud.RollDices();
            
            foreach (var dice in _battleHud._dices)
                yield return new WaitWhile(() => dice.IsRolling);

            yield return new WaitForSeconds(0.5f);
            _deckCreator.DrawHand();
            _endTurn.interactable = true;
            //PrepareForBattle();
        }
        
        private void PrepareForBattle()
        {
            _deckCreator.DrawHand();
            _enemySpawner.SpawnEnemy(_stageNumber);
            _startBattle.interactable = true;
        }

        private void CompleteStage()
        {
            //_routMap.OpenNextEvents(_stageNumber);            
            _stageNumber++;
        }
        
        private void StartBattle()
        {
            StartCoroutine(OnBattleStart());
            _startBattle.interactable = false;
            _endTurn.interactable = true;
        }
    }
}