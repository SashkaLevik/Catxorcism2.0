using System;
using UnityEngine;
using UnityEngine.UI;

namespace GameEnvironment.UI
{
    public class BaseWindow : MonoBehaviour
    {
        public Button CloseButton;

        protected virtual void Awake() => 
            CloseButton.onClick.AddListener(()=>gameObject.SetActive(false));
        
    }
}