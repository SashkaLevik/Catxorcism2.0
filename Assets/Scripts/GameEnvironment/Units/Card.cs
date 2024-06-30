using Assets.Scripts.Data;
using System;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.GameEnvironment.Units
{
    public class Card : MonoBehaviour
    {               
        protected Vector3 _startPosition;
        protected BoxCollider2D _collider;
        
        public Vector3 StartPosition => _startPosition;

        private void Awake() =>
            _collider = GetComponent<BoxCollider2D>();

        protected virtual void Start()
        {            
        }        

        public void Activate()=>
            _collider.enabled = true;

        public void Disactivate() =>
            _collider.enabled = false;
            
    }
}