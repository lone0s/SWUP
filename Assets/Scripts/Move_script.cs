using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Move_script : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject Camera;
    void Start()
    {
        Dropdown dropdown = GetComponent<Dropdown>();
        Camera_script cam_script = Camera.GetComponent<Camera_script>();
        dropdown.onValueChanged.AddListener((int value) => {
            cam_script.SetMove(value);
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
