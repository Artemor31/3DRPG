using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using RPG.Combat;
using RPG.Core;
using RPG.Movement;

namespace RPG.Control
{
    public class AICotroller : MonoBehaviour
    {
        [SerializeField] float _chaseDistance = 5f;
        [SerializeField] float _suspiciousTime = 5f;

        GameObject _player;
        Fighter _fighter;
        Health _health;
        Mover _mover;
        Vector3 _startPosition;
        float _timeSincelastPlayerSaw = Mathf.Infinity;


        private void Start()
        {
            _mover = GetComponent<Mover>();
            _startPosition = transform.position;
            _player = GameObject.FindGameObjectWithTag("Player");
            _fighter = GetComponent<Fighter>();
            _health = GetComponent<Health>();
        }
        private void Update()
        {

            if (_health.Dead())
            {
                return;
            }
            if (DistanceToPlayer() && _fighter.CanAttack(_player))
            {
                _timeSincelastPlayerSaw = 0;
                AggroBehaviour();
            }
            else if(_timeSincelastPlayerSaw < _suspiciousTime)
            {
                SuspiciousBehaviour();
            }
            else
            {
                GuardBehaviour();
            }


            _timeSincelastPlayerSaw += Time.deltaTime;
        }

        private void GuardBehaviour()
        {
            _mover.StartMove(_startPosition);
        }

        private void AggroBehaviour()
        {
            _fighter.Attack(_player);
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.Lerp(Color.red, Color.green, 0.3f);
            Gizmos.DrawWireSphere(transform.position, _chaseDistance);
        }

        void SuspiciousBehaviour()
        {
            GetComponent<ActionScheduler>().CancelCurrentAction();

        }
        private bool DistanceToPlayer()
        {
            return Vector3.Distance(_player.transform.position, transform.position) < _chaseDistance;
        }
    }
}
