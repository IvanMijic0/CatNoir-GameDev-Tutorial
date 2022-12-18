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
       
        public int hitPoints = 3;
        public Transform catSprite;
        private PlayerController _playerController;
        
        [SerializeField] private float delay = .5f;
        [SerializeField] private List<WaypointMovement> enemyMovement;

        void Awake()
        {
            catSprite = GameObject.Find("CatSprite").GetComponent<Transform>();
            _playerController = GetComponent<PlayerController>();
        }
   

        public void Defeat(Animator animator, PlayerMovement playerMovement, PlayerProjectileFire projectileFire, Rigidbody2D rigidBody2D)
        {
            if (hitPoints != 0) return;

            foreach (var enemy in enemyMovement.Where(enemy => enemy != null))
            {
                enemy.enabled = false;
            }
            
            catSprite.transform.position = new Vector3(transform.position.x, transform.position.y - 0.2f, 0f);
            playerMovement.enabled = false;
            projectileFire.enabled = false;
            _playerController.enabled = false;
            rigidBody2D.velocity = Vector2.zero;
            animator.Play("Defeated");
            StartCoroutine(LoadLevel(delay));
        }

        private static IEnumerator LoadLevel(float delay)
        {
            yield return new WaitForSeconds(delay);
            SceneManager.LoadScene(2);
        }
    }
}
