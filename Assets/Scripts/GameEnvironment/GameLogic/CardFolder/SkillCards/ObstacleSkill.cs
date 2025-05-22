using System.Collections;
using GameEnvironment.GameLogic.RowFolder;
using UnityEngine;

namespace GameEnvironment.GameLogic.CardFolder.SkillCards
{
    public class ObstacleSkill : SkillCard
    {
        [SerializeField] protected bool _isBlocked;
        [SerializeField] protected bool _isHarmful;
        
        protected RowCardSlot _cardSlot;
        private float _moveSpeed = 30f;

        public bool IsBlocked => _isBlocked;

        public bool IsHarmful => _isHarmful;

        public virtual void PlaceObstacle(MiddleRow middleRow, RowCardSlot slot){}

        public void TakeDamage()
        {
            _appliedValue -= 1;
            _valueAmount.text = _appliedValue.ToString();

            if (_appliedValue <= 0)
            {
                _cardSlot.Clear();
                Destroy(gameObject);
            }
        }

        protected IEnumerator Move(Card card, RectTransform newPos)
        {            
            while (card.transform.position != newPos.transform.position)
            {
                card.transform.position = Vector3.MoveTowards(card.transform.position, newPos.transform.position,
                    _moveSpeed * Time.deltaTime);
                yield return null;
            }
            card.transform.SetParent(newPos);
        }
    }
}