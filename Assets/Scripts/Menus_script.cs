using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using System.Xml;
using UnityEngine.EventSystems;

public class Menus_script : MonoBehaviour
{
    public Button menuPrefab;
    public Dropdown dropdownPrefab;

    public Dictionary<string,UsableObject> menus; 
    public Dictionary<string, Button> clickable;

    public Func<string, string> function;


    void Start()
    {
    }

    void Update()
    {
    }

    public void InitMenusPanel(){

        this.clickable = new Dictionary<string, Button>();

        if(menus != null){
            foreach (KeyValuePair<string, UsableObject> obj in menus)
            {
                AddMenu(obj.Value);
            }
        }
        
    }

    public void AddMenu(UsableObject o){
        if(o.children.Count > 0){
            Dropdown dropdown = Instantiate(dropdownPrefab, transform.position, Quaternion.identity);
            Transform template = dropdown.template;

            /*if(this.function != null){
                Debug.Log("not null");
                Dropdown_element_script script = template.Find("Viewport/Content/Item/Item Label").GetComponent<Dropdown_element_script>();
                script.function = function; 
                script.obj = o; 
            }*/

            List<string> options = new List<string>();  
            foreach(KeyValuePair<string, UsableObject> child in o.children){
                options.Add(child.Key);
            }
            dropdown.ClearOptions();
            dropdown.AddOptions(options);

            Button button = dropdown.GetComponent<Image>().GetComponentInChildren<Button>();
            this.clickable.Add(o.name, button);

            dropdown.GetComponentInChildren<Text>().text = o.name;
            dropdown.transform.parent = transform;
            
        }else{
            Button menu = Instantiate(menuPrefab, transform.position, Quaternion.identity);

            this.clickable.Add(o.name, menu);

            Text menuText = menu.GetComponentInChildren<Text>();
            menuText.text = o.name;
            menu.transform.parent = transform;
        }
    }
}
