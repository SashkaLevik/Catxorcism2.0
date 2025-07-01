using System.Collections.Generic;
using System.Linq;
using Data;
using GameEnvironment.GameLogic;
using GameEnvironment.GameLogic.CardFolder;
using GameEnvironment.GameLogic.DiceFolder;
using GameEnvironment.GameLogic.RowFolder;
using GameEnvironment.UI.PlayerWallet;
using GameEnvironment.Units;
using Infrastructure.Services;
using TMPro;
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
        [SerializeField] private TMP_Text _coinsAmount;

        private Vector3 _playerFrontDicePos = new Vector3(-1.9f, 4f, -10.6f);
        private Vector3 _playerBackDicePos = new Vector3(-4.4f, 4f, -10.6f);
        private Vector3 _enemyFrontDicePos = new Vector3(2f, 4f, -10.6f);
        private Vector3 _enemyBackDicePos = new Vector3(4.4f, 4f, -10.6f);
        private DiceData _diceData;
        private Player _player;
        private PlayerProgress _progress;
        private ISaveLoadService _saveLoadService;
        public List<Dice> _dices = new List<Dice>();

        public Player Player => _player;

        public GameObject Settings => _settings;

        public TMP_Text CoinsAmount => _coinsAmount;

        public Dice PlayerFrontDice { get; private set; }
        public Dice PlayerBackDice { get; private set; }
        public Dice EnemyFrontDice { get; private set; }
        public Dice EnemyBackDice { get; private set; }
        public PlayerMoney PlayerMoney { get; private set; }
        
        public RectTransform PlayerSpawnPoint => _playerSpawnPoint;

        public RectTransform EnemySpawnPoint => _enemySpawnPoint;

        public Row PlayerFrontRow => _playerFrontRow;

        public Row PlayerBackRow => _playerBackRow;

        public Row EnemyFrontRow => _enemyFrontRow;

        public Row EnemyBackRow => _enemyBackRow;

        public MiddleRow MiddleRow => _middleRow;

        private void Awake()
        {
            _canvas.worldCamera = Camera.main;
            PlayerMoney = GetComponent<PlayerMoney>();
            _saveLoadService = AllServices.Container.Single<ISaveLoadService>();
        }
        
        private void Start()
        {
            CreateEnemyDices();
            _battleMusic.Play();
            _player.LeadershipChanged += UpdateLeadership;
            UpdateLeadership(_player.Leadership);
            _coinsAmount.text = PlayerMoney.Coins.ToString();

            foreach (var button in _diceButtons) 
                button.onClick.AddListener(DeactivateDices);
        }
        
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.S))
            {
                _saveLoadService.SaveProgress();
            }
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
        
        public void RollDices()
        {
            PlayerFrontDice.Roll();
            PlayerBackDice.Roll();
            EnemyFrontDice.Roll();
            EnemyBackDice.Roll();
        }
        
        public void ChangeDiceFace(Dice dice, DiceFaceData dataToRemove, DiceFaceData dataToAdd)
        {
            foreach (var face in dice.Faces.Where(face => face.DiceFaceData == dataToRemove)) 
                face.ConstructDiceFace(dataToAdd);
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

        private void CreatePlayerDices()
        {
            PlayerFrontDice = CreateDice(_playerFrontDicePos);
            PlayerBackDice = CreateDice(_playerBackDicePos);
            InitDice(_playerFrontRow, PlayerFrontDice, _dices);
            InitDice(_playerBackRow, PlayerBackDice, _dices);
        }

        private void CreateEnemyDices()
        {
            EnemyFrontDice = CreateDice(_enemyFrontDicePos);
            EnemyBackDice = CreateDice(_enemyBackDicePos);
            InitDice(_enemyFrontRow, EnemyFrontDice, _dices);
            InitDice(_enemyBackRow, EnemyBackDice, _dices);
        }

        private void InitDice(Row row, Dice dice, List<Dice> dices)
        {
            row.InitDice(dice);
            dices.Add(dice);
        }

        private Dice LoadPlayerDice(List<DiceFaceData> faceData, Vector3 position)
        {
            Dice dice = Instantiate(_dicePrefab, position, Quaternion.identity);

            for (int i = 0; i < dice.Faces.Count; i++) 
                dice.Faces[i].ConstructDiceFace(faceData[i]);

            return dice;
        }

        private Dice CreateDice(Vector3 position)
        {
            Dice dice = Instantiate(_dicePrefab, position, Quaternion.identity);
            
            foreach (var diceFace in dice.Faces)
            {
                diceFace.DiceFaceData.SuitImage = diceFace.SuitIcon;
                diceFace.DiceFaceData.SuitType = diceFace.SuitType;
                diceFace.DiceFaceData.Material = diceFace.Material;
            }
            return dice;
        }

        private void OnPlayerDie(Unit unit)
        {
            _dieWindow.gameObject.SetActive(true);
            _player.GetComponent<Health>().Died -= OnPlayerDie;
        }
        
        public void Load(PlayerProgress progress)
        {
            _diceData = progress.PlayerStats.DiceData;

            if (_diceData.FrontDiceFaces.Count > 0)
            {
                PlayerFrontDice = LoadPlayerDice(progress.PlayerStats.DiceData.FrontDiceFaces, _playerFrontDicePos);
                PlayerBackDice = LoadPlayerDice(progress.PlayerStats.DiceData.BackDiceFaces, _playerBackDicePos);
                InitDice(_playerFrontRow, PlayerFrontDice, _dices);
                InitDice(_playerBackRow, PlayerBackDice, _dices);
            }
            else
                CreatePlayerDices();
        }

        public void Save(PlayerProgress progress)
        {
            progress.PlayerStats.DiceData.FrontDiceFaces.Clear();
            progress.PlayerStats.DiceData.BackDiceFaces.Clear();

            foreach (var diceFace in PlayerFrontDice.Faces)
                progress.PlayerStats.DiceData.FrontDiceFaces.Add(diceFace.DiceFaceData);

            foreach (var diceFace in PlayerBackDice.Faces)
                progress.PlayerStats.DiceData.BackDiceFaces.Add(diceFace.DiceFaceData);
        }
    }
}