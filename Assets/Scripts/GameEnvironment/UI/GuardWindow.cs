using System.Collections.Generic;
using System.Linq;
using Data;
using GameEnvironment.GameLogic;
using GameEnvironment.GameLogic.CardFolder;
using GameEnvironment.Units;
using Infrastructure.Services;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GameEnvironment.UI
{
    public class GuardWindow : MonoBehaviour, ISaveProgress
    {
        [SerializeField] private BattleHud _battleHud;
        [SerializeField] private GameObject _guardWindow;
        [SerializeField] private GameObject _upgradeWindow;
        [SerializeField] private RectTransform _ugradeSlot;
        [SerializeField] private BuyButton _upgradeButton;
        [SerializeField] private Button _closeGuards;
        [SerializeField] private Button _closeUpgrades;
        [SerializeField] private Warning _warning;
        [SerializeField] private List<CardData> _guards;
        [SerializeField] private List<RectTransform> _slots;
        [SerializeField] private List<BuyButton> _buyButtons;
        [SerializeField] private List<GuardSpawner> _spawners;

        private int _maxLevel = 3;
        private Player _player;
        private CardData _choosedGuard;
        private Guard _spawnedGuard;
        private Guard _currentGuard;
        private TMP_Text _priceText;
        private TMP_Text _upgradeText;
        private GuardSpawner _currentSpawner;
        private List<CardData> _guardsToSpawn = new List<CardData>();
        private List<string> _openedGuards = new List<string>();

        private void Start()
        {
            _player = _battleHud.Player;

            for (int i = 0; i < _guards.Count; i++)
            {
                if (IsOpen(_guards[i]))
                    _guardsToSpawn.Add(_guards[i]);
            }
        }

        private void OnEnable()
        {
            foreach (var spawner in _spawners)
                spawner.SpawnerTriggered += SpawnGuards;

            foreach (var button in _buyButtons)
                button.GetComponent<Button>().onClick.AddListener(() => SummonGuard(button));

            _upgradeButton.GetComponent<Button>().onClick.AddListener(() => SummonUpgraded(_upgradeButton));
            _closeGuards.onClick.AddListener(CloseGuardWindow);
            _closeUpgrades.onClick.AddListener(CloseUpgrades);
        }

        private void OnDestroy()
        {
            foreach (var spawner in _spawners)
                spawner.SpawnerTriggered -= SpawnGuards;

            foreach (var button in _buyButtons)
                button.GetComponent<Button>().onClick.RemoveListener(() => SummonGuard(button));

            _upgradeButton.GetComponent<Button>().onClick.RemoveListener(() => SummonUpgraded(_upgradeButton));
            _closeGuards.onClick.RemoveListener(CloseGuardWindow);
            _closeUpgrades.onClick.RemoveListener(CloseUpgrades);
        }

        private void SummonUpgraded(BuyButton button)
        {
            _choosedGuard = button.Guard;

            /*if (_battleHud.PlayerMoney.Coins >= _choosedGuard.UpgradePrice)
            {
                Destroy(_currentGuard.gameObject);
                _battleHud.PlayerMoney.RemoveCoin(_choosedGuard.UpgradePrice, _battleHud.Coins);
                _spawnedGuard = (Guard)Instantiate(_choosedGuard.CardPrefab, _currentSpawner.transform);
                _spawnedGuard.OnGuardPressed += ShowUpgrades;
                _upgradeWindow.SetActive(false);
                _spawnedGuard.ActivateGuard();
            }*/
          
        }        

        private void SummonGuard(BuyButton button)
        {
            _choosedGuard = button.Guard;

            /*if (_battleHud.PlayerMoney.Coins >= _choosedGuard.SummonPrice)
            {
                _battleHud.PlayerMoney.RemoveCoin(_choosedGuard.SummonPrice, _battleHud.Coins);
                _spawnedGuard = (Guard)Instantiate(_choosedGuard.CardPrefab, _currentSpawner.transform);
                _spawnedGuard.OnGuardPressed += ShowUpgrades;
                _spawnedGuard.ActivateGuard();
                CloseGuardWindow();
            }*/
        }

        private void ShowUpgrades(Guard guard, GuardSpawner spawner)
        {
            if (guard.CardData.Level < _maxLevel)
            {
                _upgradeWindow.SetActive(true);
                _currentSpawner = spawner;
                _currentGuard = guard;
                Instantiate(guard.GuardUpgrade.CardPrefab, _ugradeSlot);
                _upgradeButton.GetCard(guard.GuardUpgrade);
                _upgradeText = _upgradeButton.GetComponentInChildren<TMP_Text>();
                _upgradeText.text = guard.GuardUpgrade.UpgradePrice.ToString();
            }            
        }

        private void SpawnGuards(GuardSpawner spawner)
        {
            _guardWindow.SetActive(true);
            _currentSpawner = spawner;            

            for (int i = 0; i < _guardsToSpawn.Count; i++)
            {
                Instantiate(_guardsToSpawn[i].CardPrefab, _slots[i]);
                _buyButtons[i].GetComponent<Button>().interactable = true;
                _buyButtons[i].GetCard(_guards[i]);
                _priceText = _buyButtons[i].GetComponentInChildren<TMP_Text>();
                _priceText.text = _guardsToSpawn[i].SummonPrice.ToString();
            }
        }

        private void CloseGuardWindow()
        {
            _guardWindow.SetActive(false);
        }            

        private void CloseUpgrades() =>
            _upgradeWindow.SetActive(false);

        private bool IsOpen(CardData data)
        {
            foreach (var name in _openedGuards)
                if (data.EnName == name) return true;

            return false;
        }

        public void Load(PlayerProgress progress)
        {
        }

        public void Save(PlayerProgress progress)
        {
        }
    }
}