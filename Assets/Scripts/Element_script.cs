using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using System.Xml;
using UnityEngine.EventSystems;

public class Element_script : MonoBehaviour
{
    public Button elementPrefab;
    public Dropdown dropdownPrefab;
    public Camera camera;


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


            Transform template = dropdown.template;

            // Find the item background object
            Button item = template.Find("Viewport/Content/Item/Item Label").GetComponent<Button>();
            Debug.Log(item);
            item.onClick.AddListener(() => OnClick(p));

            List<string> options = new List<string>();
            foreach(KeyValuePair<string, Planet> s in p.satellite){
                options.Add(s.Key);
            }   
            dropdown.ClearOptions();
            dropdown.AddOptions(options);

            dropdown.GetComponentInChildren<Text>().text = p.name;

            Dropdown_element_script script = dropdown.GetComponentInChildren<Dropdown_element_script>();
            script.planet = p;

            dropdown.GetComponent<Image>().GetComponentInChildren<Button>().onClick.AddListener(() => OnClick(p));
            dropdown.transform.parent = transform;
        }else{
            Button element = Instantiate(elementPrefab, transform.position, Quaternion.identity);

            element.onClick.AddListener(() => OnClick(p));

            Text elementText = element.GetComponentInChildren<Text>();
            elementText.text = p.name;
            element.transform.parent = transform;
        }

        
    }

    public void OnClick(Planet p)
    {
        GameObject planetObj = GameObject.Find(p.name);
        Planet_script pScript = planetObj.GetComponent<Planet_script>();
        Vector3 posCam = pScript.GetPosCam();
        Camera_script script = camera.GetComponent<Camera_script>();
        script.MoveToTarget(posCam);
    }
}
