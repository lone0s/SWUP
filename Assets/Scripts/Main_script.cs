using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using UnityEditor;
using UnityEngine;
using System.Xml;
using System.Reflection;
using System.Xml.Serialization;

public class Main_script : MonoBehaviour
{
    [SerializeField] private UnityEngine.Object xmlFile;
    public Dictionary<string, Planet> planets;

    void Start()
    {
        if (xmlFile != null)
        {
            string path = AssetDatabase.GetAssetPath(xmlFile);
            InitObjects(path);
        }
        
    }

    void Update()
    {
        
    }

    private void InitObjects(string path){
        this.planets = new Dictionary<string, Planet>();

        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.Load(path);

        XmlElement root = xmlDoc.DocumentElement;

        foreach (XmlNode planet in root.ChildNodes)
        {
            
            Planet p = getPlanet(planet);
            
            this.planets.Add(p.name, p);
        }
    }

    private Planet getPlanet(XmlNode planet){
        Planet p = new Planet();
        p.name = planet.Attributes["name"].Value;
        p.mass = StringToFloat(planet.Attributes["mass"].Value);
        p.radius = StringToFloat(planet.Attributes["radius"].Value);
        p.satellite = new Dictionary<string, Planet>();

        foreach (XmlNode attr in planet.ChildNodes)
        {
            float x;
            float y;
            float z;
            switch (attr.Name)
            {
                case "position":
                    p.position = getVector(attr);
                    break;
                case "rotation":
                    p.rotation = getVector(attr);
                    break;
                case "planet":
                    Planet s = getPlanet(attr);
                    p.satellite.Add(s.name,s);
                    break;
                case "orbit":
                    Orbit o = new Orbit();
                    o.semimajoraxis = StringToFloat(attr.Attributes["semimajoraxis"].Value);
                    o.eccentricity = StringToFloat(attr.Attributes["eccentricity"].Value);
                    o.inclination = StringToFloat(attr.Attributes["inclination"].Value);
                    o.period = StringToFloat(attr.Attributes["period"].Value);
                    p.orbit = o;
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

public class Planet
{
    public string name;
    public float mass;
    public float radius;
    public Vector3 position;
    public Vector3 rotation;
    public Orbit orbit;
    public Dictionary<string, Planet> satellite;
}

public class Orbit
{
    public float semimajoraxis;
    public float eccentricity;
    public float inclination;
    public float period;
}
