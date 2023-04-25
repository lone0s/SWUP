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

    public void AddPrefab(string name){
       
        // Instancie le prefab à la position de l'objet courant
        GameObject newButton = Instantiate(prefab, transform.position, Quaternion.identity);

        
        // Met à jour le texte du bouton
        Text buttonText = newButton.GetComponentInChildren<Text>();
        buttonText.text = name;
        
        // Ajoute le nouvel objet à la hiérarchie en tant qu'enfant de l'objet courant
        newButton.transform.parent = transform;
    }
}
