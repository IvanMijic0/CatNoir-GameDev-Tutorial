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
            var transform1 = transform;
            transform1.position += transform1.right * (speed * Time.deltaTime);
            StartCoroutine(ProjectileDie());
        }

        private void OnCollisionEnter2D()
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


