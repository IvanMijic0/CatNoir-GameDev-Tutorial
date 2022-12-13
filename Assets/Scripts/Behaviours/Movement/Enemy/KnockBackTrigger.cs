using System;
using System.Collections;
using Behaviours.Combat.Player;
using UnityEngine;
using Audio;

namespace Behaviours.Movement.Enemy
{
    public class KnockBackTrigger : MonoBehaviour
    {
        [SerializeField] private PlayerHealth playerHealth;
        [SerializeField] private AudioManager audioManager;
        
        private void OnCollisionEnter2D(Collision2D other)
        {
            var player = other.collider.GetComponent<PlayerController>();

            if (player == null) return;
            playerHealth.hitPoints--;
            player.KnockBack(transform);
            Debug.Log(playerHealth.hitPoints);
            audioManager.PlaySound(2);
        }
    }
}