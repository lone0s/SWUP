using System.Collections;
using System.Collections.Generic;
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
    public float speed_objet = 1;
    public Color color_objet = Color.white;

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
            Debug.LogError("Tu n'as pas renseigné l'objet à ajouter");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void CreateObject(){
        // Crée une nouvelle sphère avec les propriétés spécifiées
        objet.name = Name_objet;
        objet.transform.position = position_objet;
        objet.transform.localScale = new Vector3(size_objet, size_objet, size_objet);

        objet.AddComponent<Planet_script>();
        Planet_script planet_Script = objet.GetComponent<Planet_script>();
        planet_Script.SetPosCam(objet.transform);

        // Applique la couleur spécifiée au matériau de la sphère
        Renderer renderer = objet.GetComponent<Renderer>();
        if (renderer != null)
        {
            Material material = renderer.material;
            if (material != null)
            {
                material.color = color_objet;
            }
        }

        script.AddPrefab(Name_objet, objet);
    }
}
