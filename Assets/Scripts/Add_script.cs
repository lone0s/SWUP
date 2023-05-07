using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class Add_script : MonoBehaviour
{
    private GameObject parent;
    private AddPrefab_script script;
    private Button addButton;

    //Parametre object
    public GameObject objet;
    public string Name_objet = "Planète";

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
        addButton.onClick.AddListener(() => CreateObject());
        script = parent.GetComponent<AddPrefab_script>();

        if (objet == null)
            Debug.LogError("Tu n'as pas renseigné l'objet à ajouter. Par défaut, ce sera une sphere");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void CreateObject(){
        // Crée un objet avec les propriétés spécifiées
        GameObject newObjet = (objet == null) ? GameObject.CreatePrimitive(PrimitiveType.Sphere) : objet;

        newObjet.name = Name_objet;
        newObjet.transform.position = position_objet;
        newObjet.transform.localScale = new Vector3(size_objet, size_objet, size_objet);

        // Ajouter le un script à l'objet
        if(script_objet != null)
            newObjet.AddComponent(script_objet.GetClass());

        // Applique la couleur spécifiée au matériau de l'objet
        Renderer renderer = newObjet.GetComponent<Renderer>();
        if (renderer != null)
        {
            Material material = renderer.material;
            if (material != null)
            {
                material.color = color_objet;
            }
        }

        script.AddPrefab(Name_objet, newObjet);
    }
}
