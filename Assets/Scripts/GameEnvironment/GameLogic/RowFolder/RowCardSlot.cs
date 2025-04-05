using UnityEngine;
using UnityEngine.UI;

namespace GameEnvironment.GameLogic.RowFolder
{
    public class RowCardSlot : MonoBehaviour
    {
        private Image _image;

        public BoxCollider2D Collider2D { get; private set; }
        
        private void Start()
        {
            _image = GetComponent<Image>();
            Collider2D = GetComponent<BoxCollider2D>();
        }

        public void Activate()
        {
            _image.enabled = true;
            //Collider2D.enabled = true;
        }
        
        public void Disactivate()
        {
            _image.enabled = false;
            //Collider2D.enabled = false;
        }
    }
}