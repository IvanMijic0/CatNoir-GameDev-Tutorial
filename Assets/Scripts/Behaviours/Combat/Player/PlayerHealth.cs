using UnityEngine.SceneManagement;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Behaviours.Movement.Enemy;
using Behaviours.Movement.PlayerMovement;

namespace Behaviours.Combat.Player
{
    public class PlayerHealth : MonoBehaviour
    {
        private PlayerController _playerController;
        public int hitPoints = 3;
        public Transform catSprite;
        public HealthBar healthBar;
        
        [SerializeField] private float delay = .5f;
        [SerializeField] private List<EnemyMovement> enemyMovements;
        
        private static readonly int Defeated = Animator.StringToHash("defeated");

        private void Awake()
        {
            catSprite = GameObject.Find("CatSprite").GetComponent<Transform>();
            healthBar = GameObject.FindGameObjectWithTag("HealthBar").GetComponent<HealthBar>();
            healthBar.SetMaxHitPoints(hitPoints);
            _playerController = GetComponent<PlayerController>();
        }
        
        public void Defeat(Animator animator, PlayerMovement playerMovement, PlayerProjectileFire projectileFire, Rigidbody2D rigidBody2D)
        {
            if (hitPoints != 0) return;

            foreach (var enemyMovement in enemyMovements.Where(enemyMovement => enemyMovement != null))
            {
                enemyMovement.enabled = false;
            }

            var position = transform.position;
            catSprite.transform.position = new Vector3(position.x, position.y - 0.2f, 0f);
            playerMovement.enabled = false;
            projectileFire.enabled = false;
            _playerController.enabled = false;
            rigidBody2D.velocity = Vector2.zero;
            animator.SetTrigger(Defeated);
            StartCoroutine(LoadLevel(delay));
        }

        private static IEnumerator LoadLevel(float delay)
        {
            yield return new WaitForSeconds(delay);
            SceneManager.LoadScene(2);
        }
    }
}
