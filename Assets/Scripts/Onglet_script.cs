using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Unity.VisualScripting;

public class Onglet_script : MonoBehaviour
{
    private Button mainButton;
    private Button deleteButton;
    private GameObject objet;
    private Camera_script camScript;
    private AttributPanelScript attribut_script;
    public Func<GameObject, int> OnClickOnglet;
    public Action<GameObject> OnClickDelete;

    // Start is called before the first frame update
    void Start()
    {
        // Recuper la reference au bouton
        mainButton = GetComponent<Button>();

        attribut_script = GameObject.Find("Attribut_panel").GetComponent<AttributPanelScript>();

        // Ajouter la m�thode onClick au bouton
        mainButton.onClick.AddListener(()=>{ OnClickOnglet.Invoke(objet); });

        deleteButton = this.transform.Find("Close_btn").GetComponent<Button>();
        deleteButton.onClick.AddListener(() => { 
            if (objet) camScript.Reset();
            if (OnClickDelete != null) OnClickDelete.Invoke(objet);  
            Destroy(objet); Destroy(gameObject); attribut_script.resetPanel(); });
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
