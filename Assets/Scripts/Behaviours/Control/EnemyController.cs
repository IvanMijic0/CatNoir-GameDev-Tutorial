using Behaviours.Movement.Enemy;
using UnityEngine;

namespace Behaviours.Control
{
    public class EnemyController: MonoBehaviour
    {
        private EnemyMovement _enemyMovement;

        private void Awake()
        {
            _enemyMovement = GetComponent<EnemyMovement>();
        }
        
        private void Update()
        {
            _enemyMovement.MoveToWaypoint();
            _enemyMovement.FlipEnemy();
        }
    }
}