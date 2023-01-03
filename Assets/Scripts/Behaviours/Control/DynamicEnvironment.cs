using Behaviours.Movement;
using UnityEngine;

namespace Behaviours.Control
{
    public class DynamicEnvironment: MonoBehaviour
    {
        private MovingPlatform _movingPlatform;

        private void Awake()
        {
            _movingPlatform = GetComponent<MovingPlatform>();
        }

        private void Update()
        {
            _movingPlatform.MoveToWaypoint();
        }
    }
}