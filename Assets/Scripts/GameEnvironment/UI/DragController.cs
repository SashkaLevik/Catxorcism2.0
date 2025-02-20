using System;
using GameEnvironment.GameLogic;
using GameEnvironment.GameLogic.CardFolder;
using GameEnvironment.GameLogic.RowFolder;
using UnityEngine;
using UnityEngine.Events;

namespace GameEnvironment.UI
{
    public class DragController : MonoBehaviour
    {
        //[SerializeField] private BattleHud _battleHud;
        [SerializeField] private RectTransform _hand;
        [SerializeField] private LayerMask _draggable;
        [SerializeField] private LayerMask _cardSlotLayer;
        [SerializeField] private Warning _warning;

        private int _slotIndex;
        private Player _player;
        private Card _currentCard;
        private Canvas _canvas;
        private Camera _camera;
        private Vector3 _worldPosition;

        public event UnityAction GuardPlaced;

        private void Start()
        {
            _camera = Camera.main;
            _canvas = GetComponent<Canvas>();
        }

        public void Construct(Player player)
        {
            _player = player;
        }
        
        private void Update()
        {
            if (Input.GetMouseButton(0))
            {
                _worldPosition = _camera.ScreenToWorldPoint(Input.mousePosition);
                RaycastHit2D hit = Physics2D.Raycast(_worldPosition, Vector2.zero, Single.PositiveInfinity, _draggable);

                if (hit.collider != null)
                {
                    if (hit.transform.TryGetComponent(out Card card))
                    {
                        _currentCard = card;
                    }
                }

            }
            
            if (Input.GetMouseButtonUp(0))
            {
                Vector2 clickPosition = _camera.ScreenToWorldPoint(Input.mousePosition);
                RaycastHit2D[] hitInfo = Physics2D.RaycastAll(clickPosition, Vector2.zero);
                
                foreach (RaycastHit2D hit2D in hitInfo)
                {
                    if (hit2D.collider.TryGetComponent(out Row row))
                    {
                        if (_currentCard != null)
                        {
                            Debug.Log(row);
                            if (row.CanPlaceGuard(_currentCard.GetComponent<Guard>()))
                            {
                                TryPlaceGuard(row);
                                break;
                            }
                        }
                    }
                }
                
                ReturnOnTable();
            }
            
            if (_currentCard != null)
            {
                _currentCard.transform.position = new Vector3(_worldPosition.x, _worldPosition.y);
                _currentCard.transform.SetParent(_canvas.transform);
            }
        }

        private void ReturnOnTable()
        {
            if (_currentCard != null)
            {
                _currentCard.transform.position = _hand.transform.position;
                _currentCard.transform.SetParent(_hand);
                _currentCard = null;
            }
        }
        
        private void TryPlaceGuard(Row row)
        {
            Vector2 clickPosition = _camera.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D[] hitInfo = Physics2D.RaycastAll(clickPosition, Vector2.zero, Single.PositiveInfinity, _cardSlotLayer);

            foreach (var hit in hitInfo)
            {
                if (_player.Leadership <= 0)
                {
                    _warning.Show(_warning.NoLeadership);
                    break;
                }
                
                if (hit.collider.TryGetComponent(out RowCardSlot cardSlot))
                {
                    _currentCard.transform.position = cardSlot.transform.position;
                    _currentCard.transform.SetParent(cardSlot.transform, true);
                    _slotIndex = row.GuardSlots.IndexOf(cardSlot);
                    _currentCard.GetComponent<Guard>().GetSlotIndex(_slotIndex);
                    GuardPlaced?.Invoke();
                    _currentCard.Disactivate();
                    _currentCard = null;
                }
            }
        }
    }
}
