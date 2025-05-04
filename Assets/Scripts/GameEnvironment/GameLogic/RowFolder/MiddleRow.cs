using System.Collections.Generic;
using UnityEngine;

namespace GameEnvironment.GameLogic.RowFolder
{
    public class MiddleRow : MonoBehaviour
    {
        [SerializeField] private List<RowCardSlot> _rowSlots;
        
        public List<RowCardSlot> RowSlots => _rowSlots;


    }
}