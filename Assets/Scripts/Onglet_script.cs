using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Onglet_script : MonoBehaviour
{
    private Button mainButton;
    private Button deleteButton;
    private GameObject objet;
    private Camera_script camScript;

    // Start is called before the first frame update
    void Start()
    {
        // Récupérer la référence au bouton
        mainButton = GetComponent<Button>();

        // Ajouter la méthode onClick au bouton
        mainButton.onClick.AddListener(MyOnClickMethod);

        deleteButton = this.transform.Find("Close_btn").GetComponent<Button>();
        deleteButton.onClick.AddListener(() => { if(objet) Destroy(objet); Destroy(gameObject); });
    }

    private void MyOnClickMethod()
    {
        camScript.MoveToTarget(objet);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    internal void SetObjet(GameObject obj)
    {
        this.objet = obj;
    }

    internal void SetCamScript(Camera_script script)
    {
        this.camScript = script;
    }
}
