using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace GameEnvironment.UI
{
    public class BaseWindow : MonoBehaviour
    {
        public Button CloseButton;

        public event UnityAction Closed;

        protected virtual void Awake() =>
            CloseButton.onClick.AddListener(OnCloseButton);

        private void OnCloseButton()
        {
            Closed?.Invoke();
            gameObject.SetActive(false);
        }
    }
}