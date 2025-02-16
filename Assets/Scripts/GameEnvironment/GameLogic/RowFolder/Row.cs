using System;
using System.Collections.Generic;
using GameEnvironment.GameLogic.CardFolder;
using GameEnvironment.GameLogic.DiceFolder;
using UnityEngine;

namespace GameEnvironment.GameLogic
{
    public class Row : MonoBehaviour
    {
        [SerializeField] private RowType _rowType;
        [SerializeField] private List<RowCardSlot> _guardSlots;

        private void Start()
        {
        }

       
        
        public bool CanPlaceGuard(Card card)
        {
            if (_rowType == card.GetComponent<Guard>().RowType)
            {
                return true;
            }
            return false;
        }
    }
}