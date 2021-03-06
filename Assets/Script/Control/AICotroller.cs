using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using RPG.Combat;
using RPG.Core;
using RPG.Movement;
using System;
using RPG.Resources;
using UnityEngine.Serialization;

namespace RPG.Control
{
    public class AICotroller : MonoBehaviour
    {
        [FormerlySerializedAs("_chaseDistance")] [SerializeField] private float chaseDistance = 5f;
        [FormerlySerializedAs("_suspiciousTime")] [SerializeField] private float suspiciousTime = 5f;
        [FormerlySerializedAs("_waypointStaytime")] [SerializeField] private float waypointStaytime = 2f;
        [FormerlySerializedAs("_patrolSpeedFraction")]
        [Range(0,1)]
        [SerializeField]
        private float patrolSpeedFraction = 0.2f;
        [FormerlySerializedAs("_pathControl")] [SerializeField] private PathControl pathControl = null;

        private int _waypointIndex = 0;
        private float _distanceToWaypoint = 0.5f;


        private GameObject _player;
        private Fighter _fighter;
        private Health _health;
        private Mover _mover;
        private Vector3 _startPosition;
        private float _timeSincelastPlayerSaw = Mathf.Infinity;
        private float _timeSinceLastWaypointArrived = Mathf.Infinity;


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
            else if(_timeSincelastPlayerSaw < suspiciousTime)
            {
                SuspiciousBehaviour();
            }
            else
            {
                PatrolBehaviour();
            }

            _timeSinceLastWaypointArrived += Time.deltaTime;
            _timeSincelastPlayerSaw += Time.deltaTime;
        }

        private void PatrolBehaviour()
        {
            Vector3 nextPosition = _startPosition;
            if (pathControl != null)
            {
                if(AtWaypoint())
                {
                    CycleWaypoint();
                }
                nextPosition = GetNextWaypoint();
            }

            if (_timeSinceLastWaypointArrived < waypointStaytime) return;

            _timeSinceLastWaypointArrived = 0;
            _mover.StartMove(nextPosition, patrolSpeedFraction);
        }

        private Vector3 GetNextWaypoint()
        {
            return pathControl.GetCurrentWaypoint(_waypointIndex);
        }

        private void CycleWaypoint()
        {
            _waypointIndex = pathControl.GetNextWaypointIndex(_waypointIndex);
        }

        private bool AtWaypoint()
        {
            return Vector3.Distance(transform.position, GetNextWaypoint()) < _distanceToWaypoint;
        }

        private void AggroBehaviour()
        {
            _fighter.Attack(_player);
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.Lerp(Color.red, Color.green, 0.3f);
            Gizmos.DrawWireSphere(transform.position, chaseDistance);
        }

        private void SuspiciousBehaviour()
        {
            GetComponent<ActionScheduler>().CancelCurrentAction();

        }
        private bool DistanceToPlayer()
        {
            return Vector3.Distance(_player.transform.position, transform.position) < chaseDistance;
        }
    }
}
