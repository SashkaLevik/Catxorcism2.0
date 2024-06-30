using Assets.Scripts.Data;
using Assets.Scripts.Infrastructure.AssetManagment;
using Assets.Scripts.Infrastructure.Services;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Infrastructure.Factory
{
    public class GameFactory : IGameFactory
    {
        private readonly IAssetProvider _assetProvider;
        //private readonly IToyDataService _toyDataService;

        public List<ISaveProgress> ProgressWriters { get; } = new List<ISaveProgress>();

        public List<ILoadProgress> ProgressReaders { get; } = new List<ILoadProgress>();

        public GameFactory(IAssetProvider assetProvider)
        {
            _assetProvider = assetProvider;
        }        

        public GameObject CreateMenuHud()
        {
            GameObject menuHud = _assetProvider.Instantiate(AssetPath.MenuHud);
            RegisterProgressWatchers(menuHud);
            return menuHud;
        }

        public GameObject CreateBattleHud()
        {
            GameObject battleHud = _assetProvider.Instantiate(AssetPath.BattleHud);
            RegisterProgressWatchers(battleHud);
            return battleHud;
        }

        //public GameObject CreateSkillPanel()
        //{
        //    GameObject skillPanel = _assetProvider.Instantiate(AssetPath.SkillPanel);
        //    RegisterProgressWatchers(skillPanel.gameObject);
        //    return skillPanel;
        //}

        //public GameObject CreateBattleSystem()
        //{
        //    GameObject battleSystem = _assetProvider.Instantiate(AssetPath.BattleSystem);
        //    RegisterProgressWatchers(battleSystem);
        //    return battleSystem;
        //}

        public GameObject CreatePlayer(CardData cardData, GameObject at)
        {
            var Player = Object.Instantiate(cardData.CardPrefab, at.transform);
            RegisterProgressWatchers(Player.gameObject);
            return Player.gameObject;
        }

        private void RegisterProgressWatchers(GameObject obj)
        {
            foreach (ILoadProgress progressReader in obj.GetComponentsInChildren<ILoadProgress>())
            {
                Register(progressReader);
            }
        }

        private void Register(ILoadProgress progressReader)
        {
            if (progressReader is ISaveProgress progressWriter)
                ProgressWriters.Add(progressWriter);

            ProgressReaders.Add(progressReader);
        }
    }
}
