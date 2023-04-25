using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Add_script : MonoBehaviour
{
    public GameObject parent;
    private View_script script;
    private Button button;
    private float sphereSize = 1f;
    private string sphereName = "Planète";
    private Vector3 spherePosition = Vector3.zero;
    private Quaternion sphereRotation = Quaternion.identity;
    private Color sphereColor = Color.white;

    // Start is called before the first frame update
    void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(() => CreatePlanet());
        script = parent.GetComponent<View_script>();
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
        sphere.transform.rotation = sphereRotation;
        sphere.transform.localScale = new Vector3(sphereSize, sphereSize, sphereSize);

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

        script.AddPrefab(sphereName);
    }
}
