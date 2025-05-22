using System.Collections.Generic;
using Data;
using GameEnvironment.GameLogic;
using GameEnvironment.GameLogic.CardFolder;
using GameEnvironment.GameLogic.DiceFolder;
using GameEnvironment.GameLogic.RowFolder;
using GameEnvironment.Units;
using Infrastructure.Services;
using UnityEngine;
using UnityEngine.UI;

namespace GameEnvironment.UI
{
    public class BattleHud : MonoBehaviour, ISaveProgress
    {
        [SerializeField] private DragController _dragController;
        [SerializeField] private Dice _dicePrefab;
        [SerializeField] private GameObject _settings;
        [SerializeField] private Canvas _canvas;
        [SerializeField] private DieWindow _dieWindow;
        [SerializeField] private Image[] _fullImages;
        [SerializeField] private Image[] _emptyImages;
        [SerializeField] private RectTransform _playerSpawnPoint;
        [SerializeField] private RectTransform _enemySpawnPoint;
        [SerializeField] private Row _playerFrontRow;
        [SerializeField] private Row _playerBackRow;
        [SerializeField] private Row _enemyFrontRow;
        [SerializeField] private Row _enemyBackRow;
        [SerializeField] private MiddleRow _middleRow;
        [SerializeField] private List<Button> _diceButtons;
        [SerializeField] private AudioSource _battleMusic;

        private Vector3 _playerFrontDicePos = new Vector3(-1.9f, 4f, -10.6f);
        private Vector3 _playerBackDicePos = new Vector3(-4.4f, 4f, -10.6f);
        private Vector3 _enemyFrontDicePos = new Vector3(2f, 4f, -10.6f);
        private Vector3 _enemyBackDicePos = new Vector3(4.4f, 4f, -10.6f);
        public List<Dice> _dices = new List<Dice>();

        private Player _player;
        private PlayerProgress _progress;

        public Player Player => _player;

        public GameObject Settings => _settings;

        public Dice PlayerFrontDice { get; private set; }
        public Dice PlayerBackDice { get; private set; }
        public Dice EnemyFrontDice { get; private set; }
        public Dice EnemyBackDice { get; private set; }
        
        public RectTransform PlayerSpawnPoint => _playerSpawnPoint;

        public RectTransform EnemySpawnPoint => _enemySpawnPoint;

        public Row PlayerFrontRow => _playerFrontRow;

        public Row PlayerBackRow => _playerBackRow;

        public Row EnemyFrontRow => _enemyFrontRow;

        public Row EnemyBackRow => _enemyBackRow;

        public MiddleRow MiddleRow => _middleRow;

        private void Awake() =>
            _canvas.worldCamera = Camera.main;        

        private void Start()
        {
            _battleMusic.Play();
            CreateDices();
            _player.LeadershipChanged += UpdateLeadership;
            UpdateLeadership(_player.Leadership);

            foreach (var button in _diceButtons) 
                button.onClick.AddListener(DeactivateDices);
        }

        private void OnDestroy()
        {
            _player.LeadershipChanged -= UpdateLeadership;
            
            foreach (var button in _diceButtons) 
                button.onClick.RemoveListener(DeactivateDices);
        }

        public void Construct(Player player)
        {
            _player = player;
            _player.GetComponent<Health>().Died += OnPlayerDie;
        }

        public void ActivateDices()
        {
            foreach (var button in _diceButtons) 
                button.interactable = true;
        }

        public void DeactivateDices()
        {
            foreach (var button in _diceButtons) 
                button.interactable = false;
        }
        
        private void UpdateLeadership(int value)
        {
            for (int i = 0; i < _emptyImages.Length; i++)
                _emptyImages[i].gameObject.SetActive(false);

            for (int i = 0; i < _fullImages.Length; i++)
                _fullImages[i].gameObject.SetActive(false);

            for (int i = 0; i < value; i++)
                _fullImages[i].gameObject.SetActive(true);
        }
        
        public void RollDices()
        {
            PlayerFrontDice.Roll();
            PlayerBackDice.Roll();
            EnemyFrontDice.Roll();
            EnemyBackDice.Roll();
        }
        
        private void CreateDices()
        {
            PlayerFrontDice = Instantiate(_dicePrefab, _playerFrontDicePos, Quaternion.identity);
            PlayerBackDice = Instantiate(_dicePrefab, _playerBackDicePos, Quaternion.identity);
            EnemyFrontDice = Instantiate(_dicePrefab, _enemyFrontDicePos, Quaternion.identity);
            EnemyBackDice = Instantiate(_dicePrefab, _enemyBackDicePos, Quaternion.identity);
            _playerFrontRow.InitDice(PlayerFrontDice);
            _playerBackRow.InitDice(PlayerBackDice);
            _enemyFrontRow.InitDice(EnemyFrontDice);
            _enemyBackRow.InitDice(EnemyBackDice);
            _dices.Add(PlayerFrontDice);
            _dices.Add(PlayerBackDice);
            _dices.Add(EnemyFrontDice);
            _dices.Add(EnemyBackDice);
        }

        private void OnPlayerDie(Unit unit)
        {
            _dieWindow.gameObject.SetActive(true);
            _player.GetComponent<Health>().Died -= OnPlayerDie;
        }

        public void Load(PlayerProgress progress)
        {
        }

        public void Save(PlayerProgress progress)
        {
        }
    }
}