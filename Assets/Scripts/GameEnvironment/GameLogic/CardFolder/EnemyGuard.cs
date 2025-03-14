using System.Collections.Generic;
using Data;
using GameEnvironment.GameLogic.RowFolder;
using UnityEngine;
using UnityEngine.UI;

namespace GameEnvironment.GameLogic.CardFolder
{
    public class EnemyGuard : Unit
    {
        [SerializeField] private Image _intention;
        [SerializeField] private SuitType _suit;
        [SerializeField] private List<SkillData> _skillDatas;

        private int _slotIndex;
        private Player _player;
        private Row _row;
        
        protected override void Start()
        {
            base.Start();
            //PrepareSkill();
        }

        public void InitPlayer(Player player) => 
            _player = player;
        
        public void InitRow(Row row, int index)
        {
            _row = row;
            _slotIndex = index;
        }

        private void PrepareSkill()
        {
            int randomSkill = Random.Range(0, _skillDatas.Count);

            _intention.sprite = _skillDatas[randomSkill]._skillIcon;
        }
    }
}