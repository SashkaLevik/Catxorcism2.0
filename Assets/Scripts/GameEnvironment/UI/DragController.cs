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
        [SerializeField] private BattleHud _battleHud;
        [SerializeField] private EnemySpawner _enemySpawner;
        [SerializeField] private DeckCreator _deckCreator;
        [SerializeField] private RectTransform _hand;
        [SerializeField] private LayerMask _draggable;
        [SerializeField] private LayerMask _cardSlotLayer;
        [SerializeField] private LayerMask _guardTrigger;
        [SerializeField] private Warning _warning;
        
        private int _slotIndex;
        private Player _player;
        private Card _currentCard;
        private Guard _currentGuard;
        private SkillCard _currentSkill;
        private SkillCard _previousSkill;
        private Canvas _canvas;
        private Camera _camera;
        private Vector3 _worldPosition;
        
        public SkillCard CurrentSkill => _currentSkill;

        public event UnityAction<Guard> GuardPlaced;
        public event UnityAction<SkillCard> SkillPlayed; 

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
            if (Input.GetMouseButtonDown(0))
            {
                _worldPosition = _camera.ScreenToWorldPoint(Input.mousePosition);
                RaycastHit2D hit = Physics2D.Raycast(_worldPosition, Vector2.zero, Single.PositiveInfinity);

                if (hit.collider != null)
                {
                    if (hit.transform.TryGetComponent(out Guard guard) && _currentSkill != null)
                    {
                        if (guard.IsOnField)
                        {
                            if (guard.ActionPoints <= 0)
                            {
                                _warning.Show(_warning.NoAP);
                                return;
                            }
                            
                            _currentSkill.UseOnGuard(guard);
                            SkillPlayed?.Invoke(_currentSkill);
                            _currentSkill.HideArrow();
                            _deckCreator.DiscardPlayedSkill(_currentSkill);
                            _currentSkill = null;
                        }
                        else
                        {
                            _currentSkill.HideArrow();
                            _currentSkill = null;
                        }
                    }
                    
                    if (hit.transform.TryGetComponent(out SkillCard skillCard))
                    {
                        if (_currentSkill == null)
                        {
                            _currentSkill = skillCard;
                            _currentSkill.ShowArrow();
                        }
                        else
                        {
                            _previousSkill = _currentSkill;
                            _previousSkill.HideArrow();
                            _currentSkill = skillCard;
                            _currentSkill.ShowArrow();
                        }
                    }
                }
            }
            if (Input.GetMouseButton(0))
            {
                _worldPosition = _camera.ScreenToWorldPoint(Input.mousePosition);
                RaycastHit2D hit = Physics2D.Raycast(_worldPosition, Vector2.zero, Single.PositiveInfinity, _draggable);
                
                if (hit.collider != null && _currentCard == null)
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
                    if (_currentCard == null)
                        break;
                    
                    if (_currentCard.GetComponent<Guard>() & hit2D.collider.TryGetComponent(out Row row))
                    {
                        if (row.CheckRowMatch(_currentCard.GetComponent<Guard>()))
                        {
                            TryPlaceGuard(row);
                            break;
                        }
                    }
                }
                
                ReturnOnTable();
            }

            if (Input.GetMouseButtonDown(1))
            {
                if (_currentSkill != null)
                {
                    _currentSkill.HideArrow();
                    _currentSkill = null;
                }
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
                _currentCard.SetDefaultScale();
                _deckCreator.UpdateHandVisual();
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
                    _currentCard.SetDefaultScale();
                    _currentCard.transform.position = cardSlot.transform.position;
                    _currentCard.transform.SetParent(cardSlot.transform, true);
                    _slotIndex = row.GuardSlots.IndexOf(cardSlot);
                    Guard guard = _currentCard.GetComponent<Guard>();
                    guard.InitRow(row, _slotIndex, this);
                    guard.InitEnemy(_enemySpawner.SpawnedEnemy);
                    GuardPlaced?.Invoke(guard);
                    guard.AddOnField();
                    cardSlot.Disactivate();
                    _deckCreator.UpdateHandVisual();
                    _currentCard.Disactivate();
                    _currentCard = null;
                }
            }
        }
    }
}
