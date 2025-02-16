using System;
using GameEnvironment.GameLogic;
using GameEnvironment.GameLogic.CardFolder;
using GameEnvironment.Units;
using UnityEngine;

namespace GameEnvironment.UI
{
    public class DragController : MonoBehaviour
    {
        [SerializeField] private BattleHud _battleHud;
        [SerializeField] private LayerMask _draggable;
        [SerializeField] private LayerMask _cardSlotLayer;
        [SerializeField] private Warning _warning;

        private Card _currentCard;
        private Camera _camera;
        private Vector3 _previpusMousePos;
        private Vector3 _mouseDelta;

        private void Start()
        {
            _camera = Camera.main;
        }

        private void Update()
        {
            var currentMousePosition = _camera.ScreenToWorldPoint(Input.mousePosition);
            currentMousePosition.z = 0;
            _mouseDelta = currentMousePosition - _previpusMousePos;
            
            if (Input.GetMouseButtonDown(0))
            {
                Vector2 clickPosition = _camera.ScreenToWorldPoint(Input.mousePosition);
                RaycastHit2D hit = Physics2D.Raycast(clickPosition, Vector2.zero, Single.PositiveInfinity, _draggable);

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
                        if (row.CanPlaceGuard(_currentCard))
                        {
                                    
                        }
                    }
                }
            }
            
            //_currentCard.transform.position += new Vector3(_mouseDelta.x, _mouseDelta.y, 0);

        }

        private void ReturnOnTable()
        {
            
        }
        
        private void CanPlaceGuard(Row row)
        {
            Vector2 clickPosition = _camera.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D[] hitInfo = Physics2D.RaycastAll(clickPosition, Vector2.zero, Single.PositiveInfinity, _cardSlotLayer);

            foreach (var hit in hitInfo)
            {
                if (hit.collider)
                {
                
                }
            }
        }
    }
}
