using System.Collections.Generic;
using GameEnvironment.GameLogic.CardFolder;
using UnityEngine;

namespace GameEnvironment.UI
{
    public class GuardsContainer : MonoBehaviour
    {
        [SerializeField] private List<RectTransform> _positions;

        private RectTransform _currentPosition;
        private List<Guard> _guards;

        public void SetIndex()
        {
            
        }

        public void Return(Guard guard)
        {
            
        }

        public void AddGuard(Guard guard)
        {
            _guards.Add(guard);
        }
        
        private RectTransform GetPosition()
        {
            foreach (var position in _positions)
            {
                if (position.GetComponentInChildren<Guard>() == null)
                {
                    _currentPosition = position;
                    return _currentPosition;
                }
            }
            return null;
        }
    }
}