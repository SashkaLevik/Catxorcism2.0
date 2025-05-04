using Data;
using GameEnvironment.GameLogic.CardFolder;
using GameEnvironment.GameLogic.CardFolder.SkillCards;
using GameEnvironment.Units;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace GameEnvironment.GameLogic.SkillEffects
{
    public class SkillEffectView : MonoBehaviour
    {
        [SerializeField] private Image _descriptionImage;
        [SerializeField] private TMP_Text _description;
        [SerializeField] private TMP_Text _effectValue;
        [SerializeField] private Image _effectImage;

        private int _effectDuration;
        private SkillCard _skill;

        public int EffectDuration => _effectDuration;

        public SkillCard Skill => _skill;

        public void InitSkill(SkillCard skill, Unit unit)
        {
            _skill = skill;
            _effectImage.gameObject.SetActive(true);
            _effectImage.sprite = _skill.EffectIcon;
            _effectDuration = _skill.AppliedValue;
            UpdateDuration();
            switch (_skill.EffectType)
            {
                case SkillEffectType.Stun:
                    unit.IsStunned = true;
                    break;
                case SkillEffectType.Mark:
                    unit.IsMarked = true;
                    break;
                case SkillEffectType.Curse:
                    unit.IsCursed = true;
                    break;
            }
        }                  

        public void ApplyOnTurn(Unit unit)
        {
            if (unit.gameObject.activeSelf == true)
            {
                switch (_skill.EffectType)
                {
                    case SkillEffectType.Poison:
                    case SkillEffectType.Bleed:
                        unit.Health.TakeDirectDamage(_effectDuration);
                        ReduceDuration(unit);
                        break;
                    case SkillEffectType.Stun:
                    case SkillEffectType.Mark:
                    case SkillEffectType.Curse:
                        ReduceDuration(unit);
                        break;
                }
            }
        }

        private void ReduceDuration(Unit unit)
        {
            _effectDuration -= 1;
            UpdateDuration();

            if (_effectDuration <= 0)
                ResetEffect(unit);
        }

        public void Stack(SkillCard skill)
        {
            _effectDuration += skill.AppliedValue;
            UpdateDuration();
        }

        public void ResetEffect(Unit unit)
        {
            switch (_skill.EffectType)
            {
                case SkillEffectType.Stun:
                    unit.IsStunned = false;
                    break;
                case SkillEffectType.Mark:
                    unit.IsMarked = false;
                    break;
                case SkillEffectType.Curse:
                    unit.IsCursed = false;
                    break;
            }

            _effectImage.gameObject.SetActive(false);
            _skill = null;
            _effectDuration = 0;
        }

        public void OnEnter()
        {
            _descriptionImage.gameObject.SetActive(true);
            //_description.text = GetLocalizedDescription(_skillData);
        }

        public void OnExit()
            => _descriptionImage.gameObject.SetActive(false);

        private void UpdateDuration()
            => _effectValue.text = _effectDuration.ToString();

        /*private string GetLocalizedDescription(SkillData skillData)
        {
            if (Application.systemLanguage == SystemLanguage.Russian)
                return skillData.RuEffectDescription;
            else
                return skillData.EnEffectDescription;
        }*/
    }
}