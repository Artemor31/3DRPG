using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace RPG.SceneManagment
{
    public class PersistObjects : MonoBehaviour
    {
        [FormerlySerializedAs("_persistObjects")] [SerializeField] private GameObject persistObjects = null;

        private static bool _isSpawned =  false;

        private void Awake()
        {
            if (_isSpawned) return;
            SpawnPersistObjects();
            _isSpawned = true;

        }

        private void SpawnPersistObjects()
        {
            GameObject persistObjects = Instantiate(this.persistObjects);
            DontDestroyOnLoad(persistObjects);
        }
    }
}
