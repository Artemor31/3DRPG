using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AI;
using RPG.Control;

namespace RPG.SceneManagment
{
    public class Portal : MonoBehaviour
    {
        enum TransitionDestination
        {
            North,
            South,
            West,
            East
        }
        
        [SerializeField] int _loadingScene = -1;
        [SerializeField] public Transform _spawnPoint;
        [SerializeField] TransitionDestination transportDestination = TransitionDestination.North;
        [SerializeField] float FadeOutTime = 1f;
        [SerializeField] float FadeInTime = 1f;


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

            yield return fader.FadeOut(FadeOutTime);
            yield return SceneManager.LoadSceneAsync(_loadingScene);

            GameObject player = GameObject.FindWithTag("Player");
            player.GetComponent<PlayerController>().enabled = false;

            Portal otherPortal = GetOtherPortal();
            UpdatePlayerPosition(player, otherPortal);

            yield return new WaitForSeconds(0.5f);
            yield return fader.FadeIn(FadeInTime);

            player.GetComponent<PlayerController>().enabled = true;

            Destroy(gameObject);
        }

        private void UpdatePlayerPosition(GameObject player, Portal otherPortal)
        {
            if (otherPortal != null)
            {
                player.GetComponent<NavMeshAgent>().Warp(otherPortal._spawnPoint.position);
                player.transform.rotation = otherPortal._spawnPoint.rotation;
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
