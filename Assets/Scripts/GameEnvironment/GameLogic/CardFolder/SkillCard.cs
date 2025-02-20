using Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GameEnvironment.GameLogic.CardFolder
{
    public class SkillCard : Card
    {
        [SerializeField] private TMP_Text _valueAmount;
        [SerializeField] private Image _skillIcon;
        [SerializeField] private SkillData _skillData;
        [SerializeField] private int _appliedValue;

        public void Init(SkillData skillData)
        {
            _skillData = skillData;
            _skillIcon.sprite = _skillData.SkillIcon;
            _appliedValue = _skillData.AppliedValue;
            _valueAmount.text = _appliedValue.ToString();
        }
    }
}