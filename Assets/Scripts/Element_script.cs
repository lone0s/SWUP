using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using System.Xml;

public class Element_script : MonoBehaviour
{
    public GameObject elementPrefab;
    public Dropdown dropdownPrefab;


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
            AddElement(planet.Value);
        }
    }

    public void AddElement(Planet p){
        if(p.satellite.Count > 0){
            Dropdown dropdown = Instantiate(dropdownPrefab, transform.position, Quaternion.identity);
            List<string> options = new List<string>();
            options.Add(p.name);
            foreach(KeyValuePair<string, Planet> s in p.satellite){
                options.Add(s.Key);
            }   
            dropdown.ClearOptions();
            dropdown.AddOptions(options);
            dropdown.transform.parent = transform;

            Text t = dropdown.GetComponent<Text>();
            t.text = "aaaa";
            Debug.Log(dropdown.GetComponent<Toggle>());

            //dropdown.onValueChanged.AddListener((index) => OnDropdownValueChanged(p,dropdown));

            //dropdown.GetComponent<Image>().GetComponentInChildren<Button>().onClick.AddListener(() => OnClick(p));
        }else{
            GameObject element = Instantiate(elementPrefab, transform.position, Quaternion.identity);

            Text elementText = element.GetComponentInChildren<Text>();
            elementText.text = p.name;
            element.transform.parent = transform;
        }

        
    }

    void OnDropdownValueChanged(Planet p, Dropdown dropdown)
    {
        dropdown.value = 0;
    }

    void OnClick(Planet p)
    {
    }
}
