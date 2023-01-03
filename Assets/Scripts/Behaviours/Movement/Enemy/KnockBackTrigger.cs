using Behaviours.Combat.Player;
using UnityEngine;
using Audio;

namespace Behaviours.Movement.Enemy
{
    public class KnockBackTrigger : MonoBehaviour
    {
        [SerializeField] private PlayerHealth playerHealth;
        [SerializeField] private AudioManager audioManager;

        private void Awake()
        {
            audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
            playerHealth = GameObject.Find("Player").GetComponent<PlayerHealth>();
        }
        
        private void OnCollisionEnter2D(Collision2D other)
        {
            var player = other.collider.GetComponent<PlayerController>();

            if (player == null) return;
            playerHealth.hitPoints--;
            player.KnockBack(transform);
            audioManager.PlaySound(2);
        }
    }
}