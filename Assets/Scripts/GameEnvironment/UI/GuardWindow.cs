using Assets.Scripts.Data;
using Assets.Scripts.GameEnvironment.GameLogic;
using Assets.Scripts.GameEnvironment.Units;
using Assets.Scripts.Infrastructure.Services;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.GameEnvironment.UI
{
    public class GuardWindow : MonoBehaviour, ISaveProgress
    {
        [SerializeField] private BattleHud _battleHud;
        [SerializeField] private DeckSpawner _deckSpawner;
        [SerializeField] private GameObject _guardWindow;
        [SerializeField] private GameObject _upgradeWindow;
        [SerializeField] private RectTransform _ugradeSlot;
        [SerializeField] private BuyButton _upgradeButton;
        [SerializeField] private Button _closeGuards;
        [SerializeField] private Button _closeUpgrades;
        [SerializeField] private List<CardData> _guards;
        [SerializeField] private List<RectTransform> _slots;
        [SerializeField] private List<BuyButton> _buyButtons;
        [SerializeField] private List<GuardSpawner> _spawners;

        private Player _player;
        private PlayerMoney _playerMoney;
        private CardData _choosedGuard;
        private Guard _spawnedGuard;
        private Guard _currentGuard;
        private TMP_Text _priceText;
        private TMP_Text _upgradeText;
        private GuardSpawner _currentSpawner;
        private int _maxLevel = 3;

        private void Start()
        {
            _player = _battleHud.Player;
            _playerMoney = _player.GetComponent<PlayerMoney>(); 
        }

        private void OnEnable()
        {
            foreach (var spawner in _spawners)
                spawner.SpawnerTriggered += SpawnGuards;

            foreach (var button in _buyButtons)
                button.GetComponent<Button>().onClick.AddListener(() => SummonGuard(button));

            _upgradeButton.GetComponent<Button>().onClick.AddListener(() => SummonUpgraded(_upgradeButton));
            _closeGuards.onClick.AddListener(CloseGuards);
            _closeUpgrades.onClick.AddListener(CloseUpgrades);
        }

        private void OnDestroy()
        {
            foreach (var spawner in _spawners)
                spawner.SpawnerTriggered -= SpawnGuards;

            foreach (var button in _buyButtons)
                button.GetComponent<Button>().onClick.RemoveListener(() => SummonGuard(button));

            _upgradeButton.GetComponent<Button>().onClick.RemoveListener(() => SummonUpgraded(_upgradeButton));
            _closeGuards.onClick.RemoveListener(CloseGuards);
            _closeUpgrades.onClick.RemoveListener(CloseUpgrades);
        }

        private void SummonUpgraded(BuyButton button)
        {
            _choosedGuard = button.Guard;

            if (_battleHud.PlayerMoney.Coins >= _choosedGuard.UpgradePrice)
            {
                _player.RemoveGuard(_currentGuard);
                Destroy(_currentGuard.gameObject);
                _battleHud.PlayerMoney.RemoveCoin(_choosedGuard.UpgradePrice, _battleHud.Coins);
                _spawnedGuard = (Guard)Instantiate(_choosedGuard.CardPrefab, _currentSpawner.transform);
                _spawnedGuard.OnGuardPressed += ShowUpgrades;
                _player.AddGuard(_spawnedGuard);
                _upgradeWindow.SetActive(false);
                _spawnedGuard.Activate();
            }
        }

        private void SummonGuard(BuyButton button)
        {
            _choosedGuard = button.Guard;

            if (_battleHud.PlayerMoney.Coins >= _choosedGuard.SummonPrice)
            {
                _battleHud.PlayerMoney.RemoveCoin(_choosedGuard.SummonPrice, _battleHud.Coins);
                _spawnedGuard = (Guard)Instantiate(_choosedGuard.CardPrefab, _currentSpawner.transform);
                _spawnedGuard.OnGuardPressed += ShowUpgrades;
                _player.AddGuard(_spawnedGuard);
                _guardWindow.SetActive(false);
                _spawnedGuard.Activate();
            }                   
        }

        private void ShowUpgrades(Guard guard, GuardSpawner spawner)
        {
            if (guard.CardData.Level < _maxLevel)
            {
                _upgradeWindow.SetActive(true);
                _currentSpawner = spawner;
                _currentGuard = guard;
                Instantiate(guard.GuardApgrade.CardPrefab, _ugradeSlot);
                _upgradeButton.GetCard(guard.GuardApgrade);
                _upgradeText = _upgradeButton.GetComponentInChildren<TMP_Text>();
                _upgradeText.text = guard.GuardApgrade.UpgradePrice.ToString();
            }            
        }

        private void SpawnGuards(GuardSpawner spawner)
        {
            _guardWindow.SetActive(true);
            _deckSpawner.DisactivateRaw();
            _currentSpawner = spawner;

            for (int i = 0; i < _guards.Count; i++)
            {
                Instantiate(_guards[i].CardPrefab, _slots[i]);
                _buyButtons[i].GetComponent<Button>().interactable = true;
                _buyButtons[i].GetCard(_guards[i]);
                _priceText = _buyButtons[i].GetComponentInChildren<TMP_Text>();
                _priceText.text = _guards[i].SummonPrice.ToString();
            }
        }

        private void CloseGuards()
        {
            _guardWindow.SetActive(false);
            _deckSpawner.ActivateRaw();
        }
            

        private void CloseUpgrades() =>
            _upgradeWindow.SetActive(false);

        public void Load(PlayerProgress progress)
        {
            _guards = progress.OpenedGuards.ToList();
        }

        public void Save(PlayerProgress progress)
        {
        }
    }
}