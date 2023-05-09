using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollbarPanel_Script : MonoBehaviour
{
    private Scrollbar scrollbar;
    private ScrollRect scrollRect;
    // Start is called before the first frame update
    void Start()
    {
        scrollbar = GetComponentInChildren<Scrollbar>();
        scrollRect = GetComponent<ScrollRect>();
        scrollRect.horizontal = false;
        scrollRect.onValueChanged.AddListener((value) =>
        {
            Debug.Log("Scrollbar scroll : " + value);
            scrollbar.onValueChanged.Invoke(Mathf.Clamp01(value.y));
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
