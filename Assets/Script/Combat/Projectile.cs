using UnityEngine;
using RPG.Resources;
using UnityEngine.Serialization;

namespace RPG.Combat
{
    public class Projectile : MonoBehaviour
    {
        [FormerlySerializedAs("_speed")] [SerializeField] private float speed = 5f;
        [SerializeField] private bool isHomingMissle = false;
        [FormerlySerializedAs("_impactEffect")] [SerializeField] private GameObject impactEffect = null;
        [FormerlySerializedAs("_lifeTime")] [SerializeField] private float lifeTime = 6f;

        private Health _target = null;

        private float _damage = 0;
        private GameObject _shooter;

        private void Start()
        {
            if (!isHomingMissle) gameObject.transform.LookAt(GetAimPosition());
        }

        private void Update()
        {
            if (isHomingMissle && !_target.Dead()) gameObject.transform.LookAt(GetAimPosition());

            if (transform.position == _target.transform.position) return;

            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }

        private Vector3 GetAimPosition()
        {
            CapsuleCollider capsuleCollider = _target.GetComponent<CapsuleCollider>();

            if (capsuleCollider == null) 
                return _target.transform.position;

            return _target.transform.position + capsuleCollider.center;
        }

        public void SetTarget(GameObject shooter, Health target, float damage)
        {
            _target = target;
            _damage = damage;
            _shooter = shooter;
            Destroy(gameObject, lifeTime);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<Health>() != _target) return;

            if (_target.Dead()) return;
            
            other.GetComponent<Health>().TakeDamage(_shooter,_damage);

            if (impactEffect != null)
                Instantiate(impactEffect, GetAimPosition(), transform.rotation);

            Destroy(gameObject);
        }
    }
}
