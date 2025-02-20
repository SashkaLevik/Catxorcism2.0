using System.Collections.Generic;
using GameEnvironment.GameLogic.CardFolder;
using UnityEngine;

namespace GameEnvironment.GameLogic.RowFolder
{
    public class Row : MonoBehaviour
    {
        [SerializeField] private RowType _rowType;
        [SerializeField] private List<RowCardSlot> _guardSlots;

        public List<RowCardSlot> GuardSlots => _guardSlots;

        private void Start()
        {
        }

        public bool CanPlaceGuard(Guard guard)
        {
            if (guard.RowType == _rowType || guard.RowType == RowType.Generic)
            {
                return true;
            }
            return false;
        }
    }
}