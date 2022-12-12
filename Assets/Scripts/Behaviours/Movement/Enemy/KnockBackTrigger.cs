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

        public void Awake()
        {
            audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            var player = other.collider.GetComponent<PlatformMovement>();

            if (player != null)
            {
                playerHealth.hitPoints--;
                player.KnockBack(transform);
                Debug.Log(playerHealth.hitPoints);
                audioManager.PlaySound(2);
            }
        }
    }
}