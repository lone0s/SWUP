using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Categories_Script : MonoBehaviour
{
    public string[] categoryNames;
    public int fontSize;
    private Transform panel;

    void Start()
    {
        injectStringsIntoTextFields();
    }

    void injectStringsIntoTextFields()
    {
        panel = transform;
        for (int i = 0; i < categoryNames.Length; ++i)
        {
            GameObject tempInputField = new GameObject(categoryNames[i] + " Input");
            Text tempTextComponent = tempInputField.AddComponent<Text>();
            tempTextComponent.text = categoryNames[i];
            tempTextComponent.font = Font.CreateDynamicFontFromOSFont("Arial", 14);
            tempTextComponent.fontSize = fontSize;
            tempTextComponent.color = Color.black;
            tempTextComponent.transform.SetParent(panel, false);
        }
    }

    void Update()
    {
        
    }
}
