using System;
using System.Collections;
using Behaviours.Movement;
using Enums;
using Audio;
using Unity.VisualScripting;
using UnityEngine;

namespace Behaviours.Combat.Player
{
    public class PlayerProjectileFire: MonoBehaviour
    {
        [Header("Projectile")]
        [SerializeField] private ProjectileBehaviour projectilePrefab;
        [SerializeField] private Transform launchOffset;
        [SerializeField] private PlatformMovement player;
        private AudioManager audioManager;

        public void Start()
        {
            audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        }

        private void Update()
        {
            if (Input.GetButtonDown("Fire1"))
            {
                if(audioManager.audioSource.enabled){
                    StartCoroutine(player.AttackSprite()); 
                    Instantiate(projectilePrefab, launchOffset.position, transform.rotation);
                    audioManager.PlaySound(1);
                }
            }
        }

     
    }
}