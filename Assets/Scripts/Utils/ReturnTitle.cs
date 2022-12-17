using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReturnTitle : MonoBehaviour
{
    [SerializeField] private float titleDelay = 4f;
    
    private void Start()
    {
        StartCoroutine(ReturnToTitle());
    }

    private  IEnumerator ReturnToTitle()
    {
        yield return new WaitForSeconds(titleDelay);
        SceneManager.LoadScene(0);
    }
}
