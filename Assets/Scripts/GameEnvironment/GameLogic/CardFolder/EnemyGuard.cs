using System.Collections.Generic;
using GameEnvironment.GameLogic.CardFolder.SkillCards;
using GameEnvironment.UI;
using TMPro;
using UnityEngine;

namespace GameEnvironment.GameLogic.CardFolder
{
    public class EnemyGuard : Unit
    {
        [SerializeField] private EnemySkillView _skillView;
        [SerializeField] private TMP_Text _valueAmount;
        [SerializeField] private List<SkillCard> _skills;

        //private int _slotIndex;
        private Player _player;
        private SkillCard _currentSkill;

        public Player Player => _player;

        protected override void Start()
        {
            base.Start();
            PrepareSkill();
        }

        public void InitPlayer(Player player) => 
            _player = player;

        public void TryGetEnemy(BattleHud battleHud)
        {
            if (GetEnemy(battleHud.PlayerFrontRow)){}
            else if (GetEnemy(battleHud.PlayerBackRow)){}
            else
                _currentEnemy = _player;
        }

        public void UsePreparedSkill()
        {
            _currentSkill.UseSkill(this);
            _skillView.HideSkill();
        }
        
        public void PrepareSkill()
        {
            int randomSkill = Random.Range(0, _skills.Count);
            _currentSkill = _skills[randomSkill];
            _skillView.InitSkill(_currentSkill);
            _valueAmount.text = _skills[randomSkill].AppliedValue.ToString();

            if (_currentSkill.Type == SkillType.Attack) 
                _currentDamage = _currentSkill.AppliedValue;
        }
    }
}