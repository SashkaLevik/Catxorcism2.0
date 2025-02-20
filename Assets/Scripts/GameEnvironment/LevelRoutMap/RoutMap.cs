using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace GameEnvironment.LevelRoutMap
{
    public class RoutMap : MonoBehaviour
    {
        [SerializeField] private GameObject _routMapWindow;
        [SerializeField] private List<Button> _stageButtons;

        public event UnityAction StageEntered;
        
        private void Start()
        {
            foreach (var button in _stageButtons)
            {
                button.onClick.AddListener(OpenStage);
            }
        }

        private void OpenStage()
        {
            _routMapWindow.SetActive(false);
            StageEntered?.Invoke();
        }
    }
}