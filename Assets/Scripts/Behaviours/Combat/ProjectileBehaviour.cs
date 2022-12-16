using UnityEngine;

namespace Behaviours.Combat
{
    public class ProjectileBehaviour : MonoBehaviour
    {
        [Header("Projectile Characteristics")] 
        [SerializeField] private float speed = 1.5f;

        private void FixedUpdate()
        {
            transform.position += transform.right * speed * Time.deltaTime;
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            Destroy(gameObject);
        }
    }
}


