using System.Collections;
using UnityEngine;

namespace Assets.Scripts.GameEnvironment.Units
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] private float _speed;        
        [SerializeField] private int _damage;
        [SerializeField] private Card _target;

        public void Init(Card target, int damage)
        {
            _target = target;
            _damage = damage;          
        }

        public void Shoot() =>
            StartCoroutine(Move());

        private IEnumerator Move()
        {
            if (_target != null)
            {
                while (transform.position != _target.transform.position)
                {
                    transform.position = Vector3.MoveTowards(transform.position, _target.transform.position, _speed * Time.deltaTime);
                    yield return null;
                }

                _target.GetComponent<Health>().TakeDamage(_damage);
                Destroy(gameObject);
            }
            else
                Destroy(gameObject);
        }
    }
}