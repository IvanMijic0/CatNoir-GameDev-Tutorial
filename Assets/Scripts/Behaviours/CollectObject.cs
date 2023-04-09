using Unity.VisualScripting;
using UnityEngine;

public class CollectObject : MonoBehaviour
{
    [SerializeField] private Transform uiObject;

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (!col.CompareTag("Player")) return;
        uiObject.gameObject.SetActive(true);
        Destroy(gameObject);
    }
}
