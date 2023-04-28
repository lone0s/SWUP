using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using System.Xml;
using UnityEngine.EventSystems;

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
            options.Add("");
            foreach(KeyValuePair<string, Planet> s in p.satellite){
                options.Add(s.Key);
            }   
            dropdown.ClearOptions();
            dropdown.AddOptions(options);

            dropdown.GetComponentInChildren<Text>().text = p.name;

            //Dropdown dropdownComponent = dropdown.GetComponent<Dropdown>();

            // Add an event listener to the Dropdown's onValueChanged event
            dropdown.onValueChanged.AddListener(delegate { Debug.Log("aaa");OnDropdownValueChanged(dropdown); });

            dropdown.value = -1;

            //dropdown.RefreshShownValue();

            Dropdown_element_script script = dropdown.GetComponentInChildren<Dropdown_element_script>();
            script.planet = p;

            dropdown.GetComponent<Image>().GetComponentInChildren<Button>().onClick.AddListener(() => OnClick(p));
            dropdown.transform.parent = transform;
        }else{
            GameObject element = Instantiate(elementPrefab, transform.position, Quaternion.identity);

            Text elementText = element.GetComponentInChildren<Text>();
            elementText.text = p.name;
            element.transform.parent = transform;
        }

        
    }

    void OnDropdownValueChanged(Dropdown changedDropdown)
{
    Debug.Log("aa");
}

    private void OnOptionSelected(Dropdown.OptionData option)
    {
        Debug.Log("Option selected: " + option.text);
    }

    void OnClick(){
        Debug.Log("aa");
    }

    void OnDropdownValueChanged(int index)
    {
        Debug.Log("clicked 2222");
    }

    void OnDropdownValueChanged(Planet p, Dropdown dropdown)
    {
        Debug.Log("clicked 2222");
        dropdown.value = 0;
    }

    void OnClick(Planet p)
    {
        Debug.Log("clicked");
    }
}
