using System;
using System.Collections;
using Data;
using Infrastructure.Services;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace GameEnvironment.UI.PlayerWallet
{
    public class PlayerMoney : MonoBehaviour, ISaveProgress
    {
        private int _coins;
        private int _materials;
        private ISaveLoadService _saveLoadService;

        public int Coins => _coins;

        public int Materials => _materials;

        public event UnityAction MoneyChanged;

        private void Awake()
            => _saveLoadService = AllServices.Container.Single<ISaveLoadService>();

        private void Start()
        {
            MoneyChanged += SaveMoney;
        }

        public void SaveMoney()
            => _saveLoadService.SaveProgress();

        public void AddCoin(int value, TMP_Text text)
        {
            _coins += value;
            StartCoroutine(AddTreasure(_coins, text));
            MoneyChanged?.Invoke();
        }

        public void RemoveCoin(int value, TMP_Text text)
        {
            _coins -= value;
            StartCoroutine(RemoveTreasure(_coins, text));
            MoneyChanged?.Invoke();
        }
        
        public void AddMaterials(int value, TMP_Text text)
        {
            _materials += value;
            StartCoroutine(AddTreasure(_materials, text));
            MoneyChanged?.Invoke();
        }
        
        public void RemoveMaterials(int value, TMP_Text text)
        {
            _materials -= value;
            StartCoroutine(RemoveTreasure(_materials, text));
            MoneyChanged?.Invoke();
        }
        
        private IEnumerator AddTreasure(int newValue, TMP_Text text)
        {
            int value = int.Parse(text.text);

            while (value != newValue)
            {
                value++;
                text.text = value.ToString();
                yield return new WaitForSeconds(0.1f);
            }

            yield return null;
        }

        private IEnumerator RemoveTreasure(int newValue, TMP_Text text)
        {
            int value = int.Parse(text.text);

            while (value != newValue)
            {
                value--;
                text.text = value.ToString();
                yield return new WaitForSeconds(0.05f);
            }

            yield return null;
        }

        public void Load(PlayerProgress progress)
        {
            _coins = progress.Coins;
            _materials = progress.Materials;
        }

        public void Save(PlayerProgress progress)
        {
            progress.Coins = _coins;
            progress.Materials = _materials;
        }
    }
}