using GameEnvironment.GameLogic.CardFolder.SkillCards;
using UnityEngine;

namespace GameEnvironment.GameLogic.SuitBuff.PlayerGuardBuff
{
    public class MouseBuff : Buff
    {
        private ObstacleSkill _obstacleSkill;
        
        public override void ApplyBuff()
        {
            if (_guard.BattleHud.MiddleRow.RowSlots[_guard.SlotIndex].GetComponentInChildren<ObstacleSkill>() != null)
                Destroy(_guard.BattleHud.MiddleRow.RowSlots[_guard.SlotIndex].GetComponentInChildren<ObstacleSkill>().gameObject);
            
            var spawnPos = _guard.GetComponentInParent<RectTransform>();
            _obstacleSkill = (ObstacleSkill) Instantiate(_guard.BuffCard, spawnPos);
            _obstacleSkill.PlaceObstacle(_guard.BattleHud.MiddleRow, _guard.BattleHud.MiddleRow.RowSlots[_guard.SlotIndex]);
            _unitToBuff.IsBuffApplied = true;
        }

        public override void ResetBuff()
        {
            _unitToBuff.IsBuffApplied = false;
        }
    }
}