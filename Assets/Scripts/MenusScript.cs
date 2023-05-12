using System;
using System.Collections.Generic;
using Assets.DataClasses;
using UnityEngine;
using UnityEngine.UI;


namespace Assets.Scripts
{
    public class MenusScript : MonoBehaviour
    {
        public Button MenuPrefab;
        public Dropdown DropdownPrefab;

        public Dictionary<string, UsableObject> menus;
        public Dictionary<string, Button> clickable;

        public Action<string> Function;


        void Start()
        {
        }

        void Update()
        {
        }

        private void ClearPanel()
        {
            foreach (Transform child in gameObject.GetComponent<Transform>())
            {
                Destroy(child.gameObject);
            }
        }

        public void InitMenusPanel()
        {
            ClearPanel();
            clickable = new Dictionary<string, Button>();

            if (menus != null)
            {
                foreach (KeyValuePair<string, UsableObject> obj in menus)
                {
                    AddMenu(obj.Value);
                }
            }

        }

        private void AddMenu(UsableObject o)
        {
            if (o.Children.Count > 0)
            {
                var dropdown = Instantiate(DropdownPrefab, transform.position, Quaternion.identity);

                var options = new List<string>();
                foreach (var child in o.Children)
                {
                    options.Add(child.Key);
                }

                dropdown.ClearOptions();
                dropdown.AddOptions(options);

                var button = dropdown.GetComponent<Image>().GetComponentInChildren<Button>();
                clickable.Add(o.name, button);

                /*var a = dropdown.transform.Find("Template").gameObject;
                Debug.LogWarning(a);
                var b = dropdown.transform.Find("Template/Viewport").gameObject;
                Debug.LogWarning(b);
                var c = dropdown.transform.Find("Template/Viewport/Content").gameObject;
                Debug.LogWarning(c);
                var d = dropdown.transform.Find("Template/Viewport/Content/Item").gameObject;
                Debug.LogWarning(d);
                var e = dropdown.transform.Find("Template/Viewport/Content/Item/Item Label").gameObject;
                Debug.LogWarning(e);*/

                /*var script = dropdown.transform.Find("Template/Viewport/Content/Item/Item label")?.gameObject.GetComponent<DropdownMenuScript>();
                script.function = Function;*/

                dropdown.GetComponentInChildren<Text>().text = o.name;
                dropdown.transform.SetParent(transform, false);

            }
            else
            {
                Button menu = Instantiate(MenuPrefab, transform.position, Quaternion.identity);

                clickable.Add(o.name, menu);

                Text menuText = menu.GetComponentInChildren<Text>();
                menuText.text = o.name;
                menu.transform.SetParent(transform, false);
            }
        }
    }
}
