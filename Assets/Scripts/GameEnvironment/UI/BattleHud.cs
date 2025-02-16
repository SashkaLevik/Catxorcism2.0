using Data;
using GameEnvironment.GameLogic.CardFolder;
using GameEnvironment.GameLogic.DiceFolder;
using GameEnvironment.Units;
using Infrastructure.Services;
using UnityEngine;
using UnityEngine.UI;

namespace GameEnvironment.UI
{
    public class BattleHud : MonoBehaviour, ISaveProgress
    {
        [SerializeField] private Dice _dicePrefab;
        [SerializeField] private GameObject _settings;
        [SerializeField] private AudioSource _battleMusic;
        [SerializeField] private Canvas _canvas;
        [SerializeField] private DieWindow _dieWindow;
        [SerializeField] private Image[] _fullImages;
        [SerializeField] private Image[] _emptyImages;

        
        private Vector3 _playerFrontDicePos = new Vector3(-1.9f, 4f, -10.6f);
        private Vector3 _playerBackDicePos = new Vector3(-4.4f, 4f, -10.6f);
        private Vector3 _enemyFrontDicePos = new Vector3(2f, 4f, -10.6f);
        private Vector3 _enemyBackDicePos = new Vector3(4.4f, 4f, -10.6f);

        private Dice _playerFrontDice;
        private Dice _playerBackDice;
        private Dice _enemyFrontDice;
        private Dice _enemyBackDice;
        private int _actionPoints = 2;
        private int _usePerTurn;        
        private Player _player;

        public Player Player => _player;

        public GameObject Settings => _settings;

        public Dice PlayerFrontDice => _playerFrontDice;

        public Dice PlayerBackDice => _playerBackDice;
        
        private void Awake() =>
            _canvas.worldCamera = Camera.main;        

        private void Start()
        {
            _usePerTurn = _actionPoints;
            UpdateCooldown();
            _battleMusic.Play();            
            CreateDice();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                _playerFrontDice.Roll();
            }
        }

        public void RollDices()
        {
            _playerFrontDice.Roll();
            _playerBackDice.Roll();
            _enemyFrontDice.Roll();
            _enemyBackDice.Roll();
        }
        
        private void CreateDice()
        {
            _playerFrontDice = Instantiate(_dicePrefab, _playerFrontDicePos, Quaternion.identity);
            _playerBackDice = Instantiate(_dicePrefab, _playerBackDicePos, Quaternion.identity);
            _enemyFrontDice = Instantiate(_dicePrefab, _enemyFrontDicePos, Quaternion.identity);
            _enemyBackDice = Instantiate(_dicePrefab, _enemyBackDicePos, Quaternion.identity);
        }
        public void Construct(Player player)
        {
            _player = player;
            _player.GetComponent<Health>().Died += OnPlayerDie;
        }

        public void ResetCooldown()
        {
            _usePerTurn = _actionPoints;
            UpdateCooldown();
        }             

        public void UpdateCooldown()
        {
            for (int i = 0; i < _emptyImages.Length; i++)
                _emptyImages[i].gameObject.SetActive(false);

            for (int i = 0; i < _fullImages.Length; i++)
                _fullImages[i].gameObject.SetActive(false);

            for (int i = 0; i < _actionPoints; i++)
                _fullImages[i].gameObject.SetActive(true);
        }

        public void DecreaseAP()
        {
            for (int i = 0; i < _usePerTurn; i++)
            {
                _fullImages[_usePerTurn - 1].gameObject.SetActive(false);
                _emptyImages[_usePerTurn - 1].gameObject.SetActive(true);
            }

            _usePerTurn--;

        }

        private void OnPlayerDie()
        {
            _dieWindow.gameObject.SetActive(true);
            _player.GetComponent<Health>().Died -= OnPlayerDie;
        }

        private void GetEnemies()
        {
            /*for (int i = 0; i < _summonGuardButtons.Count; i++)
            {
                var guard = _summonGuardButtons[i].GetComponentInChildren<Guard>();
                var enemy = _deck.FirstRaw[i].GetComponentInChildren<Enemy>();

                if (guard != null && enemy != null)
                {
                    guard.InitEnemy(enemy);
                    enemy.InitGuard(guard);
                }
            }*/
        }

        public void Load(PlayerProgress progress)
        {
        }

        public void Save(PlayerProgress progress)
        {
        }
    }
}