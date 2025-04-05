using GameEnvironment.GameLogic.CardFolder;
using UnityEngine;

namespace Data
{
    [CreateAssetMenu(fileName = "SkillData", menuName = "SkillData")]
    public class SkillData : ScriptableObject
    {
        //public SkillCardType SkillCardType;
        public SkillType SkillType;
        public Sprite SkillIcon;
        public int AppliedValue;
        public int RequiredAP;
        public Sprite _skillIcon;
    }
}