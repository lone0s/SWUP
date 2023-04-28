using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Dropdown_element_script : MonoBehaviour
{
    public Planet planet;

    void Update()
    {
        
    }

    void Start()
    {
        Dropdown myGameObject = GetComponent<Dropdown>();;

        // Do something with the GameObject
        Debug.Log(myGameObject);

        myGameObject.onValueChanged.AddListener(OnDropdownValueChanged);
    }

    public void OnDropdownValueChanged(int index)
    {
        Debug.Log("Selected index: " + index);
        Debug.Log(planet);
    }

    
}
