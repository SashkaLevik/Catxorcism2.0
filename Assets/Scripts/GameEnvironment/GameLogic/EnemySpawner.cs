using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GameEnvironment.GameLogic.CardFolder;
using GameEnvironment.GameLogic.RowFolder;
using GameEnvironment.UI;
using GameEnvironment.Units;
using UnityEngine;

namespace GameEnvironment.GameLogic
{
    public class EnemySpawner : MonoBehaviour
    {
        private const string MarketEnemies = "Data/UnitData/EnemiesContainer/MarketEnemies";

        [SerializeField] private DragController _dragController;
        [SerializeField] private DeckCreator _deckCreator;
        [SerializeField] private BattleHud _battleHud;
        [SerializeField] private Row _enemyFrontRow;
        [SerializeField] private Row _enemyBackRow;
        
        private float _moveSpeed = 30f;
        private EnemyStageID _currentStage;
        private Player _player;
        private Enemy _spawnedEnemy;
        private Enemy _randomEnemy;
        private EnemyGuard _currentGuard;
        private RectTransform _guardSlot;
        private List<EnemiesContainer> _enemies;
        private List<EnemyGuard> _enemyGuards;
        private List<EnemyGuard> _spawnedGuards = new List<EnemyGuard>();

        public Enemy SpawnedEnemy => _spawnedEnemy;

        public List<EnemyGuard> SpawnedGuards => _spawnedGuards;

        private void Awake()
        {
            _enemies = Resources.LoadAll<EnemiesContainer>(MarketEnemies).ToList();
        }

        private void OnEnable()
        {
            _dragController.GuardPlaced += OnPlayerGuardPlaced;
        }

        private void OnPlayerGuardPlaced(Guard arg0)
        {
            RefreshEnemies();
        }

        public void RefreshEnemies() => 
            StartCoroutine(GetNewEnemy());

        private IEnumerator GetNewEnemy()
        {
            yield return new WaitForSeconds(0.4f);
            
            foreach (var guard in _spawnedGuards) 
                guard.TryGetEnemy(_battleHud);
        }

        public void SpawnEnemy(int stageNumber)
        {
            _currentStage = (EnemyStageID) stageNumber;
            StartCoroutine(CreateEnemy(_currentStage, _battleHud.EnemySpawnPoint));
        }

        private IEnumerator CreateEnemy(EnemyStageID enemyType, RectTransform at)
        {
            _randomEnemy = GetRandomEnemy<Enemy>(enemyType);
            _spawnedEnemy = Instantiate(_randomEnemy, at.transform);
            _spawnedEnemy.InitBattle(_battleHud, _battleHud.Player);
            _enemyGuards = _spawnedEnemy.Guards.ToList();
            yield return new WaitForSeconds(0.2f);
            yield return StartCoroutine(SpawnGuards(_enemyFrontRow));
            yield return StartCoroutine(SpawnGuards(_enemyBackRow));
        }

        private IEnumerator SpawnGuards(Row row)
        {
            foreach (var guard in _enemyGuards)
            {
                if (row.CheckRowMatch(guard) && row.GetFreeSlot())
                {
                    RowCardSlot cardSlot = row.GetFreeSlot();
                    cardSlot.Occupy();
                    _currentGuard = Instantiate(guard, _battleHud.EnemySpawnPoint);
                    _currentGuard.Health.Died += OnGuardDie;
                    _currentGuard.SetRow(_battleHud, row, cardSlot);
                    _currentGuard.InitPlayer(_battleHud.Player);
                    _currentGuard.TryGetEnemy(_battleHud);
                    _spawnedGuards.Add(_currentGuard);
                    yield return new WaitForSeconds(0.1f);
                    StartCoroutine(Move(_currentGuard, cardSlot.SlotPosition));
                    yield return new WaitForSeconds(0.2f);
                }
            }
        }

        private void OnGuardDie(Unit unit)
        {
            unit.CardSlot.Clear();
            unit.Health.Died -= OnGuardDie;
            _deckCreator.RefreshEnemies();
        }
        
        private T GetRandomEnemy<T>(EnemyStageID type) where T : Enemy
        {
            return (T)_enemies.Where(e => e.EnemyStageID == type).OrderBy(o => Random.value).First().GetRandomPrefab();
        }   
        
        private IEnumerator Move(Card card, RectTransform newPos)
        {            
            while (card.transform.position != newPos.transform.position)
            {
                card.transform.position = Vector3.MoveTowards(card.transform.position, newPos.transform.position,
                    _moveSpeed * Time.deltaTime);
                yield return null;
            }
            card.transform.SetParent(newPos);
        }
    }
}