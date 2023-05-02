using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Dropdown_element_script : MonoBehaviour
{
    public Planet planet;
    private Element_script script;

    void Start()
    {
        this.script = GetComponentInParent<Element_script>();
    }

    public void OnClickItem()
    {
        this.script.OnClick(planet);
    }

    void Update()
    {
        
    }
}
