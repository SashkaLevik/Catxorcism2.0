using GameEnvironment.GameLogic.RowFolder;
using UnityEngine;

namespace GameEnvironment.GameLogic.CardFolder.SkillCards
{
    public class WoodBarrier : ObstacleSkill
    {
        public override void PlaceObstacle(MiddleRow middleRow, RowCardSlot slot)
        {
            _cardSlot = slot;
            HideArrow();
            Inactivate();
            DisableCollider();
            _cardSlot.Occupy();
            StartCoroutine(Move(this, slot.SlotPosition));
        }
    }
}