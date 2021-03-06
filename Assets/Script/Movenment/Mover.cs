using UnityEngine;
using UnityEngine.AI;
using RPG.Core;
using RPG.Saving;
using RPG.Resources;
using UnityEngine.Serialization;

namespace RPG.Movement
{
    public class Mover : MonoBehaviour, IAction, ISaveable
    {
        [FormerlySerializedAs("_maxSpeed")] [SerializeField] private float maxSpeed = 6f;
        private NavMeshAgent _agent;
        private Health _health;

        private void Start()
        {
            _agent = GetComponent<NavMeshAgent>();
            _health = GetComponent<Health>();
        }

        private void Update()
        {
            _agent.enabled = !_health.Dead();
            UpdateAnimator();
        }
        public void StartMove(Vector3 destination, float fraction)
        {
            GetComponent<ActionScheduler>().StartAction(this);
            MoveTo(destination, fraction);
        }
        public void MoveTo(Vector3 destination, float fraction)
        {
            _agent.destination = destination;
            _agent.speed = maxSpeed * Mathf.Clamp01(fraction);
            _agent.isStopped = false;
        }
 
        public void Cancel()
        {
            _agent.isStopped = true;
        }

        private void UpdateAnimator()
        {
            Vector3 velocity = _agent.velocity;
            Vector3 localVelocity = transform.InverseTransformDirection(velocity);
            float speed = localVelocity.z;
            GetComponent<Animator>().SetFloat("ForwardSpeed", speed);
        }

        public object CaptureState()
        {
            return new SerializableVector3(transform.position);
        }

        public void RestoreState(object state)
        {
            SerializableVector3 position = state as SerializableVector3;
            GetComponent<NavMeshAgent>().enabled = false;
            transform.position = position.ToVector();
            GetComponent<NavMeshAgent>().enabled = true;
        }
    }
}
