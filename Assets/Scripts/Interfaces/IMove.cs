using System;
using UnityEngine;

namespace Interfaces
{
    public interface IMove
    {
        public Transform GetNextWaypoint();
        public void MoveToWaypoint();
        public void OnDrawGizmos();
    }
}