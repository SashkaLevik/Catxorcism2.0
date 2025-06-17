using System.Collections.Generic;
using Data;
using UnityEngine;
using UnityEngine.UI;

namespace GameEnvironment.LevelRoutMap.RoutEventWindows
{
    public class EventButton : MonoBehaviour
    {
        public EventButtonData ButtonData;
        public PathData PathData;
        public RoutEventType _routEventType;
        private Button _button;
        private RoutEvent _routEvent;
        private RoutMap _routMap;
        private bool _isVisited;
        private bool _isReachable;
        private List<EventButton> _connectedButtons = new List<EventButton>();

        public RoutEventType RoutEventType => _routEventType;

        public bool IsReachable => _isReachable;

        public List<EventButton> ConnectedButtons => _connectedButtons;

        private void Awake()
        {
            _button = GetComponent<Button>();
            _button.interactable = false;
            _button.onClick.AddListener(OpenEvent);
        }

        public void Construct(RoutMap routMap) => 
            _routMap = routMap;

        public void SetType(RoutEventType routEventType) => 
            this._routEventType = routEventType;

        public void AddConnectedButton(EventButton button)
        {
            if (!_connectedButtons.Contains(button)) 
                _connectedButtons.Add(button);
        }

        public void SetVisited(bool isVisited)
        {
            _isVisited = isVisited;
        }
        
        public void SetReachable(bool isReachable)
        {
            _isReachable = isReachable;
            _button.interactable = _isReachable;
        }

        private void OnMouseEnter()
        {
            if (_isReachable) transform.localScale = Vector3.one * 1.2f;
        }

        private void OnMouseExit() => 
            transform.localScale = Vector3.one;

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