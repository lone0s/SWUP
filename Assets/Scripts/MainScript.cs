using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.IO;

using Newtonsoft.Json;
using Assets.DataClasses;
using Object = UnityEngine.Object;

namespace Assets.Scripts
{
    public class MainScript : MonoBehaviour
    {
        public Camera Cam;

        [SerializeField] private Object jsonFile;
        private readonly Dictionary<string, UsableObject> _objects = new ();

        private void Start()
        {
            if (jsonFile != null)
            {
                var jsonPath = AssetDatabase.GetAssetPath(jsonFile);
                InitObjectsFromJSON(jsonPath);
            }

            foreach (var obj in _objects)
            {
                AddPlanet((Planet)obj.Value, null);
            }

            InitMenus();
        }

        private void Update()
        {

        }

        private void InitMenus()
        {
            MenusScript script = gameObject.GetComponentInChildren<MenusScript>();
            script.menus = _objects;
            script.function = arg =>
            {
               OnClickMenu(arg);
               return null;
            };
            script.InitMenusPanel();

            foreach (var obj in script.clickable)
            {
                obj.Value.onClick.AddListener(() => { OnClickMenu(obj.Key); });
            }
        }

        private GameObject RenderPlanet(object obj)
        {
            var p = (Planet)obj;

            if (p.GetOldName() != null && p.name != p.GetOldName())
            {
                if (_objects.ContainsKey(p.name))
                {
                    p.name = p.GetOldName();
                    Debug.LogWarning("Name already exist => name is not updated");
                }

                {
                    _objects.Remove(p.GetOldName());
                    _objects.Add(p.name, p);
                    p.UpdateOldName();
                }
            }
            
            var gObj = GameObject.Find(p.name);
            if (gObj == null)
            {
                gObj = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                gObj.AddComponent<Planet_script>();
                var script = gObj.GetComponentInChildren<Planet_script>();
                script.planet = p;
            }
            
            gObj.transform.position = p.position;
            gObj.transform.localScale = new Vector3(p.radius, p.radius, p.radius);

            return gObj;
        }

        private void OnClickMenu(string objName)
        {
            var obj = FindObject(objName, this._objects);
            var gObj = GameObject.Find(objName);
            if (gObj != null)
            {
                Camera_script camScript = Cam.GetComponent<Camera_script>();
                camScript.MoveToTarget(gObj);
            }

            AttributPanelScript pScript = GetComponentInChildren<AttributPanelScript>();
            pScript.function = (arg) =>
            {
                RenderPlanet(arg);
                return null;
            };
            pScript.initPanel(obj);

        }

        private void AddPlanet(Planet p, GameObject parent)
        {
            var gObj = RenderPlanet(p);
            var script = gObj.GetComponentInChildren<Planet_script>();

            if (parent != null)
            {
                gObj.transform.SetParent(parent.transform);
                script.parent = parent;
            }


            var objRenderer = gObj.GetComponent<Renderer>();
            if (objRenderer != null)
            {
                objRenderer.material = Resources.Load<Material>("Materials/" + p.name.ToLower());
            }

            foreach (KeyValuePair<string, UsableObject> child in p.Children)
            {
                AddPlanet((Planet)child.Value, gObj);
            }
        }

        private void InitObjectsFromJSON(string path)
        {
            string json = File.ReadAllText(path);

            var settings = new JsonSerializerSettings
            {
                Converters = { new UsableObjectConverter(typeof(Planet)) }
            };

            SolarSystem data = JsonConvert.DeserializeObject<SolarSystem>(json, settings);

            if (data != null)
            {
                foreach (var p in data.planets)
                {
                    _objects.Add(p.name, p);
                }
            }
        }

        private void SaveObjectsInJSON(string path)
        {
            Planet[] data = new Planet[this._objects.Count];
            var idx = 0;
            foreach (KeyValuePair<string, UsableObject> obj in this._objects)
            {
                data[idx] = (Planet)obj.Value;
                idx++;
            }

            var settings = new JsonSerializerSettings
            {
                Converters = { new Vector3Converter() }
            };

            var json = JsonConvert.SerializeObject(data, Newtonsoft.Json.Formatting.Indented, settings);
            File.WriteAllText(path, json);
        }

        private static UsableObject FindObject(string key, Dictionary<string, UsableObject> dict)
        {
            if (dict.ContainsKey(key))
            {
                return dict[key];
            }
            else
            {
                foreach (UsableObject child in dict.Values)
                {
                    UsableObject result = FindObject(key, child.Children);
                    if (result != null)
                    {
                        return result;
                    }
                }

                return null;
            }
        }
    }
}