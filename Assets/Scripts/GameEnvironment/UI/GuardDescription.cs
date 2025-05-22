using System.Collections.Generic;
using Data;
using GameEnvironment.GameLogic.CardFolder;
using GameEnvironment.GameLogic.CardFolder.SkillCards;
using UnityEngine;

namespace GameEnvironment.UI
{
    public class GuardDescription : MonoBehaviour
    {
        [SerializeField] private List<RectTransform> _skillPositions;
        [SerializeField] private GameObject _window;

        private Guard _selectedGuard;
        
        public void Show(Guard guard)
        {
            if (_selectedGuard != null) Clear();
            
            _selectedGuard = guard;
            _window.SetActive(true);

            for (int i = 0; i < guard.SkillCards.Count; i++)
            {
                Instantiate(guard.SkillCards[i], _skillPositions[i]);
            }
        }

        public void Hide()
        {
            Clear();
            _window.SetActive(false);
        }

        private void Clear()
        {
            foreach (var position in _skillPositions)
            {
                if (position.GetComponentInChildren<SkillCard>() != null)
                    Destroy(position.GetComponentInChildren<SkillCard>().gameObject);
            }
        }
    }
}