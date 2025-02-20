using System;
using System.Collections.Generic;
using GameEnvironment.GameLogic.CardFolder;
using UnityEngine;

namespace GameEnvironment.UI
{
    public class ActionPointsViewer : MonoBehaviour
    {
        [SerializeField] private GameObject[] _fullImages;
        [SerializeField] private GameObject[] _emptyImages;

        public void UpdateAP(int requiredAP)
        {
            for (int i = 0; i < _emptyImages.Length; i++)
                _emptyImages[i].gameObject.SetActive(false);

            for (int i = 0; i < _fullImages.Length; i++)
                _fullImages[i].gameObject.SetActive(false);

            for (int i = 0; i < requiredAP; i++)
                _fullImages[i].gameObject.SetActive(true);
        }
    }
}