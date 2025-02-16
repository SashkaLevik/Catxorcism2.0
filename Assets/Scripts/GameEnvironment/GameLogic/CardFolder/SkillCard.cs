using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GameEnvironment.GameLogic.CardFolder
{
    public class SkillCard : Card
    {
        [SerializeField] private SkillData _skillData;
        [SerializeField] private Image _skillIcon;
        [SerializeField] private int _appliedValue;
        [SerializeField] private TMP_Text _valueAmount;

        
        public void Init(SkillData skillData)
        {
            _skillData = skillData;
            _skillIcon.sprite = _skillData.SkillIcon;
            _appliedValue = _skillData.AppliedValue;
            _valueAmount.text = _appliedValue.ToString();
        }
    }
}