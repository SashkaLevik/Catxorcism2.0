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
        [SerializeField] private List<Button> _eventButtons;
        
        public event UnityAction StageButtonPressed;

        private void Start()
        {
            foreach (var button in _stageButtons)
                button.onClick.AddListener(() => EnterStage(button));
        }

        private void OnDestroy()
        {
            foreach (var button in _stageButtons)
                button.onClick.RemoveListener(() => EnterStage(button));
        }   
        
        private void EnterStage(Button button)
        {
            StageButtonPressed?.Invoke();
            button.interactable = false;
            this.gameObject.SetActive(false);
        }
    }
}