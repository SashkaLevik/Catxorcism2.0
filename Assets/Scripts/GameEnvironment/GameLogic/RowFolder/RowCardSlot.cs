using UnityEngine;
using UnityEngine.UI;

namespace GameEnvironment.GameLogic.RowFolder
{
    public class RowCardSlot : MonoBehaviour
    {
        private Image _image;
        private BoxCollider2D _collider;

        private void Start()
        {
            _image = GetComponent<Image>();
            _collider = GetComponent<BoxCollider2D>();
        }

        public void Activate()
        {
            _image.enabled = true;
            _collider.enabled = true;
        }
        
        public void Disactivate()
        {
            _image.enabled = false;
            _collider.enabled = false;
        }
    }
}