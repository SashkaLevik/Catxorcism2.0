using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GameEnvironment.GameLogic.CardFolder
{
    public class Card : MonoBehaviour
    {
        [SerializeField] protected GameObject _backSprite;
        
        protected Vector3 _startPosition;
        protected BoxCollider2D _collider;
        private int _positionIndex;
        protected bool _isInHand = false;
        private bool _isFacedUp = false;
        private bool _isCoroutineAllowed = true;
        private Canvas _canvas;
        private Transform _cardTransform;
        private RectTransform _handPosition;

        public Vector3 StartPosition => _startPosition;

        protected virtual void Start()
        {
            _collider = GetComponent<BoxCollider2D>();
            _cardTransform = GetComponent<Transform>();
        }

        public void GetCanvas(Canvas canvas, RectTransform handPosition)
        {
            _canvas = canvas;
            _handPosition = handPosition;
        }
        
        public virtual void Activate()
        {
            _isInHand = true;
            _collider.enabled = true;
        }

        public virtual void Disactivate()
        {
            _isInHand = false;
        }

        public void DisableCollider() => 
            _collider.enabled = false;

        public void Flip() => 
            StartCoroutine(Rotate());

        public void SetDefaultScale()
        {
            var localScale = _cardTransform.localScale;
            var localPosition = _cardTransform.localPosition;
            localScale = new Vector3(1, 1, 1);
            localPosition = new Vector3(localPosition.x, 0, localPosition.z);
            _cardTransform.localScale = localScale;
            _cardTransform.localPosition = localPosition;
        }
        
        protected virtual void OnMouseEnter()
        {
            if (_isInHand)
            {
                List<Card> handCards = new List<Card>();
                
                if (_handPosition.GetComponentsInChildren<Card>().Length > 0)
                    handCards = _handPosition.GetComponentsInChildren<Card>().ToList();

                foreach (var card in handCards)
                    if (card == this)
                        _positionIndex = handCards.IndexOf(card);
                
                _cardTransform.SetParent(_canvas.transform);
                var localScale = _cardTransform.localScale;
                var localPosition = _cardTransform.localPosition;
                localScale = new Vector3(localScale.x + 0.5f, localScale.y + 0.5f, localScale.z + 0.5f);
                localPosition = new Vector3(localPosition.x, localPosition.y + 50, localPosition.z);
                _cardTransform.localScale = localScale;
                _cardTransform.localPosition = localPosition;
            }
        }

        protected virtual void OnMouseExit()
        {
            if (_isInHand)
            {
                _cardTransform.SetParent(_handPosition);
                _cardTransform.SetSiblingIndex(_positionIndex);
                SetDefaultScale();
            }
        }
        
        private IEnumerator Rotate()
        {
            _isCoroutineAllowed = false;
            
            if (!_isFacedUp)
            {
                for (float i = 0; i < 190; i += 10)
                {
                    transform.rotation = Quaternion.Euler(0,i,0);
                    if (i == 90) 
                        _backSprite.SetActive(true);

                    yield return new WaitForSeconds(0.01f);
                }
            }
            else if (_isFacedUp)
            {
                for (float i = 180; i >= 0; i -= 10)
                {
                    transform.rotation = Quaternion.Euler(0,i,0);
                    if (i == 90) 
                        _backSprite.SetActive(false);

                    yield return new WaitForSeconds(0.01f);
                }
            }

            _isCoroutineAllowed = true;
            _isFacedUp = !_isFacedUp;
        }
    }
}