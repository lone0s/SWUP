using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConfigCam_script : MonoBehaviour
{
    private Camera cam;
    private Camera_script cam_script;
    public Slider speedSlider;
    private Button reset;

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        cam_script = cam.GetComponentInChildren<Camera_script>();
        speedSlider.onValueChanged.AddListener((float value) => {
            cam_script.SetTransition(value); 
        });

        reset = transform.Find("Reset_btn").GetComponent<Button>();
        reset.onClick.AddListener(() => cam_script.Reset());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
