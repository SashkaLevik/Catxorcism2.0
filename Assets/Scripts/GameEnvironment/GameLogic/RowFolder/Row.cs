using System.Collections.Generic;
using GameEnvironment.GameLogic.CardFolder;
using GameEnvironment.GameLogic.DiceFolder;
using UnityEngine;
using UnityEngine.UI;

namespace GameEnvironment.GameLogic.RowFolder
{
    public class Row : MonoBehaviour
    {
        [SerializeField] private RowType _rowType;
        [SerializeField] private SuitType _rowSuit;
        [SerializeField] private Button _diceButton;
        [SerializeField] private Dice _dice;
        [SerializeField] private List<RowCardSlot> _guardSlots;

        private Image _suitImage;
        private BoxCollider2D _collider;

        public SuitType RowSuit => _rowSuit;

        public List<RowCardSlot> GuardSlots => _guardSlots;

        private void Start()
        {
            _collider = GetComponent<BoxCollider2D>();
            _suitImage = _diceButton.GetComponent<Image>();
            _diceButton.onClick.AddListener(OnDiceButton);
        }

        private void OnDestroy()
        {
            _dice.OnDiceResult -= ChangeSuit;
            _diceButton.onClick.RemoveListener(OnDiceButton);
        }
            

        public void InitDice(Dice dice)
        {
            _dice = dice;
            _dice.OnDiceResult += ChangeSuit;
        }

        public void Activate()
        {
            _collider.enabled = true;

            foreach (var slot in _guardSlots) 
                slot.Collider2D.enabled = true;
        }

        public void Disactivate()
        {
            _collider.enabled = false;

            foreach (var slot in _guardSlots) 
                slot.Collider2D.enabled = false;
        }

        public bool CheckRowMatch(Unit guard)
        {
            if (guard.RowType == _rowType || guard.RowType == RowType.Generic)
            {
                return true;
            }
            return false;
        }
        
        public RowCardSlot GetFreeSlot()
        {
            for (int i = 0; i < _guardSlots.Count; i++)
            {
                if (_guardSlots[i].IsFree)
                {
                    return _guardSlots[i];
                }
            }

            return null;
        }
        
        /*public RectTransform GetFreeSlot()
        {
            for (int i = 0; i < _guardSlots.Count; i++)
            {
                if (_guardSlots[i].GetComponentInChildren<EnemyGuard>() == null)
                {
                    return _guardSlots[i].GetComponent<RectTransform>();
                }
            }

            return null;
        }*/

        private void ChangeSuit(DiceFace diceFace)
        {
            _rowSuit = diceFace.SuitType;
            _suitImage.sprite = diceFace.FaceData.SuitImage;

            foreach (var slot in _guardSlots)
            {
                if (slot.GetComponentInChildren<Unit>() != null)
                    slot.GetComponentInChildren<Unit>().CheckSuitMatch(this);
            }
        }
        
        private void OnDiceButton() => 
            _dice.Roll();

        public List<Guard> GetRowGuards()
        {
            List<Guard> rowGuards = new List<Guard>();
            
            foreach (var slot in _guardSlots)
            {
                if (slot.GetComponentInChildren<Guard>() != null)
                {
                    rowGuards.Add(slot.GetComponentInChildren<Guard>());
                }
            }

            return rowGuards;
        }
    }
}