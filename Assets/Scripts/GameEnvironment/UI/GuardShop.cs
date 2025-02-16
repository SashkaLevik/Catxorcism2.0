using System.Collections.Generic;
using System.Linq;
using Data;
using Infrastructure.Services;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GameEnvironment.UI
{
    public class GuardShop : MonoBehaviour, ISaveProgress
    {
        [SerializeField] private TMP_Text _crystal;
        [SerializeField] private MenuHud _menuHud;
        [SerializeField] private List<CardData> _guardDatas;
        [SerializeField] private List<RectTransform> _slots;
        [SerializeField] private List<BuyButton> _buyButtons;
        [SerializeField] private Warning _warning;

        private TMP_Text _priceText;
        private TMP_Text _description;
        private PlayerProgress _progress;
        private CardData _choosedGuard;
        private List<string> _openedGuards = new List<string>();

        private void Start()
        {
            if (_progress.WorldData.IsNewGame == true)
                _openedGuards.Add(_guardDatas[0].EnName);

            SpawnGuards();                   
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
                Instantiate(_guardDatas[i].CardPrefab, _slots[i]);
                _buyButtons[i].GetCard(_guardDatas[i]);

                foreach (var guard in _openedGuards)
                {
                    if (_buyButtons[i].Guard.EnName == guard)
                        _buyButtons[i].GetComponent<Button>().interactable = false;
                }

                _priceText = _buyButtons[i].GetComponentInChildren<TMP_Text>();
                _priceText.text = _guardDatas[i].ActivatePrice.ToString();
                _description = _slots[i].GetComponentInChildren<TMP_Text>();
                _description.text = GetLocalizedDescription(_guardDatas[i]);
            }
        }

        private void BuyGuard(BuyButton button)
        {
            _choosedGuard = button.Guard;

            if (_menuHud.PlayerMoney.Crystals >= _choosedGuard.ActivatePrice)
            {
                _openedGuards.Add(_choosedGuard.EnName);
                _menuHud.PlayerMoney.RemoveCrystal(_choosedGuard.ActivatePrice, _crystal);
                button.GetComponent<Button>().interactable = false;
            }
        }
       
        private string GetLocalizedDescription(CardData cardData)
        {
            if (Application.systemLanguage == SystemLanguage.Russian)
                return cardData.RuDescription;
            else
                return cardData.EnDescription;
        }

        public void Save(PlayerProgress progress)
        {
        }

        public void Load(PlayerProgress progress)
        {
            _progress = progress;

            
        }       
    }
}