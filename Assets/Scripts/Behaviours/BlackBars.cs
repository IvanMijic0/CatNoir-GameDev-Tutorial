using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlackBars : MonoBehaviour
{
    public RectTransform barUp;
    public RectTransform barDown;
    
    void Awake()
    {
        barUp = GetComponent<RectTransform>();
        barDown = GetComponent<RectTransform>();
    }

   
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.B)){
            barUp.localPosition = new Vector3(0, 20, 0);
            barDown.localPosition = new Vector3(0, 0, 0);
            Debug.Log("B");
        }
    }
}
