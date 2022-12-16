using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    [SerializeField] private GameObject cam;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Player")){
            cam.transform.Translate(Vector2.up * 20f);
            cam.transform.Translate(Vector2.right * 25f);
            Destroy(gameObject);
        }
    }
}
