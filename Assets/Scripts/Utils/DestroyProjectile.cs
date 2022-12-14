using UnityEngine;

public class DestroyProjectile : MonoBehaviour
{
    private const string Projectile = "Projectile";

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag(Projectile))
        {
            Destroy(collision.gameObject);
        }
    }
}
