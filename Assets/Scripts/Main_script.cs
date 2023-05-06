using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using System.Xml;
using System.Reflection;
using System.Xml.Serialization;

public class Main_script : MonoBehaviour
{
    public Camera camera;

    [SerializeField] private UnityEngine.Object xmlFile;
    public Dictionary<string, UsableObject> objects;

    void Start()
    {
        if (xmlFile != null)
        {
            string path = AssetDatabase.GetAssetPath(xmlFile);
            InitObjects(path);
        }

        foreach (KeyValuePair<string, UsableObject> obj in this.objects)
        {
            AddPlanet((Planet) obj.Value);
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
            Planet_script script = gObj.GetComponent<Planet_script>();
            Vector3 posCam = script.GetPosCam();
            Camera_script camScript = camera.GetComponent<Camera_script>();
            camScript.MoveToTarget(posCam);
        }
        
    }

    public void AddPlanet(Planet p){
        GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        sphere.name = p.name;
        sphere.transform.position = p.position;
        sphere.transform.localScale = new Vector3(p.radius, p.radius, p.radius);

        sphere.AddComponent<Planet_script>();
        Planet_script script = sphere.GetComponentInChildren<Planet_script>();
        script.planet = p;

        Renderer renderer = sphere.GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.material = Resources.Load<Material>("Materials/" + p.name.ToLower());
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

    private UsableObject getPlanet(XmlNode node){
        Planet p = new Planet();
        p.name = node.Attributes["name"].Value;
        p.radius = StringToFloat(node.Attributes["radius"].Value);
        p.period = StringToFloat(node.Attributes["period"].Value);
        p.children = new Dictionary<string, UsableObject>();

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



public class UsableObject
{
    public string name;
    public Dictionary<string, UsableObject> children;
}

public class Planet : UsableObject
{
    public float radius;
    public float period;
    public Vector3 position;
}
