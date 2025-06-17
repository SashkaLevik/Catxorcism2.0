using System;
using GameEnvironment.GameLogic.CardFolder;
using GameEnvironment.UI;
using UnityEngine;
using UnityEngine.UI;

namespace GameEnvironment.GameLogic.PlayerSkills
{
    public class PlayerSkill : MonoBehaviour
    {
        [SerializeField] private GameObject _skillDescription;
        
        protected Player _player;
        protected Camera _camera;
        protected bool _isSkillActive;
        protected BattleHud _battleHud;
        protected DeckCreator _deckCreator;
        protected Button _skillButton;

        public GameObject SkillDescription => _skillDescription;

        private void Start()
        {
            _camera = Camera.main;
            _player = GetComponentInParent<Player>();
            _skillButton = GetComponent<Button>();
            _skillButton.onClick.AddListener(OnSkillButton);
        }

        private void OnDestroy() => 
            _skillButton.onClick.RemoveListener(OnSkillButton);

        public void Construct(BattleHud battleHud, DeckCreator deckCreator)
        {
            _battleHud = battleHud;
            _deckCreator = deckCreator;
        }

        public void ResetSkill() => 
            _skillButton.interactable = true;

        protected virtual void OnSkillButton()
        {
            _isSkillActive = true;
        }
    }
}