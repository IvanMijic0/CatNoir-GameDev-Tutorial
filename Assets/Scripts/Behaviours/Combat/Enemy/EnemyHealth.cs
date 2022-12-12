using UnityEngine;
using Audio;
using Behaviours.Combat.Enemy;

namespace Behaviours.Combat.Enemy
{
    public class EnemyHealth : MonoBehaviour
    {
        [SerializeField] private int hitPoints = 2;
        private AudioManager audioManager;

        public void Awake()
        {
            audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        }

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

