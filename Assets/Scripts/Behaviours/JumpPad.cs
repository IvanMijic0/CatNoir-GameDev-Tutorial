using UnityEngine;
using Audio;

namespace Behaviours
{
    public class JumpPad : MonoBehaviour
    {
        private AudioManager _audioManager;
        public float bounce = 45f;

        public void Awake()
        {
            _audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.gameObject.CompareTag("Player")) return;
            other.gameObject.GetComponent<Rigidbody2D>().AddForce(Vector2.up * bounce, ForceMode2D.Impulse);
            _audioManager.PlaySound(3);
        }
    }
}

