using UnityEngine;

namespace GameEnvironment.GameLogic.CardFolder
{
    public class Card : MonoBehaviour
    {               
        protected Vector3 _startPosition;
        
        public Vector3 StartPosition => _startPosition;

        protected virtual void Start()
        {            
        }

        public void Activate()
        {
            gameObject.layer = 8;
        }

        public void Disactivate()
        {
            gameObject.layer = 5;
        }
            
    }
}