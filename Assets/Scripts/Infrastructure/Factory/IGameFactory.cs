using Assets.Scripts.Data;
using Assets.Scripts.Infrastructure.Services;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Infrastructure.Factory
{
    public interface IGameFactory : IService
    {
        GameObject CreatePlayer(CardData CardData, GameObject at);

        GameObject CreateMenuHud();
        GameObject CreateBattleHud();
        //GameObject CreateSkillPanel();
        //GameObject CreateBattleSystem();
        //GameObject CreateArtifactsWatcher();
        List<ISaveProgress> ProgressWriters { get; }
        List<ILoadProgress> ProgressReaders { get; }
    }
}
