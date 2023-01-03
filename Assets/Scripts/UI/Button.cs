using UnityEngine;
using Audio;

namespace Behaviours
{
    public class Button : MonoBehaviour
    {
        [SerializeField] private AudioManager audioManager;
        [SerializeField] private Sprite buttonPressed;
        [SerializeField] private Collider2D collider2D;

        private SpriteRenderer _spriteRenderer;

        public void Awake()
        {
            audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.gameObject.CompareTag("Projectile")) return;
            gameObject.GetComponent<Collider2D>().isTrigger = false;
            _spriteRenderer.sprite = buttonPressed;
            audioManager.PlaySound(4);
            collider2D.enabled = true;
        }
    }

}

