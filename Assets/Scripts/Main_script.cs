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

public class Main_script : MonoBehaviour
{
    public Camera cam;

    [SerializeField] private UnityEngine.Object xmlFile;
    [SerializeField] private UnityEngine.Object jsonFile;
    public Dictionary<string, UsableObject> objects;

    void Start()
    {
        if (xmlFile != null)
        {
            string path = AssetDatabase.GetAssetPath(xmlFile);
            InitObjects(path);
            string jsonPath = AssetDatabase.GetAssetPath(jsonFile);
            InitJsonObjects(jsonPath);
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
            obj.Value.onClick.AddListener(() => OnClickMenu(obj.Key));
        }
    }

    public void OnClickMenu(string objName){
        GameObject gObj = GameObject.Find(objName);
        if(gObj != null){
            Camera_script camScript = cam.GetComponent<Camera_script>();
            camScript.MoveToTarget(gObj);
        }
        
    }

    public void AddPlanet(Planet p, GameObject parent){
        GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        sphere.name = p.name;
        sphere.transform.position = p.position;
        sphere.transform.localScale = new Vector3(p.radius, p.radius, p.radius);

        sphere.AddComponent<Planet_script>();
        Planet_script script = sphere.GetComponentInChildren<Planet_script>();
        script.planet = p;

        if(parent != null){
            sphere.transform.SetParent(parent.transform);
            script.parent = parent;
        }
        

        Renderer renderer = sphere.GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.material = Resources.Load<Material>("Materials/" + p.name.ToLower());
        }

        foreach(KeyValuePair<string,UsableObject> child in p.children){
            AddPlanet((Planet) child.Value, sphere);
        }
    }

    private void InitObjects(string path){
        this.objects = new Dictionary<string, UsableObject>();

        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.Load(path);

        XmlElement root = xmlDoc.DocumentElement;

        foreach (XmlNode planet in root.ChildNodes)
        {
            
            UsableObject obj = getPlanet(planet);
            
            this.objects.Add(obj.name, obj);
        }
    }

    private void InitJsonObjects(string path){
        string json = File.ReadAllText(path);
        SolarSystem data = JsonUtility.FromJson<SolarSystem>(json);

        PlanetData pp = new PlanetData();
        //pp.Children = new Planet[2];
        pp.Children[0] = new Planet();

        foreach (PlanetData p in data.planets) {
            Debug.Log(p.name);
            /*foreach(KeyValuePair<string,UsableObject> child in p.children2){
                Debug.Log(child.Key);
            }*/
            /*foreach(UsableObject child in p.children){
                Debug.Log(child.name);
            }*/
            /*foreach(UsableObject child in p.Children){
                Debug.Log(child.name);
            }*/
        }

    }

    private UsableObject getPlanet(XmlNode node){
        Planet p = new Planet();
        p.name = node.Attributes["name"].Value;
        p.radius = StringToFloat(node.Attributes["radius"].Value);
        p.period = StringToFloat(node.Attributes["period"].Value);
        p.rotation_days = StringToFloat(node.Attributes["rotation_days"].Value);

        foreach (XmlNode attr in node.ChildNodes)
        {
            switch (attr.Name)
            {
                case "position":
                    p.position = getVector(attr);
                    break;
                case "planet":
                    UsableObject s = getPlanet(attr);
                    p.children.Add(s.name,s);
                    break;
                default:
                    break;
            }
        }
        return p;
    }

    private Vector3 getVector(XmlNode attr){
        float x = StringToFloat(attr.Attributes["x"].Value);
        float y = StringToFloat(attr.Attributes["y"].Value);
        float z = StringToFloat(attr.Attributes["z"].Value);
        return new Vector3(x, y, z);
    }

    private float StringToFloat(string s){
        return float.Parse(s, System.Globalization.CultureInfo.InvariantCulture);
    }
}


public abstract class UsableObject
{
    public string name = "Object";
    public Dictionary<string, UsableObject> children = new Dictionary<string, UsableObject>();

    public abstract string extractXML();
}


public class Planet : UsableObject
{
    public float radius = 0.25F;
    public float period = 365F;
    public float rotation_days = 1;
    public Vector3 position = new Vector3(0,0,0);

    public override string extractXML(){
        return "";
    }
}

[System.Serializable]
public abstract class UsableObjectData
{
    public string name = "Object";
    public Dictionary<string, UsableObject> children2 = new Dictionary<string, UsableObject>();
    public Planet[] children = new Planet[0];

    public Planet[] Children 
    {
        get{return children;}
        set
        {
            //children = value;
            Debug.Log("aaaaaaaa");
            foreach (Planet child in value)
            {
                Debug.Log(child.name);
                Debug.Log(child.name);
            }
        }
    }

    public abstract string extractXML();
}

[System.Serializable]
public class PlanetData : UsableObjectData
{
    public float radius = 0.25F;
    public float period = 365F;
    public float rotation_days = 1;
    public Vector3 position = new Vector3(0,0,0);

    public override string extractXML(){
        return "";
    }
}



[System.Serializable]
public class SolarSystem {
    public PlanetData[] planets;
}