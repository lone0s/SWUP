using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class View_btn : MonoBehaviour
{
    private Button deleteButton;

    // Start is called before the first frame update
    void Start()
    {
        deleteButton = this.transform.Find("Close_btn").GetComponent<Button>();
        deleteButton.onClick.AddListener(() => Destroy(gameObject));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
