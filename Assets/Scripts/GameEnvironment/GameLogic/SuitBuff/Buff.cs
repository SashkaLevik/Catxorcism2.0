using System;
using GameEnvironment.GameLogic.CardFolder;
using UnityEngine;
using UnityEngine.UI;

namespace GameEnvironment.GameLogic.SuitBuff
{
    public class Buff : MonoBehaviour
    {
        [SerializeField] private Sprite _buffIcon;
        [SerializeField] private Image _buffImage;
        
        protected Unit _unitToBuff;
        protected Guard _guard;

        private void Start()
        {
            _unitToBuff = GetComponent<Unit>();
            
            if (_unitToBuff.GetComponent<Guard>()) 
                _guard = _unitToBuff.GetComponent<Guard>();
        }

        public virtual void ApplyBuff()
        {
            //_buffImage.gameObject.SetActive(true);
            //_buffImage.sprite = _buffIcon;
        }

        public virtual void ResetBuff()
        {
            /*_buffImage.gameObject.SetActive(false);
            _buffImage = null;*/
        }

        public void UpdateBuff() => 
            _unitToBuff.IsBuffApplied = false;
    }
}