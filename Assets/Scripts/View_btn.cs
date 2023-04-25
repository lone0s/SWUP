using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class View_btn : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Button deleteButton = this.transform.Find("Close_btn").GetComponent<Button>();
        deleteButton.onClick.AddListener(() => Destroy(gameObject));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
