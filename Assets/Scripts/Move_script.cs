using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Move_script : MonoBehaviour
{
    
    private Camera cam;
    private Dropdown dropdown;

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        dropdown = GetComponent<Dropdown>();
        Camera_script cam_script = cam.GetComponent<Camera_script>();
        dropdown.onValueChanged.AddListener((int value) => {
            cam_script.SetMove(value);
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
