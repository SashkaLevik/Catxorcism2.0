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
        private Canvas _canvas;
        private Camera _camera;
        private Vector3 _worldPosition;

        public Card CurrentCard => _currentCard;

        public event UnityAction<Guard> GuardPlaced;

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

                    if (_currentCard.GetComponent<SkillCard>())
                    {
                        var skill = _currentCard.GetComponent<SkillCard>();

                        if (skill.Type == SkillType.UseOnGuard && hit2D.collider.TryGetComponent(out Guard guard))
                            skill.UseOnGuard(guard);
                        else if (skill.Type == SkillType.UseOnEnemy && hit2D.collider.TryGetComponent(out EnemyGuard enemyGuard))
                            skill.UseOnEnemy(enemyGuard);
                        else if (skill.Type == SkillType.UseOnWorld)
                            skill.UseSkill();
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
                    Guard guard = _currentCard.GetComponent<Guard>();
                    guard.InitRow(row, _slotIndex, _battleHud);
                    guard.InitEnemy(_enemySpawner.SpawnedEnemy);
                    GuardPlaced?.Invoke(guard);
                    _currentCard.Disactivate();
                    _currentCard = null;
                }
            }
        }
    }
}
