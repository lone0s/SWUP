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
    public string Name_object = "Planète";

    //taille, position, vitesse rotation, materiel
    public float size_object = 1f;
    public Vector3 position_object = Vector3.zero;
    public float speed_object = 1;
    public Color color_object = Color.white;

    // Start is called before the first frame update
    void Start()
    {
        parent = transform.parent.gameObject;
        if (parent == null)
            Debug.Log("Cet objet n'a pas de parent.");
        addButton = GetComponent<Button>();
        addButton.onClick.AddListener(() => CreatePlanet());
        script = parent.GetComponent<AddPrefab_script>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void CreatePlanet(){
         // Crée une nouvelle sphère avec les propriétés spécifiées
        GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        sphere.name = Name_object;
        sphere.transform.position = position_object;
        sphere.transform.localScale = new Vector3(size_object, size_object, size_object);

        sphere.AddComponent<Planet_script>();
        Planet_script planet_Script = sphere.GetComponent<Planet_script>();
        planet_Script.SetPosCam(sphere.transform);

        // Applique la couleur spécifiée au matériau de la sphère
        Renderer renderer = sphere.GetComponent<Renderer>();
        if (renderer != null)
        {
            Material material = renderer.material;
            if (material != null)
            {
                material.color = color_object;
            }
        }

        script.AddPrefab(Name_object, sphere);
    }
}
