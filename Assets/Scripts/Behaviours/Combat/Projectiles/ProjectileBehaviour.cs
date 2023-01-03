using System.Collections;
using UnityEngine;

namespace Behaviours.Combat
{
    public class ProjectileBehaviour : MonoBehaviour
    {
        [Header("Projectile Characteristics")] 
        [SerializeField] private float speed = 1.5f;

        private const float DieTime = 2f;

        private void FixedUpdate()
        {
            transform.position += transform.right * speed * Time.deltaTime;
            StartCoroutine(ProjectileDie());
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            Destroy(transform.gameObject);
        }


        private IEnumerator ProjectileDie()
        {
            yield return new WaitForSeconds(DieTime);
            Destroy(gameObject);
        }
    }
}


