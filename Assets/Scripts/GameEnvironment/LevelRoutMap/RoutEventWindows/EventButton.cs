using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace GameEnvironment.LevelRoutMap.RoutEventWindows
{
    public class EventButton : MonoBehaviour
    {
        public RoutEventType routEventType;
        private Button _button;
        private RoutEvent _routEvent;
        private RoutMap _routMap;
        private bool _isReachable;
        public List<EventButton> ConnectedButtons = new List<EventButton>();

        private void Awake()
        {
            _button = GetComponent<Button>();
            _button.interactable = false;
            _button.onClick.AddListener(OpenEvent);
        }

        public void Construct(RoutMap routMap) => 
            _routMap = routMap;

        public void SetType(RoutEventType routEventType) => 
            this.routEventType = routEventType;

        public void AddConnectedButton(EventButton button)
        {
            if (!ConnectedButtons.Contains(button)) 
                ConnectedButtons.Add(button);
        }

        public void SetReachable(bool isReachable)
        {
            _isReachable = isReachable;
            _button.interactable = _isReachable;
        }
        
        private void OpenEvent()
        {
            if (_isReachable)
            {
                _routMap.OpenEvent(this);
                _routMap.SetCurrentButton(this);
            }
        }
    }
}