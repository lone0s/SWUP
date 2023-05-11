using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using System.Xml;
using System.Reflection;
using System.Xml.Serialization;
using System.IO;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class Main_script : MonoBehaviour
{
    public Camera cam;

    [SerializeField] private UnityEngine.Object jsonFile;
    public Dictionary<string, UsableObject> objects = new Dictionary<string, UsableObject>();

    void Start()
    {
        if (jsonFile != null)
        {
            string jsonPath = AssetDatabase.GetAssetPath(jsonFile);
            InitObjectsFromJSON(jsonPath);
        }

        foreach (KeyValuePair<string, UsableObject> obj in this.objects)
        {
            AddPlanet((Planet) obj.Value, null);
        }

        InitMenus();
    }

    void Update()
    {
        
    }

    public void InitMenus(){
        Menus_script script = gameObject.GetComponentInChildren<Menus_script>();
        script.menus = this.objects;
        script.function = (arg) => { OnClickMenu(arg); return null; };
        script.InitMenusPanel();

        foreach(KeyValuePair<string, Button> obj in script.clickable){
            obj.Value.onClick.AddListener(() => {OnClickMenu(obj.Key);});
        }
    }

    public GameObject renderPlanet(object obj){
        Planet p = (Planet) obj;
        GameObject gObj = GameObject.Find(p.name);
        if(gObj == null){
            gObj = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            gObj.AddComponent<Planet_script>();
            Planet_script script = gObj.GetComponentInChildren<Planet_script>();
            script.planet = p;
        }

        gObj.name = p.name;
        gObj.transform.position = p.position;
        gObj.transform.localScale = new Vector3(p.radius, p.radius, p.radius);

        return gObj;
    }

    public void OnClickMenu(string objName){
        UsableObject obj = FindObject(objName, this.objects);
        GameObject gObj = GameObject.Find(objName);
        if(gObj != null){
            Camera_script camScript = cam.GetComponent<Camera_script>();
            camScript.MoveToTarget(gObj);
        }
        AttributPanelScript pScript = GetComponentInChildren<AttributPanelScript>();
        pScript.function = (arg) => {renderPlanet(arg); return null;};
        pScript.initPanel(obj);
        
    }

    public void AddPlanet(Planet p, GameObject parent){
        GameObject gObj = this.renderPlanet(p);
        Planet_script script = gObj.GetComponentInChildren<Planet_script>();

        if(parent != null){
            gObj.transform.SetParent(parent.transform);
            script.parent = parent;
        }
        

        Renderer renderer = gObj.GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.material = Resources.Load<Material>("Materials/" + p.name.ToLower());
        }

        foreach(KeyValuePair<string,UsableObject> child in p.children){
            AddPlanet((Planet) child.Value, gObj);
        }
    }

    private void InitObjectsFromJSON(string path){
        string json = File.ReadAllText(path);

        var settings = new JsonSerializerSettings
        {
            Converters = { new UsableObjectConverter(typeof(Planet)) }
        };

        SolarSystem data = JsonConvert.DeserializeObject<SolarSystem>(json, settings);  

        foreach (Planet p in data.planets) {
            this.objects.Add(p.name,p);
        }

    }

    private void SaveObjectsInJSON(string path){
        SolarSystem data = new SolarSystem();
        data.planets = new Planet[this.objects.Count];
        int idx = 0;
        foreach(KeyValuePair<string,UsableObject> obj in this.objects){
            data.planets[idx] = (Planet) obj.Value;
            idx++;
        }
        var settings = new JsonSerializerSettings
        {
            Converters = { new Vector3Converter() }
        };
        
        string json = JsonConvert.SerializeObject(data, Newtonsoft.Json.Formatting.Indented, settings);
        File.WriteAllText(path, json);
    }

    public static UsableObject FindObject(string key, Dictionary<string, UsableObject> dict) {
        if (dict.ContainsKey(key)) {
            return dict[key];
        } else {
            foreach (UsableObject child in dict.Values) {
                UsableObject result = FindObject(key, child.children);
                if (result != null) {
                    return result;
                }
            }
            return null;
        }
    }
}

[System.Serializable]
public class UsableObject
{
    public string name = "Object";
    public Dictionary<string, UsableObject> children = new Dictionary<string, UsableObject>();
}

[System.Serializable]
public class Planet : UsableObject
{
    public float radius = 0.25F;
    public float period = 365F;
    public float rotation_days = 1;
    public Vector3 position = new Vector3(0,0,0);
}


[System.Serializable]
public class SolarSystem {
    public Planet[] planets;
}



public class UsableObjectConverter : JsonConverter
{

    private Type objType;

    public UsableObjectConverter(Type t){
        this.objType = t;
    }

    public override bool CanConvert(Type objectType)
    {
        return objectType == typeof(UsableObject);
    }

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
        JObject obj = JObject.Load(reader);
        dynamic result = Convert.ChangeType(existingValue, this.objType);

        if(result == null){
            result = (Planet)existingValue ?? new Planet();
        }

        serializer.Populate(obj.CreateReader(), result);

        return result;
    }

    

    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
        throw new NotImplementedException();
    }
}

public class Vector3Converter : JsonConverter<Vector3>
{
    public override void WriteJson(JsonWriter writer, Vector3 value, JsonSerializer serializer)
    {
        var jObject = new JObject
        {
            { "x", value.x },
            { "y", value.y },
            { "z", value.z }
        };
        jObject.WriteTo(writer);
    }

    public override Vector3 ReadJson(JsonReader reader, Type objectType, Vector3 existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        return existingValue;
    }
}