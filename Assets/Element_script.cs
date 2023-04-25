using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using System.Xml;

public class Element_script : MonoBehaviour
{
    public GameObject prefab;
    [SerializeField] private UnityEngine.Object file;


    void Start()
    {
        if (file != null)
        {
            string path = AssetDatabase.GetAssetPath(file);
            InitElementPanel(path); 
        }
        
    }

    void Update()
    {
        
    }

    public void InitElementPanel(string path){
        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.Load(path);

        XmlElement root = xmlDoc.DocumentElement;

        foreach (XmlNode node in root.ChildNodes)
        {
            string name = node.Attributes["name"].Value;
            AddElement(name);
        }   
    }

    public void AddElement(string name){
        GameObject element = Instantiate(prefab, transform.position, Quaternion.identity);

        
        // Met à jour le texte du bouton
        Text elementText = element.GetComponentInChildren<Text>();
        Debug.Log(elementText);
        elementText.text = name;
        Debug.Log(elementText.text);
        // Ajoute le nouvel objet à la hiérarchie en tant qu'enfant de l'objet courant
        element.transform.parent = transform;
    }
}
