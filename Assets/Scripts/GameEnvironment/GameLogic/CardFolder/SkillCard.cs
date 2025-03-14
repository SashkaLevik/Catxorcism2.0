using System.Collections.Generic;
using Data;
using GameEnvironment.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GameEnvironment.GameLogic.CardFolder
{
    public class SkillCard : Card
    {
        [SerializeField] private SkillType _skillType;
        [SerializeField] private TMP_Text _valueAmount;
        //[SerializeField] private Image _skillIcon;
        //[SerializeField] private SkillData _skillData;
        [SerializeField] protected int _appliedValue;
        [SerializeField] protected int _requiredAP;
        [SerializeField] private List<Image> _APImages;

        //public SkillData SkillData => _skillData;
        protected BattleHud _battleHud;

        public int AppliedValue => _appliedValue;

        public int RequiredAP => _requiredAP;

        public SkillType Type => _skillType;

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
        
        public void Init(SkillData skillData)
        {
            //_skillData = skillData;
            //_skillIcon.sprite = _skillData.SkillIcon;
            //_appliedValue = _skillData.AppliedValue;
            _valueAmount.text = _appliedValue.ToString();

            for (int i = 0; i < skillData.RequiredAP; i++) 
                _APImages[i].gameObject.SetActive(true);
        }

        public virtual void UseSkill(){}
        
        public virtual void UseOnGuard(Guard guard){}

        public virtual void UseOnEnemy(EnemyGuard enemyGuard){}

        public override void Activate()
        {
            base.Activate();
            _collider.enabled = true;
        }

        public override void Disactivate()
        {
            base.Disactivate();
            _collider.enabled = false;
        }
    }
}