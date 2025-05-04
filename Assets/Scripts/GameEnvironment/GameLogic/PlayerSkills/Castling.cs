using System;
using System.Collections;
using GameEnvironment.GameLogic.CardFolder;
using GameEnvironment.GameLogic.RowFolder;
using UnityEngine;
using UnityEngine.UI;

namespace GameEnvironment.GameLogic.PlayerSkills
{
    public class Castling : PlayerSkill
    {
        private float _moveSpeed = 30f;
        private bool _isReady;
        private Guard _firstSelectedGuard;
        private Guard _secondSelectedGuard;
        private Vector3 _worldPosition;
        private RowCardSlot _firstSlot;
        private RowCardSlot _secondSlot;
        private RectTransform _firstGuardPos;
        private RectTransform _secondGuardPos;
        private Row _firstGuardRow;
        private Row _secondGuardRow;

        private void Update()
        {
            if (_isSkillActive)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    _worldPosition = _camera.ScreenToWorldPoint(Input.mousePosition);
                    RaycastHit2D hit = Physics2D.Raycast(_worldPosition, Vector2.zero, Single.PositiveInfinity);

                    if (hit.collider != null && hit.transform.TryGetComponent(out Guard guard))
                    {
                        if (guard.IsOnField && _firstSelectedGuard == null)
                        {
                            _firstSelectedGuard = guard;
                            _firstSlot = _firstSelectedGuard.GetComponentInParent<RowCardSlot>();
                            _firstGuardRow = _firstSlot.GetComponentInParent<Row>();
                            _firstGuardPos = _firstSlot.GetComponent<RectTransform>();
                            _firstSelectedGuard.GetComponent<Image>().color = Color.green;
                        }
                        else if (guard.IsOnField && _firstSelectedGuard != null)
                        {
                            _secondSelectedGuard = guard;
                            _secondSlot = _secondSelectedGuard.GetComponentInParent<RowCardSlot>();
                            _secondGuardRow = _secondSlot.GetComponentInParent<Row>();
                            _secondGuardPos = _secondSlot.GetComponent<RectTransform>();
                            _secondSelectedGuard.GetComponent<Image>().color = Color.green;
                            _isReady = true;
                        }
                        else
                            Disactivate();
                    }
                    else
                        Disactivate();
                }

                if (_isReady)
                {
                    _isReady = false;
                    StartCoroutine(UseSkill());
                }
            }
        }

        private IEnumerator UseSkill()
        {
            yield return new WaitForSeconds(0.2f);
            /*StartCoroutine(MoveCard(_firstSelectedGuard, _secondGuardPos));
            yield return StartCoroutine(MoveCard(_secondSelectedGuard, _firstGuardPos));

            int firstGuardIndex = _firstGuardRow.GuardSlots.IndexOf(_firstSlot);
            int secondGuardIndex = _secondGuardRow.GuardSlots.IndexOf(_secondSlot);
            _firstSelectedGuard.InitRow(_secondGuardRow, secondGuardIndex);
            _secondSelectedGuard.InitRow(_firstGuardRow, firstGuardIndex);
            _firstSelectedGuard.TryGetEnemy(_battleHud);
            _secondSelectedGuard.TryGetEnemy(_battleHud);
            _firstSelectedGuard = null;
            _secondSelectedGuard = null;
            _isSkillActive = false;
            _skillButton.interactable = false;*/
        }
        
        private IEnumerator MoveCard(Guard guard, RectTransform newPos)
        {            
            while (guard.transform.position != newPos.transform.position)
            {
                guard.transform.position = Vector3.MoveTowards(guard.transform.position, newPos.transform.position,
                    _moveSpeed * Time.deltaTime);
                yield return null;
            }
            guard.transform.SetParent(newPos);
            yield return new WaitForSeconds(0.2f);
            guard.GetComponent<Image>().color = Color.white;
        }
        
        private void Disactivate()
        {
            _isSkillActive = false;
            _secondSelectedGuard = null;
            
            if (_firstSelectedGuard != null)
            {
                _firstSelectedGuard.GetComponent<Image>().color = Color.white;
                _firstSelectedGuard = null;
            }
        }
    }
}