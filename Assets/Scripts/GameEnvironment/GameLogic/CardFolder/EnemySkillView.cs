using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GameEnvironment.GameLogic.CardFolder
{
    public class EnemySkillView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _descriptionText;
        [SerializeField] private TMP_Text _effectValue;
        [SerializeField] private Image _skillIcon;
        [SerializeField] private Image _descriptionImage;

        private SkillCard _skillCard;
        
        public void InitSkill(SkillCard skillCard)
        {
            _skillCard = skillCard;
            _skillIcon.gameObject.SetActive(true);
            _skillIcon.sprite = skillCard.SkillIcon;
            _effectValue.text = _skillCard.AppliedValue.ToString();
        }

        public void HideSkill() => 
            _skillIcon.gameObject.SetActive(false);

        private void OnMouseEnter()
        {
            _descriptionImage.gameObject.SetActive(true);
        }

        private void OnMouseExit()
        {
            _descriptionImage.gameObject.SetActive(false);
        }
    }
}