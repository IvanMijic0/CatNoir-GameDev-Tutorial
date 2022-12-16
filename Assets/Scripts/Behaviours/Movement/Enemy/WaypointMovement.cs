using Interfaces;
using UnityEngine;

namespace Behaviours.Movement.Enemy
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(BoxCollider2D))]
    
    public class WaypointMovement : MonoBehaviour,  IMove
    {
        [SerializeField] private Transform[] waypoints;
        [SerializeField] private float speed;
        [SerializeField] private float checkDistance = .05f;
        
        private Transform _targetWaypoint;
        private int _currentWaypointIndex ;

   
        private void Start()
        {
            _targetWaypoint = waypoints[0];
        }

        private void Update()
        {
            MoveToWaypoint();
            FlipEnemy();
        }

        private void FlipEnemy()
        {
            if (transform.position.x <= waypoints[0].position.x + checkDistance)
            {
                transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
            }
            else if (transform.position.x >= waypoints[1].position.x - checkDistance)
            {
                transform.rotation = Quaternion.Euler(new Vector3(0, 180, 0));
            }
        }

        public Transform GetNextWaypoint()
        {
            _currentWaypointIndex++;
            if (_currentWaypointIndex >= waypoints.Length)
            {
                _currentWaypointIndex = 0;
            }

            return waypoints[_currentWaypointIndex];
        }

        public void MoveToWaypoint()
        {
            transform.position = Vector2.MoveTowards
            (
                transform.position,
                _targetWaypoint.position,
                speed * Time.deltaTime
            );

            if (Vector2.Distance(transform.position, _targetWaypoint.position) < checkDistance)
            {
                _targetWaypoint = GetNextWaypoint();
            }
        }

        public void OnDrawGizmos()
        {
            Gizmos.DrawLine(waypoints[0].position, waypoints[1].position);
        }
    }
}