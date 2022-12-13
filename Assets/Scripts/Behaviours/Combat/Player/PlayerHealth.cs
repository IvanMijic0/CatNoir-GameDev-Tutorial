using UnityEngine.SceneManagement;
using UnityEngine;
using System.Collections;
using Behaviours.Movement.PlayerMovement;

namespace Behaviours.Combat.Player
{
    public class PlayerHealth : MonoBehaviour
    {
       
        public int hitPoints = 3;
        [SerializeField] private float delay = 5f;

        public void Defeat(Animator animator, PlayerMovement playerMovement, PlayerProjectileFire projectileFire)
        {
            if (hitPoints == 0)
            {
                playerMovement.enabled = false;
                projectileFire.enabled = false;
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
