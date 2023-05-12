using System;
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
        private readonly Dictionary<string, UsableObject> _newObjects = new ();

        private void Start()
        {
            if (jsonFile != null)
            {
                var jsonPath = AssetDatabase.GetAssetPath(jsonFile);
                InitObjectsFromJson(jsonPath);
            }

            foreach (var obj in _objects)
            {
                AddPlanet((Planet)obj.Value, null);
            }

            InitMenus();
            InitAddPanel();
            InitCamPanel();
        }

        private void Update()
        {

        }

        private void InitMenus()
        {
            var script = gameObject.GetComponentInChildren<MenusScript>();
            script.menus = _objects;
            script.Function = OnClickMenu;
            script.InitMenusPanel();

            foreach (var obj in script.clickable)
            {
                
                obj.Value.onClick.AddListener(() => {OnClickMenu(obj.Key); });
            }
        }

        private Add_script _addScript;
        private void InitAddPanel()
        {
            _addScript = gameObject.GetComponentInChildren<Add_script>();
            _addScript.setFuncObj(gObj =>
            {
                var p = new Planet();
                gObj.name = p.name;
                RenderPlanet(p);
                _newObjects.Add(p.name, p);
                gObj.AddComponent<Planet_script>();
                gObj.GetComponent<Renderer>().material = p.material;
                var scriptObj = gObj.GetComponent<Planet_script>();
                scriptObj.planet = p;
                return p;
            });
            _addScript.setFunOnCLick(OnClickOnglet);
            
            _addScript.setFunOnClickDelete(gObj =>
            {
                if(gObj != null && _newObjects.ContainsKey(gObj.name))
                    _newObjects.Remove(gObj.name);

                Camera_script camera_Script = Camera.main.GetComponent<Camera_script>();
                if (gObj) camera_Script.Reset();

                AttributPanelScript attribut_script = GameObject.Find("Attribut_panel").GetComponent<AttributPanelScript>();
                attribut_script.resetPanel();
            });
        }

        private void InitCamPanel()
        {
            var script = gameObject.GetComponentInChildren<ConfigCam_script>();
            script.setResetFun(() => {
                AttributPanelScript attribut_script;
                attribut_script = GameObject.Find("Attribut_panel").GetComponent<AttributPanelScript>();
                attribut_script.resetPanel();
            });
        }

        private void RenderPlanet(object obj, bool isChild = false)
        {
            var p = (Planet)obj;

            if (p.GetOldName() == null) p.UpdateOldName();

            if (p.name != p.GetOldName())
            {
                if (_objects.ContainsKey(p.name) || _newObjects.ContainsKey(p.name))
                {
                    p.name = p.GetOldName();
                    Debug.LogWarning("Name already exist => name is not updated");
                }else {
                    GameObject.Find(p.GetOldName()).name = p.name;
                    if (_objects.ContainsKey(p.GetOldName()))
                    {
                        _objects.Remove(p.GetOldName());
                        _objects.Add(p.name, p);
                    }else if (_newObjects.ContainsKey(p.GetOldName()))
                    {
                        _newObjects.Remove(p.GetOldName());
                        _newObjects.Add(p.name, p);
                    }
                    
                    p.UpdateOldName();
                }
            }
            
            var gObj = GameObject.Find(p.name);

            if (gObj == null)
            {
                Debug.LogWarning("NImpossible to render => GameObject not found");
                return;
            }

            if (!isChild)
            {
                gObj.transform.position = p.position;
                p.UpdateOldPosition();
            }else{
                p.position = p.GetOldPosition();
                Debug.LogWarning("Impossible to change satellite position");
            }
            gObj.transform.localScale = new Vector3(p.radius, p.radius, p.radius);
            
            var objRenderer = gObj.GetComponent<Renderer>();
            if (objRenderer != null)
            {
                objRenderer.material = p.material;
            }
        }

        private void OnClickMenu(string objName, bool isChild = false)
        {
            var obj = FindObject(objName, _objects);
            var gObj = GameObject.Find(objName);
            if (gObj != null)
            {
                var camScript = Cam.GetComponent<Camera_script>();
                camScript.MoveToTarget(gObj);
            }

            var pScript = GetComponentInChildren<AttributPanelScript>();
            pScript.function = arg =>
            {
                RenderPlanet(arg,isChild);
                InitMenus();
                pScript.initPanel(obj);
                return null;
            };
            pScript.initPanel(obj);
        }

        private void OnClickOnglet(GameObject gObj)
        {
            var obj = FindObject(gObj.name, _newObjects);
            
            if (gObj != null)
            {
                var camScript = Cam.GetComponent<Camera_script>();
                camScript.MoveToTarget(gObj);
            }
            
            var pScript = GetComponentInChildren<AttributPanelScript>();
            pScript.function = arg =>
            {
                RenderPlanet(arg);
                pScript.initPanel(obj);
                _addScript.UpdateOnglets();
                return null;
            };
            pScript.initPanel(obj);
        }

        private void AddPlanet(Planet p, GameObject parent) 
        { 
            p.InitMaterial();
            var gObj = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            gObj.name = p.name;
            gObj.AddComponent<Planet_script>();
            var script = gObj.GetComponentInChildren<Planet_script>();
            script.planet = p;
            RenderPlanet(p);

            if (parent != null)
            {
                gObj.transform.SetParent(parent.transform);
                script.parent = parent;
            }


            /*var objRenderer = gObj.GetComponent<Renderer>();
            if (objRenderer != null)
            {
                objRenderer.material = Resources.Load<Material>("Materials/" + p.name.ToLower());
                p.material = objRenderer.material;
            }*/

            foreach (var child in p.Children)
            {
                AddPlanet((Planet)child.Value, gObj);
            }
        }

        private void InitObjectsFromJson(string path)
        {
            var json = File.ReadAllText(path);

            var settings = new JsonSerializerSettings
            {
                Converters = { new UsableObjectConverter() }
            };

            var data = JsonConvert.DeserializeObject<SolarSystem>(json, settings);

            if (data != null)
            {
                foreach (var p in data.planets)
                {
                    _objects.Add(p.name, p);
                }
            }
        }

        private void SaveObjectsInJson(string path)
        {
            var data = new Planet[_objects.Count + _newObjects.Count];
            var idx = 0;
            foreach (var obj in _objects)
            {
                var p = (Planet)obj.Value;
                data[idx] = p;
                p.UpdateMaterialPath();
                
                idx++;
            }
            
            foreach (var obj in _newObjects)
            {
                var p = (Planet)obj.Value;
                data[idx] = p;
                p.UpdateMaterialPath();
                idx++;
            }
            

            var settings = new JsonSerializerSettings
            {
                Converters = { new Vector3Converter() }
            };

            var json = JsonConvert.SerializeObject(data, Formatting.Indented, settings);
            File.WriteAllText(path, json);
        }

        private static UsableObject FindObject(string key, Dictionary<string, UsableObject> dict)
        {
            if (dict.TryGetValue(key, out var o))
            {
                return o;
            }
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
        
        private void OnDestroy()
        {
            SaveObjectsInJson("Assets/Resources/Saves/save_" + Guid.NewGuid().ToString("N") + ".json") ;
        }
    }
    
    
}