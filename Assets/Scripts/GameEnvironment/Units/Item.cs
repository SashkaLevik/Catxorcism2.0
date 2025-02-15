using GameEnvironment.GameLogic.CardFolder;
using UnityEngine;

namespace GameEnvironment.Units
{
    public class Item : Card
    {
        [SerializeField] protected int _minValue;
        [SerializeField] protected int _maxValue;
        [SerializeField] protected AudioSource _clickSound;

        public AudioSource Sound => _clickSound;        
    }
}