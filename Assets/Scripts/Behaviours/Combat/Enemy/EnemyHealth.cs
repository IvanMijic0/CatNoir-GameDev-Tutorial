using UnityEngine;
using Audio;
using UnityEngine.Serialization;

namespace Behaviours.Combat.Enemy
{
    public class EnemyHealth : MonoBehaviour
    {
        [SerializeField] private int hitPoints = 2;
        [SerializeField] AudioManager audioManager;

     

        private void OnCollisionEnter2D(Collision2D other)
        {
            if(other.gameObject.CompareTag("Projectile")){
                if (gameObject != null){
                    hitPoints--;
                    audioManager.PlaySound(2);
                }
            }
        }

        public void Update()
        {
            if(hitPoints == 0){
                Destroy(gameObject);            
            }
        }
    }
}

