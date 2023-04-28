using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Add_script : MonoBehaviour
{
    public GameObject parent;
    private AddPrefab_script script;
    private Button addButton;

    //Parametre Planet
    public string sphereName = "Planète";
    //taille, position, vitesse rotation
    public float sphereSize = 1f;
    public Vector3 spherePosition = Vector3.zero;
    public float sphereSpeed = 1;
    public Color sphereColor = Color.white;

    // Start is called before the first frame update
    void Start()
    {
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
        sphere.name = sphereName;
        sphere.transform.position = spherePosition;
        sphere.transform.localScale = new Vector3(sphereSize, sphereSize, sphereSize);

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
                material.color = sphereColor;
            }
        }

        script.AddPrefab(sphereName, sphere);
    }
}
