using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AddPrefab_script : MonoBehaviour
{
    private GameObject prefab;
    private Camera cam;
    private Camera_script camScript;
    // Start is called before the first frame update
    void Start()
    {
        prefab = Resources.Load<GameObject>("Prefabs/Onglet");
        if (prefab == null)
            Debug.LogError("Impossible de charger le prefab depuis les ressources : Prefabs/Onglet");
        
        cam = Camera.main;
        camScript = cam.GetComponent<Camera_script>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddPrefab(string name, GameObject objet){
       
        // Instancie le prefab à la position de l'objet courant
        GameObject newOnglet = Instantiate(prefab, transform.position, Quaternion.identity);

        // On rattache la planet crée au script du prefab
        Onglet_script OngletScript = newOnglet.GetComponent<Onglet_script>();
        OngletScript.SetObjet(objet);
        OngletScript.SetCamScript(camScript);

        // Met à jour le texte du bouton
        Text buttonText = newOnglet.GetComponentInChildren<Text>();
        buttonText.text = name;
        
        // Ajoute le nouvel objet à la hiérarchie en tant qu'enfant de l'objet courant
        newOnglet.transform.SetParent(transform, false);
    }
}
