using System.Collections;
using System.Collections.Generic;
using GameEnvironment.GameLogic.CardFolder;
using GameEnvironment.Units;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace GameEnvironment.GameLogic.SkillEffects
{
    public class EffectsReceiver : MonoBehaviour
    {
        [SerializeField] protected List<SkillEffectView> _receivedEffects;
        [SerializeField] protected Image _resist;

        private int _effectPercent;
        private float _animationDelay = 0.8f;
        private IHealth _health;
        private Unit _unit;

        private void Start()
        {
            _unit = GetComponent<Unit>();
        }

        public void TryApplyEffect(SkillCard skill)
        {
            _effectPercent = Random.Range(0, 11);
            
            if (skill.EffectType != SkillEffectType.NoEffect)
                StartCoroutine(ReceiveEffect(skill));
        }

        private IEnumerator ReceiveEffect(SkillCard skill)
        {
            yield return new WaitForSeconds(_animationDelay);

            Receive(skill);

            /*if (_effectPercent <= skill.EffectChance)
                Receive(skill);
            else
                StartCoroutine(ShowResist());  */                      
        }

        private IEnumerator ShowResist()
        {
            _resist.gameObject.SetActive(true);
            yield return new WaitForSeconds(0.7f);
            _resist.gameObject.SetActive(false);
        }

        public void ApplyReceivedEffect()
        {
            StartCoroutine(CheckEffects());
        }

        private IEnumerator CheckEffects()
        {
            foreach (var effect in _receivedEffects)
            {
                if (effect.Skill != null)
                {
                    effect.ApplyOnTurn(_unit);
                    yield return new WaitForSeconds(0.2f);
                }
            }
        }
        
        private void Receive(SkillCard skill)
        {
            foreach (var effect in _receivedEffects)
            {
                if (effect.Skill != null && effect.Skill == skill)
                {
                    effect.Stack(skill);
                    return;
                }                    
                else if (effect.Skill == null)
                {
                    effect.InitSkill(skill, _unit);
                    return;
                }
            }
        }
    }
}