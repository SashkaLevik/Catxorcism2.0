using System.Collections.Generic;
using System.Linq;
using GameEnvironment.GameLogic.RowFolder;
using UnityEngine;

namespace GameEnvironment.GameLogic.CardFolder.SkillCards
{
    public class AttackSkill : SkillCard
    {
        public override void UseSkill(Unit unit)
        {
            if (unit.CurrentEnemy != null)
            {
                var target = unit.CurrentEnemy;
                unit.Attack(this, target, unit.CurrentDamage);
            }
            
            if (GetComponent<Guard>()) 
                GetComponent<Guard>().OnSkillPlayed(this);
        }

        private Unit GetRandomTarget(Row frontRow, Row backRow, Unit unit)
        {
            List<Unit> targets = (from slot in frontRow.GuardSlots where slot.GetComponentInChildren<Unit>() != null 
                select slot.GetComponentInChildren<Unit>()).ToList();
            
            targets.AddRange(from slot in backRow.GuardSlots where slot.GetComponentInChildren<Unit>() != null 
                select slot.GetComponentInChildren<Unit>());

            int currentTarget = Random.Range(0, targets.Count);

            if (targets.Count > 0)
                return targets[currentTarget];
            else if (unit.CurrentEnemy != null)
                return unit.CurrentEnemy;

            return null;
        }
    }
}