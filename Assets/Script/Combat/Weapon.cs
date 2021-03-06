using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Resources;
using UnityEngine.Serialization;

namespace RPG.Combat
{
    [CreateAssetMenu(fileName = "Weapon", menuName = "Weapons/New Weapon", order = 0)]
    public class Weapon : ScriptableObject
    {
        private const string WeaponName = "PickedWeapon";

        [SerializeField] private float damage = 10f;
        [SerializeField] private float range = 2f;
        [SerializeField] private float attackDelay = 1f;
        [SerializeField] private bool isRightHanded = true;

        [SerializeField] private GameObject weaponPrefab = null;
        [SerializeField] private AnimatorOverrideController overridedAnimator = null;
        [SerializeField] private Projectile projectile = null;


        public void Spawn(Transform rightHand, Transform lefthand, Animator animator)
        {
            DestrouOldWeapon(rightHand, lefthand);

            if (weaponPrefab != null)
            {
                GameObject weapon = Instantiate(weaponPrefab, GetSpawnTransform(rightHand, lefthand));
                weapon.name = WeaponName;
            }
            if (overridedAnimator != null)
                animator.runtimeAnimatorController = overridedAnimator;
        }

        private void DestrouOldWeapon(Transform rightHand, Transform leftHand)
        {
            Transform oldWeapon = rightHand.Find(WeaponName);
            if (oldWeapon == null)
            {
                oldWeapon = leftHand.Find(WeaponName);
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

        public void CreateProjectile(Transform rightHand, Transform lefthand, Health target, GameObject shooter, float damageBonus)
        {
            Projectile projectile = Instantiate(this.projectile, GetSpawnTransform(rightHand, lefthand).position, Quaternion.identity);
            projectile.SetTarget(shooter, target, damage * damageBonus);
        }

        public float GetDamage()
        {
            return damage;
        }
        public float GetRange()
        {
            return range;
        }
        public float GetDelay()
        {
            return attackDelay;
        }
        public bool HasProjectile()
        {
            return projectile != null;
        }
    }
}
