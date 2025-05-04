using GameEnvironment.GameLogic.RowFolder;

namespace GameEnvironment.GameLogic.CardFolder.SkillCards
{
    public class WoodBarrier : ObstacleSkill
    {
        public override void UseObstacleSkill(MiddleRow middleRow, RowCardSlot slot)
        {
            _cardSlot = slot;
            HideArrow();
            Disactivate();
            DisableCollider();
            _cardSlot.Occupy();
            StartCoroutine(Move(this, slot.SlotPosition));
        }
    }
}