using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class Onglet_script : MonoBehaviour
{
    private Button deleteButton;
    private GameObject planet;

    // Start is called before the first frame update
    void Start()
    {
        deleteButton = this.transform.Find("Close_btn").GetComponent<Button>();
        deleteButton.onClick.AddListener(() => { if(planet)Destroy(planet); Destroy(gameObject); });
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    internal void SetPlanet(GameObject obj)
    {
        this.planet = obj;
    }
}
