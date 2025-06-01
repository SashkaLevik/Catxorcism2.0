using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Data;
using GameEnvironment.UI.PlayerWallet;
using Infrastructure.Services;
using TMPro;
using UnityEngine;

namespace GameEnvironment.UI
{
    public class Academy : BaseWindow, ISaveProgress
    {
        [SerializeField] private PlayerMoney _playerMoney;
        [SerializeField] private TMP_Text _materialsAmount;
        [SerializeField] private RectTransform _materialsPos;
        [SerializeField] private RectTransform _upPos;
        [SerializeField] private GameObject _materialPrefab;

        private float _time;
        private Vector3 _path;
        private float _speed = 3f;
        private float _speedModifier = 2.5f;
        private float _currentSpeed;

        private int _playersLeadership;
        private int _playersHandCapacity;
        private List<CardData> _availableGuards = new List<CardData>();
        private List<string> _restoredBuildings = new List<string>();

        public PlayerMoney PlayerMoney => _playerMoney;

        public TMP_Text MaterialsAmount => _materialsAmount;

        public List<CardData> AvailableGuards => _availableGuards;

        public List<string> RestoredBuildings => _restoredBuildings;

        private void Start()
        {
            _materialsAmount.text = _playerMoney.Materials.ToString();
        }

        public void AddHiredGuards(List<CardData> guardDatas, string buildName)
        {
            _availableGuards.AddRange(guardDatas);
            _restoredBuildings.Add(buildName);
        }

        public void IncreaseHandCapacity() => 
            _playersHandCapacity++;

        public void IncreaseLeadership() => 
            _playersLeadership++;

        public void OnConstruct(int amount, RectTransform newPos)
        {
            StartCoroutine(MoveMaterials(amount, newPos));
        }
        
        private IEnumerator MoveMaterials(int amount, RectTransform newPos)
        {
            _currentSpeed = _speed;
            
            for (int i = 0; i < amount; i++)
            {
                GameObject materialPrefab = Instantiate(_materialPrefab, _materialsPos);
                Vector3 startPos = materialPrefab.transform.position;
                Vector3 upPos = _upPos.transform.position;
                Vector3 endPos = newPos.transform.position;

                while (materialPrefab.transform.position != newPos.transform.position)
                {
                    _currentSpeed += Time.deltaTime * _speedModifier;
                    _time += Time.deltaTime * _currentSpeed;
                    _path = GetPoint(startPos, upPos, endPos, _time);
                    materialPrefab.transform.position = _path;
                    yield return null;
                }

                _time = 0;
                Destroy(materialPrefab, 0.3f);
            }
        }

        private Vector3 GetPoint(Vector3 pos1, Vector3 pos2, Vector3 pos3, float t)
        {
            Vector3 firstPos = Vector3.Lerp(pos1, pos2, t);
            Vector3 secondPos = Vector3.Lerp(pos2, pos3, t);
            Vector3 vector = Vector3.Lerp(firstPos, secondPos, t);
            return vector;
        }
        
        public void Load(PlayerProgress progress)
        {
            _availableGuards = progress.WorldData.AvailableGuards.ToList();
            _restoredBuildings = progress.WorldData.RestoredBuildings.ToList();
            _playersLeadership = progress.PlayerStats.Leadership;
            _playersHandCapacity = progress.PlayerStats.HandCapacity;
        }

        public void Save(PlayerProgress progress)
        {
            progress.WorldData.AvailableGuards = _availableGuards.ToList();
            progress.WorldData.RestoredBuildings = _restoredBuildings.ToList();
            progress.PlayerStats.Leadership = _playersLeadership;
            progress.PlayerStats.HandCapacity = _playersHandCapacity;
        }
    }
}