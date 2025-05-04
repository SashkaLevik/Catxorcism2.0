using GameEnvironment.GameLogic.RowFolder;
using GameEnvironment.Units;
using UnityEngine;

namespace GameEnvironment.GameLogic.CardFolder.SkillCards
{
    public class StrikeThrough : SkillCard
    {
        private Unit _currentUnit;
        
        public override void UseSkill(Unit unit)
        {
            if (unit.GetComponent<Guard>())
            {
                _currentUnit = unit.GetComponent<Guard>();
                Unit firstTarget = null;
                Unit secondTarget = null;

                if (GetTarget(_battleHud.EnemyFrontRow) != null && GetTarget(_battleHud.EnemyBackRow) != null)
                {
                    firstTarget = GetTarget(_battleHud.EnemyFrontRow);
                    secondTarget = GetTarget(_battleHud.EnemyBackRow);
                }
                else if (GetTarget(_battleHud.EnemyFrontRow) != null && GetTarget(_battleHud.EnemyBackRow) == null)
                {
                    firstTarget = GetTarget(_battleHud.EnemyFrontRow);
                    secondTarget = unit.GetComponent<Guard>().Enemy;
                }
                else if (GetTarget(_battleHud.EnemyFrontRow) == null && GetTarget(_battleHud.EnemyBackRow) != null)
                {
                    firstTarget = GetTarget(_battleHud.EnemyBackRow);
                    secondTarget = unit.GetComponent<Guard>().Enemy;
                }
                else if (GetTarget(_battleHud.EnemyFrontRow) == null && GetTarget(_battleHud.EnemyBackRow) == null)
                {
                    firstTarget = unit.GetComponent<Guard>().Enemy;
                }
                
                if (firstTarget != null) 
                    firstTarget.Health.TakeDamage(unit.CurrentDamage);

                if (secondTarget != null) 
                    secondTarget.Health.TakeDamage(unit.CurrentDamage / 2);
            }
            else if (unit.GetComponent<EnemyGuard>())
            {
                _currentUnit = unit.GetComponent<EnemyGuard>();
                Unit firstTarget = null;
                Unit secondTarget = null;

                if (GetTarget(_battleHud.PlayerFrontRow) != null && GetTarget(_battleHud.PlayerBackRow) != null)
                {
                    firstTarget = GetTarget(_battleHud.PlayerFrontRow);
                    secondTarget = GetTarget(_battleHud.PlayerBackRow);
                }
                else if (GetTarget(_battleHud.PlayerFrontRow) != null && GetTarget(_battleHud.PlayerBackRow) == null)
                {
                    firstTarget = GetTarget(_battleHud.PlayerFrontRow);
                    secondTarget = unit.GetComponent<EnemyGuard>().Player;
                }
                else if (GetTarget(_battleHud.PlayerFrontRow) == null && GetTarget(_battleHud.PlayerBackRow) != null)
                {
                    firstTarget = GetTarget(_battleHud.PlayerBackRow);
                    secondTarget = unit.GetComponent<EnemyGuard>().Player;
                }
                else if (GetTarget(_battleHud.PlayerFrontRow) == null && GetTarget(_battleHud.PlayerBackRow) == null)
                {
                    firstTarget = unit.GetComponent<EnemyGuard>().Player;
                }
                
                if (firstTarget != null) 
                    firstTarget.Health.TakeDamage(unit.CurrentDamage);

                if (secondTarget != null) 
                    secondTarget.Health.TakeDamage(unit.CurrentDamage / 2);
            }
            
            
            if (unit.GetComponent<Guard>()) 
                unit.GetComponent<Guard>().OnSkillPlayed(this);
        }

        private Unit GetTarget(Row row)
        {
            if (row.GuardSlots[_currentUnit.SlotIndex].GetComponentInChildren<Unit>() != null)
            {
                var target = row.GuardSlots[_currentUnit.SlotIndex].GetComponentInChildren<Unit>();
                
                return target;
            }

            return null;
        }
    }
}