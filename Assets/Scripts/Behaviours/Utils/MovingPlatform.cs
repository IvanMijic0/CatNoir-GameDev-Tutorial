using Interfaces;
using UnityEngine;

namespace Behaviours.Movement
{
    public class MovingPlatform : MonoBehaviour, IMove, ICollision
    {
        [SerializeField] private Transform[] waypoints;
        [SerializeField] private float speed;
        [SerializeField] private float checkDistance = .05f;

        private Transform _targetWaypoint;
        private int _currentWaypointIndex;

        private void Start()
        {
            _targetWaypoint = waypoints[0];
        }

        public void MoveToWaypoint()
        {
            transform.position = Vector2.MoveTowards
            (
                transform.position,
                _targetWaypoint.position,
                speed * Time.deltaTime
            );

            if (Vector2.Distance(transform.position, _targetWaypoint.position) < checkDistance){
                _targetWaypoint = GetNextWaypoint();
            }
        }

        public Transform GetNextWaypoint()
        {
            _currentWaypointIndex++;
            if (_currentWaypointIndex >= waypoints.Length){
                _currentWaypointIndex = 0;
            }
            return waypoints[_currentWaypointIndex];
        }

        public void OnDrawGizmos()
        {
            Gizmos.DrawLine(waypoints[0].position, waypoints[1].position);
        }

        public void OnCollisionEnter2D(Collision2D other)
        {
            var playerMovement = other.collider.GetComponent<PlayerController>();
            if (playerMovement != null){
                playerMovement.SetParent(transform);
            }
        }

        public void OnCollisionExit2D(Collision2D other)
        {
            var playerMovement = other.collider.GetComponent<PlayerController>();
            if (playerMovement != null){
                playerMovement.ResetParent();
            }
        }
        
       
    }
}

