using System.Collections.Generic;
using Data;
using Infrastructure.Services;
using UnityEngine;

namespace Infrastructure.Factory
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
