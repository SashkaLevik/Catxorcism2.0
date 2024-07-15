using Assets.Scripts.Infrastructure.Services;
using System.Collections;
using UnityEngine;
using Agava.YandexGames;

namespace Assets.Scripts.States
{
    public class SDKInit : MonoBehaviour
    {
        private IGameStateMachine _stateMachine;


        private void Awake()
        {
            _stateMachine = AllServices.Container.Single<IGameStateMachine>();
            //_stateMachine.Enter<ProgressState>();
            StartCoroutine(Init());
        }

        private IEnumerator Init()
        {
            yield return YandexGamesSdk.Initialize(OnInitialize);
        }

        private void OnInitialize()
        {
            _stateMachine.Enter<ProgressState>();
        }              
    }
}