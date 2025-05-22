using System.Collections.Generic;
using GameEnvironment.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GameEnvironment.GameLogic.CardFolder.SkillCards
{
    public class SkillCard : Card
    {
        [SerializeField] protected TMP_Text _valueAmount;
        [SerializeField] protected int _appliedValue;
        [SerializeField] protected int _requiredAP;
        [SerializeField] private SkillType _skillType;
        [SerializeField] private SkillEffectType _effectType;
        [SerializeField] private GameObject _arrow;
        [SerializeField] private Sprite _skillIcon;
        [SerializeField] private Sprite _effectIcon;
        [SerializeField] private List<Image> _APImages;

        protected Guard _playerGuard;
        protected BattleHud _battleHud;

        public int AppliedValue => _appliedValue;

        public int RequiredAP => _requiredAP;

        public SkillType Type => _skillType;

        public Sprite SkillIcon => _skillIcon;

        public Sprite EffectIcon => _effectIcon;

        public SkillEffectType EffectType => _effectType;

        protected override void Start()
        {
            base.Start();
            _valueAmount.text = _appliedValue.ToString();

            for (int i = 0; i < _requiredAP; i++) 
                _APImages[i].gameObject.SetActive(true);
        }

        public void InitHud(BattleHud battleHud)
        {
            _battleHud = battleHud;
        }

        public void ShowArrow() => 
            _arrow.SetActive(true);

        public void HideArrow() => 
            _arrow.SetActive(false);

        public virtual void UseSkill(Unit unit){}
    }
}