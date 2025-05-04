using System.Collections;
using GameEnvironment.GameLogic.RowFolder;
using UnityEngine;

namespace GameEnvironment.GameLogic.CardFolder.SkillCards
{
    public class ObstacleSkill : SkillCard
    {
        protected RowCardSlot _cardSlot;
        private float _moveSpeed = 30f;

        public virtual void UseObstacleSkill(MiddleRow middleRow, RowCardSlot slot){}

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