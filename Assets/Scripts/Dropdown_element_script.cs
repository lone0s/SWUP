using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Dropdown_element_script : MonoBehaviour
{
    private Menus_script script;
    public Func<string, string> function;

    void Start()
    {
        this.script = GetComponentInParent<Menus_script>();
        this.function = script.function;
    }

    public void OnClickItem()
    {
        string objName = gameObject.GetComponent<Text>().text;
        if(function != null && objName != null){
            this.function.Invoke(objName);
        }
        
    }

    void Update()
    {
        
    }
}
