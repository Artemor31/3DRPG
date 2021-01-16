using RPG.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Combat
{
    [CreateAssetMenu(fileName = "Weapon", menuName = "Weapons/New Weapon", order = 0)]
    public class Weapon : ScriptableObject
    {
        const string WEAPON_NAME = "PickedWeapon";

        [SerializeField] float _damage = 10f;
        [SerializeField] float _range = 2f;
        [SerializeField] float _attackDelay = 1f;
        [SerializeField] bool isRightHanded = true;

        [SerializeField] GameObject _weaponPrefab = null;
        [SerializeField] AnimatorOverrideController _overridedAnimator = null;
        [SerializeField] Projectile _projectile = null;


        public void Spawn(Transform rightHand, Transform lefthand, Animator animator)
        {
            DestrouOldWeapon(rightHand, lefthand);

            if (_weaponPrefab != null)
            {
                GameObject weapon = Instantiate(_weaponPrefab, GetSpawnTransform(rightHand, lefthand));
                weapon.name = WEAPON_NAME;
            }
            if (_overridedAnimator != null)
                animator.runtimeAnimatorController = _overridedAnimator;
        }

        void DestrouOldWeapon(Transform rightHand, Transform leftHand)
        {
            Transform oldWeapon = rightHand.Find(WEAPON_NAME);
            if (oldWeapon == null)
            {
                oldWeapon = leftHand.Find(WEAPON_NAME);
            }
            if (oldWeapon == null) return;
            oldWeapon.name = "destroy";
            Destroy(oldWeapon.gameObject);
        }

        private Transform GetSpawnTransform(Transform rightHand, Transform lefthand)
        {
            Transform hand;
            if (isRightHanded) hand = rightHand;
            else hand = lefthand;
            return hand;
        }

        public void CreateProjectile(Transform rightHand, Transform lefthand, Health target)
        {
            Projectile projectile = Instantiate(_projectile, GetSpawnTransform(rightHand, lefthand).position, Quaternion.identity);
            projectile.SetTarget(target, _damage);
        }

        public float GetDamage()
        {
            return _damage;
        }
        public float GetRange()
        {
            return _range;
        }
        public float GetDelay()
        {
            return _attackDelay;
        }
        public bool hasProjectile()
        {
            return _projectile != null;
        }
    }
}
