using Assets.Scripts.GameEnvironment.UI;
using Assets.Scripts.GameEnvironment.Units;
using System;
using UnityEngine;

public class MouseTrakcing : MonoBehaviour
{
    [SerializeField] private BattleHud _battleHud;
    [SerializeField] private LayerMask _layerMask;
    [SerializeField] private Enemy _enemy;

    private Camera _camera;
    private Player _player;
    private Health _playerHealth;
    private PlayerMoney _playerMoney;
    private float _animationDelay = 0.4f;

    private void Start()
    {
        _camera = Camera.main;
        _player = _battleHud.Player;
        _playerMoney = _battleHud.PlayerMoney;
        _playerHealth = _player.GetComponent<Health>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 clickPosition = _camera.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(clickPosition, Vector2.zero, Single.PositiveInfinity, _layerMask);

            if (hit.collider != null)
            {
                if (hit.transform.TryGetComponent(out Enemy enemy))
                {
                    _enemy = hit.collider.GetComponent<Enemy>();
                    if (_player != null) _player.Attack(_enemy);                   
                }
                else if (hit.transform.TryGetComponent(out Coin coin))
                {
                    _playerMoney.AddCoin(coin.Amount, _battleHud.Coins);
                    coin.Sound.Play();
                    Destroy(coin.gameObject, _animationDelay);
                }
                else if (hit.transform.TryGetComponent(out Crystal crystal))
                {
                    _playerMoney.AddCrystal(crystal.Amount, _battleHud.Crystals);
                    crystal.Sound.Play();
                    Destroy(crystal.gameObject, _animationDelay);
                }
                else if (hit.transform.TryGetComponent(out HealPotion potion))
                {
                    if (_player.Type == PlayerType.Barbarian)
                        _playerHealth.HealBarbarian(potion.Amount);
                    else
                        _playerHealth.Heal(potion.Amount);

                    potion.Sound.Play();
                    Destroy(potion.gameObject, _animationDelay);
                }
                else if (hit.transform.TryGetComponent(out Shield shield))
                {
                    if (_player.Type == PlayerType.Knight)
                        _playerHealth.RiseDefence(shield.Amount);
                    else if (_player.Type == PlayerType.Mage)
                        _player.RiseDamage(shield.Amount);

                    shield.Sound.Play();
                    Destroy(shield.gameObject, _animationDelay);
                }

                _battleHud.DecreaseAP();
            }
        }                
    }
}
