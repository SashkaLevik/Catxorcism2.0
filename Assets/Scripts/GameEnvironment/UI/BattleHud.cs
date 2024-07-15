using Assets.Scripts.GameEnvironment.GameLogic;
using Assets.Scripts.GameEnvironment.Units;
using Assets.Scripts.Infrastructure.GameManegment;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.GameEnvironment.UI
{
    public class BattleHud : MonoBehaviour
    {
        [SerializeField] private AudioSource _battleMusic;
        [SerializeField] private GameObject _guardWindow;
        [SerializeField] private Button _endTurn;
        [SerializeField] private Button _retreat;
        [SerializeField] private PlayerMoney _playerMoney;
        [SerializeField] private TMP_Text _coinsCount;
        [SerializeField] private TMP_Text _crystalsCount;
        [SerializeField] private GuardWindow _guardSpawner;
        [SerializeField] private Canvas _canvas;
        [SerializeField] private DeckSpawner _deck;
        [SerializeField] private DieWindow _dieWindow;
        [SerializeField] private Image[] _fullImages;
        [SerializeField] private Image[] _emptyImages;
        [SerializeField] private List<Button> _summonGuardButtons;

        private int _actionPoints = 2;
        private int _usePerTurn;        
        private Player _player;

        public Player Player => _player;
        public PlayerMoney PlayerMoney => _playerMoney;
        public TMP_Text Coins => _coinsCount;
        public TMP_Text Crystals => _crystalsCount;

        private void Awake() =>
            _canvas.worldCamera = Camera.main;        

        private void Start()
        {
            _coinsCount.text = _playerMoney.Coins.ToString();
            _crystalsCount.text = _playerMoney.Crystals.ToString();
            _usePerTurn = _actionPoints;
            UpdateCooldown();
            _endTurn.onClick.AddListener(EndPlayerTurn);
            _retreat.onClick.AddListener(OnPlayerDie);
            _battleMusic.Play();            
        }

        private void OnDestroy()
        {
            _endTurn.onClick.RemoveListener(EndPlayerTurn);
            _retreat.onClick.RemoveListener(OnPlayerDie);
        }        

        public void Construct(Player player)
        {
            _player = player;
            _player.EnemyAttacked += OnPlayerAttack;
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

            if (_usePerTurn == 0) EndPlayerTurn();
        }

        private void OnPlayerDie()
        {
            _playerMoney.SaveMoney();
            _dieWindow.gameObject.SetActive(true);
            _player.GetComponent<Health>().Died -= OnPlayerDie;
            _player.EnemyAttacked -= OnPlayerAttack;
        }

        private void OnPlayerAttack(bool value)
        {
            GetEnemies();

            if (value == true)
                _deck.DisactivateRaw();
            else
                _deck.ActivateRaw();
        }

        private void EndPlayerTurn() =>
            StartCoroutine(EnemyTurn());

        private IEnumerator EnemyTurn()
        {
            GetEnemies();
            yield return new WaitWhile(() => _player.IsAttacking);
            yield return new WaitForSeconds(0.2f);
            _deck.Attack();
        }        

        private void GetEnemies()
        {
            for (int i = 0; i < _summonGuardButtons.Count; i++)
            {
                var guard = _summonGuardButtons[i].GetComponentInChildren<Guard>();
                var enemy = _deck.FirstRaw[i].GetComponentInChildren<Enemy>();

                if (guard != null && enemy != null)
                {
                    guard.InitEnemy(enemy);
                    enemy.InitGuard(guard);
                }
            }
        }        
    }
}