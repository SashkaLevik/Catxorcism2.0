using UnityEngine;
using UnityEngine.UI;

namespace GameEnvironment.GameLogic.RowFolder
{
    public class RowCardSlot : MonoBehaviour
    {
        private Image _image;
        private bool _isFree = true;

        public bool IsFree => _isFree;
        
        public RectTransform SlotPosition { get; private set; }

        public BoxCollider2D Collider2D { get; private set; }
        
        private void Start()
        {
            SlotPosition = GetComponent<RectTransform>();
            _image = GetComponent<Image>();
            Collider2D = GetComponent<BoxCollider2D>();
        }

        public void Occupy()
        {
            _isFree = false;
        }
        
        public void Clear()
        {
            _isFree = true;
        }
    }
}