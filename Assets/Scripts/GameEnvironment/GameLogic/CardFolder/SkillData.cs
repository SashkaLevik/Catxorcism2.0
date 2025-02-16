using UnityEngine;

namespace GameEnvironment.GameLogic.CardFolder
{
    [CreateAssetMenu(fileName = "SkillCard", menuName = "SkillCard")]
    public class SkillData : ScriptableObject
    {
        public SkillType SkillType;
        public Sprite SkillIcon;
        public int AppliedValue;
    }
}