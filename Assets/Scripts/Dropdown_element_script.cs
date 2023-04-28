using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dropdown_element_script : MonoBehaviour
{
    public Dropdown dropdown;

    void Start()
    {
        dropdown.onValueChanged.AddListener(OnDropdownValueChanged);
    }

    void Update()
    {
        
    }

    void OnDropdownValueChanged(int index)
    {
        dropdown.value = 0;
    }

    
}
