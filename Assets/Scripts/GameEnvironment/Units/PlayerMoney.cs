using Assets.Scripts.Data;
using Assets.Scripts.Infrastructure.Services;
using System.Collections;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.GameEnvironment.Units
{
    public class PlayerMoney : MonoBehaviour, ISaveProgress
    {
        private int _coins;
        private int _crystals;
        private ISaveLoadService _saveLoadService;

        public int Coins => _coins;
        public int Crystals => _crystals;

        private void Awake()
            => _saveLoadService = AllServices.Container.Single<ISaveLoadService>();


        public void SaveMoney()
            => _saveLoadService.SaveProgress();

        public void AddCoin(int value, TMP_Text text)
        {
            _coins += value;
            StartCoroutine(AddTreasure(_coins, text));
        }

        public void AddCrystal(int value, TMP_Text text)
        {
            _crystals += value;
            StartCoroutine(AddTreasure(_crystals, text));
        }

        public void RemoveCoin(int value, TMP_Text text)
        {
            _coins -= value;
            StartCoroutine(RemoveTreasure(_coins, text));
        }

        public void RemoveCrystal(int value, TMP_Text text)
        {
            _crystals -= value;
            StartCoroutine(RemoveTreasure(_crystals, text));
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
            //_coins = progress.Coins;
            _crystals = progress.Crystals;
        }

        public void Save(PlayerProgress progress)
        {
            //progress.Coins = _coins;
            progress.Crystals = _crystals;
        }
    }
}