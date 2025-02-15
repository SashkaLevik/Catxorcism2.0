using System.Collections.Generic;
using Data;
using Infrastructure.AssetManagment;
using Infrastructure.Services;
using UnityEngine;

namespace Infrastructure.Factory
{
    public class GameFactory : IGameFactory
    {
        private readonly IAssetProvider _assetProvider;

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

        public GameObject CreatePlayer(CardData cardData, GameObject at)
        {
            var player = Object.Instantiate(cardData.CardPrefab, at.transform);
            RegisterProgressWatchers(player.gameObject);
            return player.gameObject;
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
