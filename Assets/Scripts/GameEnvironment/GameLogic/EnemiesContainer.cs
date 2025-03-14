using GameEnvironment.GameLogic.CardFolder;
using UnityEngine;

namespace GameEnvironment.GameLogic
{
    [CreateAssetMenu(fileName = "StageEnemies", menuName = "StageEnemies")]
    public class EnemiesContainer : ScriptableObject
    {
        [SerializeField] private Enemy _enemy;
        [SerializeField] private Enemy[] _prefabs;
        [SerializeField] private EnemyStageID _enemyStageID;

        public EnemyStageID EnemyStageID => _enemyStageID;

        public Enemy GetRandomPrefab()
        {
            int randomPrefab = Random.Range(0, _prefabs.Length);
            _enemy = _prefabs[randomPrefab];
            return _enemy;
        }
    }
}