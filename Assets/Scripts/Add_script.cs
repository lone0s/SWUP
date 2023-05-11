using System;
using System.Collections;
using System.Collections.Generic;
using Assets.DataClasses;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class Add_script : MonoBehaviour
{
    private GameObject parent;
    private AddPrefab_script addPrefab_script;
    private SelectPrefab_script select_script;
    private Button addButton;
    private string objet_path = "";

    public Func<GameObject,UsableObject> FunctionUObj;

    //Parametre object
    private GameObject obj;
    public string name_objet = "Planète";

    //taille, position, vitesse rotation, materiel
    public float size_objet = 1f;
    public Vector3 position_objet = Vector3.zero;
    public Color color_objet = Color.white;
    public MonoScript script_objet = null;

    // Start is called before the first frame update
    void Start()
    {
        parent = transform.parent.gameObject;
        if (parent == null)
            Debug.Log("Cet objet n'a pas de parent.");
        addButton = GetComponent<Button>();
        addButton.onClick.AddListener(CreateObject);
        addPrefab_script = parent.GetComponent<AddPrefab_script>();

        GameObject select_btn = GameObject.Find("Select_btn");
        select_script = select_btn.GetComponent<SelectPrefab_script>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void CreateObject(){
        objet_path = select_script.GetSelectedFile();

        GameObject newObjet;
        if (!objet_path.IsUnityNull())
        {
            obj = PrefabUtility.LoadPrefabContents(objet_path);
            newObjet = Instantiate(obj);
        }
        else
        {
            newObjet = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        }
        

        //newObjet.name = name_objet;
        newObjet.transform.position = position_objet;
        newObjet.transform.localScale = new Vector3(size_objet, size_objet, size_objet);

        // Ajouter le un script à l'objet
        // if(script_objet != null)
        //     newObjet.AddComponent(script_objet.GetClass());

        // Applique la couleur spécifiée au matériau de l'objet
        if (newObjet.TryGetComponent<Renderer>(out var renderer))
        {
            Material material = renderer.material;
            if (material != null)
            {
                material.color = color_objet;
            }
        }

        var uObj = FunctionUObj != null ? FunctionUObj.Invoke(newObjet) : new UsableObject();

        newObjet.name = uObj.name;
        
        addPrefab_script.AddPrefab(uObj, newObjet);
    }

    public GameObject GetObject()
    {
        return obj;
    }
}
