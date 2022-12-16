using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Audio;

namespace Behaviours
{
    public class Button : MonoBehaviour
    {
        [SerializeField] private GameObject jumpPad;
        [SerializeField] private AudioManager audioManager;
        [SerializeField] private Sprite buttonPressed;

        private SpriteRenderer spriteRenderer;

        public void Awake()
        {
            audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
            spriteRenderer = GetComponent<SpriteRenderer>();
            //spriteRenderer.sprite = redButton;
        }

        private void OnTriggerEnter2D(Collider2D other) 
        {
            if(other.gameObject.CompareTag("Projectile"))
            {
                this.gameObject.GetComponent<Collider2D>().isTrigger = false;
                spriteRenderer.sprite = buttonPressed;
                audioManager.PlaySound(4);
                jumpPad.transform.Translate(Vector2.up * 0.3f);
            }
        }
    }

}

