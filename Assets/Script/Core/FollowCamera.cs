using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Core
{
    public class FollowCamera : MonoBehaviour
    {
        public Transform _target;

        void Update()
        {
            transform.position = _target.position;
        }
    }
}
