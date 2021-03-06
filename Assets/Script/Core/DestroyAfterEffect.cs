using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Core
{
    public class DestroyAfterEffect : MonoBehaviour
    {
        private ParticleSystem _particleSystem;
        [SerializeField] private GameObject parentToDestroy = null;

        private void Start()
        {
            _particleSystem = GetComponent<ParticleSystem>();
        }

        private void Update()
        {
            if (!_particleSystem.IsAlive())
            {
                if (parentToDestroy != null)
                    Destroy(parentToDestroy);
                else
                    Destroy(gameObject);
            }
        }
    }
}
