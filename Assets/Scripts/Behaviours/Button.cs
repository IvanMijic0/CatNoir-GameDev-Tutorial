using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Audio;

namespace Behaviours
{
    public class Button : MonoBehaviour
    {
        [SerializeField] private GameObject jumpPad;
        private AudioManager audioManager;

        public void Start()
        {
            audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        }

        private void OnTriggerEnter2D(Collider2D other) 
        {
            if(other.gameObject.CompareTag("Projectile"))
            {
                gameObject.transform.Translate(Vector2.right * 0.2f);
                other.enabled = true;
                audioManager.PlaySound(4);
                
                
                jumpPad.transform.Translate(Vector2.up * 0.3f);

            }
        }
    }

}

