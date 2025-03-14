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

        [SerializeField] private BattleHud _battleHud;
        [SerializeField] private Row _enemyFrontRow;
        [SerializeField] private Row _enemyBackRow;
        
        private float _moveSpeed = 30f;
        private int _leadership;
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

        private void Awake()
        {
            _enemies = Resources.LoadAll<EnemiesContainer>(MarketEnemies).ToList();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                StartCoroutine(SpawnGuards());

            }
        }

        public void SpawnEnemy(int stageNumber)
        {
            _currentStage = (EnemyStageID) stageNumber;
            StartCoroutine(CreateEnemy(_currentStage, _battleHud.EnemySpawnPoint));
        }

        private IEnumerator CreateEnemy(EnemyStageID enemyType, RectTransform at)
        {
            Debug.Log("EnemySpawned");
            _randomEnemy = GetRandomEnemy<Enemy>(enemyType);
            _spawnedEnemy = Instantiate(_randomEnemy, at.transform);
            _spawnedEnemy.InitBattle(_battleHud, _battleHud.Player);
            _enemyGuards = _spawnedEnemy.Guards.ToList();
            yield return new WaitForSeconds(1.3f);
            _leadership = _spawnedEnemy.Leadership;
            Debug.Log("SpawnerGetLeadership");
            StartCoroutine(SpawnGuards());
        }

        private IEnumerator SpawnGuards()
        {
            int guardsToSpawn = _leadership;
            
            for (int i = 0; i < guardsToSpawn; i++)
            {
                if (_enemyGuards.Count == 0)
                    break;
                
                _currentGuard = Instantiate(_enemyGuards[0], _battleHud.EnemySpawnPoint);
                _currentGuard.GetComponent<Health>().Died += OnGuardDie;
                _spawnedGuards.Add(_currentGuard);
                _enemyGuards.Remove(_enemyGuards[0]);
                yield return new WaitForSeconds(0.2f);

                if (_enemyFrontRow.CheckRowMatch(_currentGuard))
                {
                    if (_enemyFrontRow.GetFreeSlot(i) != null)
                    {
                        _guardSlot = _enemyFrontRow.GetFreeSlot(i);
                        _currentGuard.InitRow(_enemyFrontRow, i);
                        StartCoroutine(Move(_currentGuard, _guardSlot));
                    }
                }
                else if (_enemyBackRow.CheckRowMatch(_currentGuard))
                {
                    if (_enemyBackRow.GuardSlots[i].GetComponentInChildren<EnemyGuard>() == null)
                    {
                        _guardSlot = _enemyBackRow.GuardSlots[i].GetComponent<RectTransform>();
                        _currentGuard.InitRow(_enemyBackRow, i);
                        StartCoroutine(Move(_currentGuard, _guardSlot));
                    }
                }

                _leadership--;
                yield return new WaitForSeconds(0.2f);
            }
        }

        private void OnGuardDie()
        {
            _leadership++;
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