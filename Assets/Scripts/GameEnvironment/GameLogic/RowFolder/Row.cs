using System.Collections.Generic;
using GameEnvironment.GameLogic.CardFolder;
using UnityEngine;

namespace GameEnvironment.GameLogic.RowFolder
{
    public class Row : MonoBehaviour
    {
        [SerializeField] private RowType _rowType;
        [SerializeField] private List<RowCardSlot> _guardSlots;

        private BoxCollider2D _collider;
        public List<RowCardSlot> GuardSlots => _guardSlots;

        private void Start()
        {
            _collider = GetComponent<BoxCollider2D>();
        }

        public void Activate()
        {
            _collider.enabled = true;

            foreach (var slot in _guardSlots) 
                slot.Collider2D.enabled = true;
        }

        public void Disactivate()
        {
            _collider.enabled = false;

            foreach (var slot in _guardSlots) 
                slot.Collider2D.enabled = false;
        }
        
        public bool CheckRowMatch(Unit guard)
        {
            if (guard.RowType == _rowType || guard.RowType == RowType.Generic)
            {
                return true;
            }
            return false;
        }

        public RectTransform GetFreeSlot()
        {
            for (int i = 0; i < _guardSlots.Count; i++)
            {
                if (_guardSlots[i].GetComponentInChildren<EnemyGuard>() == null)
                {
                    return _guardSlots[i].GetComponent<RectTransform>();
                }
            }

            return null;
        }
    }
}