using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AI;
using RPG.Control;
using RPG.Saving;
using UnityEngine.Serialization;

namespace RPG.SceneManagment
{
    public class Portal : MonoBehaviour
    {
        private enum TransitionDestination
        {
            North,
            South,
            West,
            East
        }
        
        [FormerlySerializedAs("_loadingScene")] [SerializeField] private int loadingScene = -1;
        [FormerlySerializedAs("_spawnPoint")] [SerializeField] public Transform spawnPoint;
        [SerializeField] private TransitionDestination transportDestination = TransitionDestination.North;
        [FormerlySerializedAs("FadeOutTime")] [SerializeField] private float fadeOutTime = 1f;
        [FormerlySerializedAs("FadeInTime")] [SerializeField] private float fadeInTime = 1f;


        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                StartCoroutine(Teleport());
            }
        }

        private IEnumerator Teleport()
        {
            DontDestroyOnLoad(gameObject);

            Fader fader = FindObjectOfType<Fader>();
            yield return fader.FadeOut(fadeOutTime);


            SavingWrapper wrapper = FindObjectOfType<SavingWrapper>();
            wrapper.Save();
            yield return SceneManager.LoadSceneAsync(loadingScene);
            wrapper.Load();


            GameObject player = GameObject.FindWithTag("Player");
            player.GetComponent<PlayerController>().enabled = false;


            Portal otherPortal = GetOtherPortal();
            UpdatePlayerPosition(player, otherPortal);
            wrapper.Save();

            yield return new WaitForSeconds(0.5f);
            yield return fader.FadeIn(fadeInTime);

            player.GetComponent<PlayerController>().enabled = true;

            Destroy(gameObject);
        }

        private void UpdatePlayerPosition(GameObject player, Portal otherPortal)
        {
            if (otherPortal != null)
            {
                player.GetComponent<NavMeshAgent>().Warp(otherPortal.spawnPoint.position);
                player.transform.rotation = otherPortal.spawnPoint.rotation;
            }
        }

        private Portal GetOtherPortal()
        {
            foreach (Portal portal in FindObjectsOfType<Portal>())
            {
                if (portal == this) continue;
                if (portal.transportDestination != transportDestination) continue;
                
                return portal;  
            }
            return null;
        }
    }
}
