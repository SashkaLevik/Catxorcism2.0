using System;
using GameEnvironment.GameLogic.CardFolder;
using UnityEngine;

namespace GameEnvironment.GameLogic.PlayerSkills
{
    public class WarCry : PlayerSkill
    {
        private void Update()
        {
            if (_isSkillActive)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    Vector3 _worldPosition = _camera.ScreenToWorldPoint(Input.mousePosition);
                    RaycastHit2D hit = Physics2D.Raycast(_worldPosition, Vector2.zero, Single.PositiveInfinity);
                
                    if (hit.collider != null && hit.collider.TryGetComponent(out Guard guard))
                    {
                        UseSkill(guard);
                    }
                    else
                        _isSkillActive = false;
                }
            }
        }

        private void UseSkill(Guard guard)
        {
            guard.ResetAP();
            _isSkillActive = false;
            _skillButton.interactable = false;
        }
    }
}