using System;
using System.Collections;
using System.Collections.Generic;
using Assets.DataClasses;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class AddPrefab_script : MonoBehaviour
{
    private GameObject onglet;
    private Camera cam;
    private Camera_script camScript;

    public Action<GameObject> FunctionOnClick;
    public Action<GameObject> FunctionOnClickDelete;

    public Text buttonText;
    // Start is called before the first frame update
    void Start()
    {
        onglet = Resources.Load<GameObject>("Prefabs/Onglet");
        if (onglet == null)
            Debug.LogError("Impossible de charger l'onglet depuis les ressources : Prefabs/Onglet");
        
        cam = Camera.main;
        camScript = cam.GetComponent<Camera_script>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    public Onglet_script AddPrefab(UsableObject uObj, GameObject objet){
       
        // Instancie le prefab à la position de l'objet courant
        GameObject newOnglet = Instantiate(onglet, transform.position, Quaternion.identity);

        // On rattache la planet crée au script du prefab
        Onglet_script ongletScript = newOnglet.GetComponent<Onglet_script>();
        ongletScript.OnClickOnglet = FunctionOnClick;
        ongletScript.OnClickDelete = FunctionOnClickDelete;
        ongletScript.SetObjet(objet);
        ongletScript.SetCamScript(camScript);

        // Met à jour le texte du bouton
        buttonText = newOnglet.GetComponentInChildren<Text>();
        buttonText.text = uObj.name;
        
        // Ajoute le nouvel objet à la hiérarchie en tant qu'enfant de l'objet courant
        newOnglet.transform.SetParent(transform, false);

        return ongletScript;
    }
}
