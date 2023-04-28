using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AddPrefab_script : MonoBehaviour
{
    public GameObject prefab;
    public GameObject cam;
    private Camera_script camScript;
    // Start is called before the first frame update
    void Start()
    {
        camScript = cam.GetComponent<Camera_script>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddPrefab(string name, GameObject planet){
       
        // Instancie le prefab à la position de l'objet courant
        GameObject newOnglet = Instantiate(prefab, transform.position, Quaternion.identity);

        // On rattache la planet crée au script du prefab
        Onglet_script OngletScript = newOnglet.GetComponent<Onglet_script>();
        OngletScript.SetPlanet(planet);
        OngletScript.SetCamScript(camScript);

        // Met à jour le texte du bouton
        Text buttonText = newOnglet.GetComponentInChildren<Text>();
        buttonText.text = name;
        
        // Ajoute le nouvel objet à la hiérarchie en tant qu'enfant de l'objet courant
        newOnglet.transform.parent = transform;
    }
}
