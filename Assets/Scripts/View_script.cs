using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class View_script : MonoBehaviour
{
    public GameObject prefab;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddPrefab(string name, GameObject planet){
       
        // Instancie le prefab à la position de l'objet courant
        GameObject newOnglet = Instantiate(prefab, transform.position, Quaternion.identity);

        // On rattache la planet crée au script du prefab
        View_btn OngletScript = newOnglet.GetComponent<View_btn>();
        OngletScript.SetPlanet(planet);

        // Met à jour le texte du bouton
        Text buttonText = newOnglet.GetComponentInChildren<Text>();
        buttonText.text = name;
        
        // Ajoute le nouvel objet à la hiérarchie en tant qu'enfant de l'objet courant
        newOnglet.transform.parent = transform;
    }
}
