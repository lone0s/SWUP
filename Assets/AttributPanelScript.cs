using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

public class AttributPanelScript : MonoBehaviour
{
    // Start is called before the first frame update
    
    public Type objectType;
    public object objectInstance;

    private HashSet<Type> compatibleTypes;

    private int nbFields;
    private FieldInfo[] fields;
    private Type[] fieldTypes;

    void Start()
    {
        compatibleTypes= new HashSet<Type>();
        fillCompatibleTypes();
        Planet test = new Planet();
        objectInstance = test;
        objectType = typeof(Planet);
        Debug.Log("Type: " + objectType.Name);
        /*PropertyInfo[] properties = objectType.GetProperties();*/ //<-- Permet de récupérer methodes avec getter + setter
        fields = objectType.GetFields();
        fieldTypes = new Type[fields.Length];
        for(int i = 0; i < fields.Length; i++)
        {
            fieldTypes[i] = fields[i].FieldType;
        }
        Debug.Log("Nb attributes : "+ fields.Length);
        foreach (FieldInfo field in fields)
        {
            Debug.Log(field.Name + " | " + field.FieldType);
        }
        setObjectAttribute<float>("radius", 666f);
        Debug.Log((float)objectType.GetField("radius").GetValue(objectInstance));

        insufflatePanel();
    }

    void setObjectAttribute<T>(string fieldName, T fieldValue)
    {
        objectType.GetField(fieldName).SetValue(objectInstance, fieldValue);
    }

    void insufflatePanel()
    {
        GameObject troisDPrefab  = Resources.Load<GameObject>("Prefabs/3D_Input");
        GameObject deuxDPrefab = Resources.Load<GameObject>("Prefabs/2D_Input");
        GameObject unDPrefab = Resources.Load<GameObject>("Prefabs/1D_Input");
        Debug.Log("Niveau 1 InsufflatPanel atteint");
        for(int i = 0; i < fields.Length; ++i)
        {
            Debug.Log("Niveau 2 InsufflatPanel atteint");
            Debug.Log(fieldTypes[i]);
            Debug.Log(compatibleTypes.Contains(fieldTypes[i]));
            if (compatibleTypes.Contains(fieldTypes[i]))
            {
                Debug.Log("Niveau 3 InsufflatPanel atteint");
                if (fieldTypes[i] == typeof(Vector3))
                {
                    Debug.Log("Niveau 4 InsufflatPanel atteint");
                    GameObject attributeSubPanel = Instantiate(troisDPrefab, transform);
                    InputField dim1 = attributeSubPanel.transform.Find("dim1/InputDim").GetComponent<InputField>();
                    Debug.Log(dim1.name);
                }
            }
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void fillCompatibleTypes()
    {
        Type[] types = {
                typeof(int), typeof(double),typeof(float), // Types numériques
                typeof(Vector2), typeof(Vector3), //Types vecteurs
                typeof(char), typeof(string) //Types texte
        };
    }
}
