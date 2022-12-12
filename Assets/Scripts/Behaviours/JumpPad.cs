using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Audio;

namespace Behaviours
{
    public class JumpPad : MonoBehaviour
    {
        private AudioManager audioManager;
        private float bounce = 45f;

        public void Start()
        {
            audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        }

        private void OnTriggerEnter2D(Collider2D other) 
        {
            if(other.gameObject.CompareTag("Player"))
            {
                other.gameObject.GetComponent<Rigidbody2D>().AddForce(Vector2.up * bounce, ForceMode2D.Impulse);
                audioManager.PlaySound(3);
            }
        }
    }
}

