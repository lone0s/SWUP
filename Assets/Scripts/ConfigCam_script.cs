using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConfigCam_script : MonoBehaviour
{
    public GameObject Camera;
    public Slider speedSlider;

    // Start is called before the first frame update
    void Start()
    {
        speedSlider.onValueChanged.AddListener((float value) => { 
            Camera.GetComponentInChildren<Camera_script>().SetTransition(value); 
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
