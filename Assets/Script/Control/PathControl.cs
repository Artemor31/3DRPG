using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Control
{
    public class PathControl : MonoBehaviour
    {
        private void OnDrawGizmos()
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                int j = GetNextWaypointIndex(i);

                Gizmos.DrawSphere(GetCurrentWaypoint(i), 0.5f);

                Gizmos.DrawLine(transform.GetChild(i).position, transform.GetChild(j).position);
            }
        }

        public Vector3 GetCurrentWaypoint(int i)
        {
            return transform.GetChild(i).position;
        }

        public int GetNextWaypointIndex(int i)
        {
            int j = i < (transform.childCount - 1) ? i + 1 : 0;
            return j;
        }
    }
}
