using System.Collections;
using UnityEngine;

namespace Assets.Scripts.GameEnvironment.Units
{
    public class AttackSystem : MonoBehaviour
    {
        [SerializeField] protected AudioSource _attackSound;
        [SerializeField] private float _moveSpeed;
        [SerializeField] protected Projectile _projectile;

        public void Attack(Unit player, Unit enemy)
        {
            if (player.AttackType == AttackType.Melee)
                StartCoroutine(MeleeAttack(player, enemy));
            else if (player.AttackType == AttackType.Range)
            {
                StartCoroutine(RangeAttack(player, enemy));                
            }            
        }

        private IEnumerator RangeAttack(Unit player, Unit enemy)
        {
            yield return new WaitForSeconds(0.4f);

            if (enemy != null)
            {
                Projectile projectile = Instantiate(_projectile, player.FirePos.transform.position, Quaternion.identity, player.FirePos);
                projectile.Init(enemy, player.Damage);
                projectile.Shoot();
                _attackSound.Play();

                if (player.GetComponent<Player>())
                    player.GetComponent<Player>().ReduceDamage();
            }            
        }

        private IEnumerator MeleeAttack(Unit player, Unit enemy)
        {
            yield return new WaitForSeconds(0.4f);

            if (enemy != null)
            {
                _attackSound.Play();
                yield return StartCoroutine(Move(enemy.transform.position));
                enemy.GetComponent<Health>().TakeDamage(player.Damage);
                player.GetComponent<Health>().TakeDamage(enemy.Damage);
                yield return StartCoroutine(Move(player.StartPosition));
            }
        }

        private IEnumerator Move(Vector3 newPos)
        {
            while (transform.position != newPos)
            {
                transform.position = Vector3.MoveTowards(transform.position, newPos, _moveSpeed * Time.deltaTime);
                yield return null;
            }
        }
    }
}