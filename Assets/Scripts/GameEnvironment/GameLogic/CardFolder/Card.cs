﻿using System.Collections;
using UnityEngine;

namespace GameEnvironment.GameLogic.CardFolder
{
    public class Card : MonoBehaviour
    {
        [SerializeField] protected GameObject _backSprite;
        
        protected Vector3 _startPosition;
        private bool _isFacedUp = false;
        private bool _isCoroutineAllowed = true;
        
        public Vector3 StartPosition => _startPosition;

        protected virtual void Start()
        {            
        }

        public void Activate()
        {
            gameObject.layer = 8;
        }

        public void Disactivate() => 
            gameObject.layer = 5;

        public void Flip() => 
            StartCoroutine(Rotate());

        private IEnumerator Rotate()
        {
            _isCoroutineAllowed = false;
            
            if (!_isFacedUp)
            {
                for (float i = 0; i < 180; i += 10)
                {
                    transform.rotation = Quaternion.Euler(0,i,0);
                    if (i == 90)
                    {
                        _backSprite.SetActive(true);
                    }

                    yield return new WaitForSeconds(0.01f);
                }
            }
            else if (_isFacedUp)
            {
                for (float i = 180; i >= 0; i -= 10)
                {
                    transform.rotation = Quaternion.Euler(0,i,0);
                    if (i == 90)
                    {
                        _backSprite.SetActive(false);
                    }

                    yield return new WaitForSeconds(0.01f);
                }
            }

            _isCoroutineAllowed = true;
            _isFacedUp = !_isFacedUp;
        }
    }
}