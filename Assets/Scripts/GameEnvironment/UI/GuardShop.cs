using Assets.Scripts.Data;
using Assets.Scripts.Infrastructure.Services;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.GameEnvironment.UI
{
    public class GuardShop : MonoBehaviour, ISaveProgress
    {
        [SerializeField] private TMP_Text _coins;
        [SerializeField] private MenuHud _menuHud;
        [SerializeField] private List<CardData> _guardData;
        [SerializeField] private List<RectTransform> _slots;
        [SerializeField] private List<BuyButton> _buyButtons;
        [SerializeField] private Warning _warning;

        private TMP_Text _priceText;
        private PlayerProgress _progress;
        private CardData _choosedGuard;
        private List<CardData> _activeGuard = new List<CardData>();
        private List<CardData> _closeGuard = new List<CardData>();

        private void Start()
        {
            SpawnGuards();
            _coins.text = _menuHud.PlayerMoney.Coins.ToString();

            if (_progress.WorldData.IsNewGame == true)
            {
                _closeGuard = _guardData.ToList();
                OpenFirstGuard();
            }

            for (int i = 0; i < _activeGuard.Count; i++)
                _buyButtons[i].GetComponent<Button>().interactable = false;            
        }

        private void OnEnable()
        {
            foreach (var button in _buyButtons)
                button.GetComponent<Button>().onClick.AddListener(() => BuyGuard(button));
        }        

        private void OnDestroy()
        {           
            foreach (var button in _buyButtons)
                button.GetComponent<Button>().onClick.RemoveListener(() => BuyGuard(button));
        }

        private void SpawnGuards()
        {
            for (int i = 0; i < _slots.Count; i++)
            {
                Instantiate(_guardData[i].CardPrefab, _slots[i]);
                _buyButtons[i].GetCard(_guardData[i]);
                _priceText = _buyButtons[i].GetComponentInChildren<TMP_Text>();
                _priceText.text = _guardData[i].ActivatePrice.ToString();
            }
        }

        private void BuyGuard(BuyButton button)
        {
            _choosedGuard = button.Guard;

            if (_menuHud.PlayerMoney.Coins >= _choosedGuard.ActivatePrice)
            {
                _activeGuard.Add(_choosedGuard);
                _closeGuard.Remove(_choosedGuard);
                _menuHud.PlayerMoney.RemoveCoin(_choosedGuard.ActivatePrice, _coins);
                button.GetComponent<Button>().interactable = false;
            }
            else
                _warning.Show();
        }

        private void OpenFirstGuard()
        {
            _activeGuard.Add(_closeGuard[0]);
            _closeGuard.Remove(_closeGuard[0]);
        }       
       
        public void Load(PlayerProgress progress)
        {
            _progress = progress;

            if (progress.WorldData.IsNewGame == false)
            {
                _closeGuard = progress.CloseGuards.ToList();
                _activeGuard = progress.OpenedGuards.ToList();
            }
        }

        public void Save(PlayerProgress progress)
        {
            progress.CloseGuards = _closeGuard.ToList();
            progress.OpenedGuards = _activeGuard.ToList();
        }
    }
}