using Assets.Scripts.Data;
using Assets.Scripts.GameEnvironment.GameLogic;
using Assets.Scripts.GameEnvironment.Units;
using Assets.Scripts.Infrastructure.Services;
using Assets.Scripts.States;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.GameEnvironment.UI
{
    public class BattleHud : MonoBehaviour, ISaveProgress
    {
        [SerializeField] private Player _player;
        [SerializeField] private GameObject _guardWindow;
        [SerializeField] private Button _closeGuards;
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

        private int _actionPoints;
        private int _usePerTurn;
        private ISaveLoadService _saveLoadService;

        public Player Player => _player;
        public PlayerMoney PlayerMoney => _playerMoney;
        public TMP_Text Coins => _coinsCount;
        public TMP_Text Crystals => _crystalsCount;

        private void Awake()
        {
            _saveLoadService = AllServices.Container.Single<ISaveLoadService>();
            _canvas.worldCamera = Camera.main;
        }

        private void Start()
        {
            _coinsCount.text = _playerMoney.Coins.ToString();
            _crystalsCount.text = _playerMoney.Crystals.ToString();
        }              

        private void OnDestroy()
        {
            _player.EnemyAttacked -= OnPlayerAttack;
        }

        public void Construct(Player player)
        {
            _player = player;
            _player.EnemyAttacked += OnPlayerAttack;
            _player.GetComponent<Health>().Died += OnPlayerDie;
            _actionPoints = _player.CardData.AP;
            _usePerTurn = _actionPoints;
            UpdateCooldown();
        }

        private void OnPlayerDie()
        {
            _saveLoadService.SaveProgress();
            _dieWindow.gameObject.SetActive(true);
            _player.GetComponent<Health>().Died -= OnPlayerDie;
        }

        private void OnPlayerAttack(bool value)
        {
            if (value == true)
                _deck.DisactivateRaw();
            else
                _deck.ActivateRaw();
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

        private void EndPlayerTurn()
        {
            StartCoroutine(EnemyTurn());
        }

        private IEnumerator EnemyTurn()
        {
            yield return new WaitForSeconds(0.5f);
            _deck.Attack();
            yield return new WaitForSeconds(0.5f);
            ResetCooldown();
        }

        public void ResetCooldown()
        {
            _usePerTurn = _actionPoints;
            UpdateCooldown();
        }

        public void Save(PlayerProgress progress)
        {
        }

        public void Load(PlayerProgress progress)
        {
        }
    }
}