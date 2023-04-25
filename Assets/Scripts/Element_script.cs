using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using System.Xml;

public class Element_script : MonoBehaviour
{
    public GameObject prefab;


    void Start()
    {
        InitElementPanel(); 
    }

    void Update()
    {
        
    }

    public void InitElementPanel(){

        Main_script scriptParent = GetComponentInParent<Main_script>();
        Dictionary<string, Planet> d = scriptParent.planets;

        foreach (KeyValuePair<string, Planet> planet in d)
        {
            AddElement(planet.Key);
        }
    }

    public void AddElement(string name){
        GameObject element = Instantiate(prefab, transform.position, Quaternion.identity);

        Text elementText = element.GetComponentInChildren<Text>();
        elementText.text = name;

        element.transform.parent = transform;
    }
}
