using UnityEngine.SceneManagement;
using UnityEngine;
using System.Collections;
using Behaviours.Movement.PlayerMovement;


namespace Behaviours.Combat.Player
{
    public class PlayerHealth : MonoBehaviour
    {
       
        public int hitPoints = 3;
        public Transform catSprite;
        private PlayerController _playerController;
        
        [SerializeField] private float delay = .5f;

        void Awake()
        {
            catSprite = GameObject.Find("CatSprite").GetComponent<Transform>();
            _playerController = GetComponent<PlayerController>();
        }
   

        public void Defeat(Animator animator, PlayerMovement playerMovement, PlayerProjectileFire projectileFire, Rigidbody2D rigidBody2D)
        {
            if (hitPoints == 0)
            {
                catSprite.transform.position = new Vector3(transform.position.x, transform.position.y - 0.2f, 0f);
                playerMovement.enabled = false;
                projectileFire.enabled = false;
                _playerController.enabled = false;
                rigidBody2D.velocity = Vector2.zero;
                animator.Play("Defeated");
                StartCoroutine(LoadLevel(delay));
            }
        }

        static IEnumerator LoadLevel(float delay)
        {
            yield return new WaitForSeconds(delay);
            SceneManager.LoadScene(2);
        }
    }
}
