using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MultipleAttributesWithIcon_Script : MonoBehaviour
{
    public string text;
    public string[] attributeNames;
    public int textSize;
    public string iconPath;
    private ImagedText_Script nestedScript;


    Transform panel;
    RectTransform panelTransform;

    public void Awake()
    {
        initialize();
        panel = transform;
        panelTransform = panel.GetComponent<RectTransform>();
    }

    public void initialize()
    {
        nestedScript = transform.GetChild(0).GetComponent<ImagedText_Script>();
        nestedScript.text = text;
        nestedScript.iconPath = iconPath;
    }

    void Start()
    {
        injectStringsIntoTextFields();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void injectStringsIntoTextFields()
    {
        for (int i = 0; i < attributeNames.Length; ++i)
        {
            GameObject tempInputField = new GameObject(attributeNames[i] + " Input");
            Text tempTextComponent = tempInputField.AddComponent<Text>();
            tempTextComponent.text = attributeNames[i];
            tempTextComponent.font = Font.CreateDynamicFontFromOSFont("Arial", 14);
            tempTextComponent.fontSize = textSize;
            tempTextComponent.color = Color.black;
            tempTextComponent.transform.SetParent(panel, false);
            tempTextComponent.alignment = TextAnchor.MiddleLeft;
        }
    }
}
