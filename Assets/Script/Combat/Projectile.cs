using RPG.Core;
using UnityEngine;

namespace RPG.Combat
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] float _speed = 5f;
        [SerializeField] bool isHomingMissle;

        Health _target = null;

        float _damage = 0;

        private void Start()
        {
            if (!isHomingMissle) gameObject.transform.LookAt(GetAimPosition());
        }

        void Update()
        {
            if (isHomingMissle && !_target.Dead()) gameObject.transform.LookAt(GetAimPosition());

            if (transform.position == _target.transform.position) return;

            transform.Translate(Vector3.forward * _speed * Time.deltaTime);
        }

        Vector3 GetAimPosition()
        {
            CapsuleCollider capsuleCollider = _target.GetComponent<CapsuleCollider>();

            if (capsuleCollider == null) 
                return _target.transform.position;

            return _target.transform.position + capsuleCollider.center;
        }

        public void SetTarget(Health target, float damage)
        {
            _target = target;
            _damage = damage;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<Health>() != _target) return;

            if (_target.Dead()) return;
            
            other.GetComponent<Health>().TakeDamage(_damage);
            Destroy(gameObject);
        }
    }
}
