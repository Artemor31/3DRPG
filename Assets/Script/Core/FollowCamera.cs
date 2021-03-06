using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace RPG.Core
{
    public class FollowCamera : MonoBehaviour
    {
        [FormerlySerializedAs("_target")] public Transform target;

        private void Update()
        {
            transform.position = target.position;
        }
    }
}
