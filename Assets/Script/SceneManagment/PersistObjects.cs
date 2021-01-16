using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.SceneManagment
{
    public class PersistObjects : MonoBehaviour
    {
        [SerializeField] GameObject _persistObjects = null;

        static bool isSpawned =  false;

        private void Awake()
        {
            if (isSpawned) return;
            SpawnPersistObjects();
            isSpawned = true;

        }

        private void SpawnPersistObjects()
        {
            GameObject persistObjects = Instantiate(_persistObjects);
            DontDestroyOnLoad(persistObjects);
        }

        void Update()
        {

        }
    }
}
