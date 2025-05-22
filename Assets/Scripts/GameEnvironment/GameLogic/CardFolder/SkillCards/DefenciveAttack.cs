using GameEnvironment.Units;
using UnityEngine;

namespace GameEnvironment.GameLogic.CardFolder.SkillCards
{
    public class DefenciveAttack : SkillCard
    {
        public override void UseSkill(Unit unit)
        {
            if (unit.CurrentEnemy != null)
            {
                var target = unit.CurrentEnemy;
                unit.Attack(this, target, unit.CurrentDamage);
            }

            unit.OnDefence(Mathf.RoundToInt(_appliedValue / 2));
            
            if (unit.GetComponent<Guard>()) 
                unit.GetComponent<Guard>().OnSkillPlayed(this);
        }
    }
}