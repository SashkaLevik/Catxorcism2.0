using Assets.Scripts.GameEnvironment.Units;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.GameEnvironment.GameLogic
{
    public class ObjectPool : MonoBehaviour
    {
        private const string FirstWaveDeck = "Player/1LevelParts";

        private List<Card> _firstWaveDeck = new List<Card>();

        private void Awake()
        {
            _firstWaveDeck = Resources.LoadAll<Card>(FirstWaveDeck).ToList();
        }
    }
}