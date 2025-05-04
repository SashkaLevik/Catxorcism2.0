using System;
using GameEnvironment.GameLogic.CardFolder;
using GameEnvironment.GameLogic.RowFolder;
using UnityEngine;

namespace GameEnvironment.GameLogic.PlayerSkills
{
    public class RowProtection : PlayerSkill
    {
        [SerializeField] private int _appliedValue;
        
        private Row _row;

        protected override void OnSkillButton()
        {
            base.OnSkillButton();
            _battleHud.PlayerFrontRow.Activate();
            _battleHud.PlayerBackRow.Activate();
        }

        private void Update()
        {
            if (_isSkillActive)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    Vector3 _worldPosition = _camera.ScreenToWorldPoint(Input.mousePosition);
                    RaycastHit2D[] hitInfo = Physics2D.RaycastAll(_worldPosition, Vector2.zero);

                    foreach (RaycastHit2D hit2D in hitInfo)
                    {
                        if (hit2D.collider.TryGetComponent(out Guard guard))
                            UseSkill(guard);
                        else
                        {
                            _isSkillActive = false;
                            _battleHud.PlayerFrontRow.Disactivate();
                            _battleHud.PlayerBackRow.Disactivate();
                        }
                    }
                    _isSkillActive = false;
                    _battleHud.PlayerFrontRow.Disactivate();
                    _battleHud.PlayerBackRow.Disactivate();
                }
            }
        }

        private void UseSkill(Guard guard)
        {
            foreach (var slot in guard.UnitRow.GuardSlots)
            {
                if (slot.GetComponentInChildren<Guard>() != null)
                {
                    slot.GetComponentInChildren<Guard>().Health.RiseDefence(_appliedValue);
                }
            }
            
            _battleHud.PlayerFrontRow.Disactivate();
            _battleHud.PlayerBackRow.Disactivate();
            _isSkillActive = false;
            _skillButton.interactable = false;
        }
    }
}