using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

public class AttributPanelScript : MonoBehaviour
{
    public Type objectType;
    public object objectInstance;
    private HashSet<Type> compatibleTypes;
    private FieldInfo[] fields;
    private Type[] fieldTypes;

    bool attributePanelHasElements = false;

    void Start()
    {
        compatibleTypes= new HashSet<Type>();
        fillCompatibleTypes();
    }

    
    void initPanel(object objInstance)
    {
        this.objectInstance = objInstance;
        if (attributePanelHasElements)
            resetPanel();
        insufflatePanel();
    }

    void setObjectAttributeValue<T>(string fieldName, T fieldValue)
    {
        objectType.GetField(fieldName).SetValue(objectInstance, fieldValue);
    }

    object getObjectAttributeValue(string fieldName)
    {
        object fieldValue = objectType.GetField(fieldName).GetValue(objectInstance);
        return fieldValue;
    }

    void insufflatePanel()
    {
        GameObject troisDPrefab = Resources.Load<GameObject>("Prefabs/3D_Input");
        GameObject deuxDPrefab = Resources.Load<GameObject>("Prefabs/2D_Input");
        GameObject unDPrefab = Resources.Load<GameObject>("Prefabs/1D_Input");
        for (int i = 0; i < fields.Length; ++i)
        {
            if (compatibleTypes.Contains(fieldTypes[i]))
            {

                if (fieldTypes[i] == typeof(Vector3))
                {
                    GameObject attributeSubPanel = Instantiate(troisDPrefab, transform);
                    InputField dim1 = attributeSubPanel.transform.Find("dim1/InputDim").GetComponent<InputField>();
                    InputField dim2 = attributeSubPanel.transform.Find("dim2/InputDim").GetComponent<InputField>();
                    InputField dim3 = attributeSubPanel.transform.Find("dim3/InputDim").GetComponent<InputField>();
                    Vector3 vec3 = (Vector3)getObjectAttributeValue(fields[i].Name);
                    dim1.text = vec3.x.ToString();
                    dim2.text = vec3.y.ToString();
                    dim3.text = vec3.z.ToString();
                    Text dim1Name = dim1.GetComponentInChildren<Text>();
                    string fieldName = fields[i].Name;
                    dim1.onEndEdit.AddListener((value) =>
                    {
                        vec3.x = float.Parse(value);
                        setObjectAttributeValue(fieldName, vec3);
                    });

                    dim2.onEndEdit.AddListener((value) =>
                    {
                        vec3.y = float.Parse(value);
                        setObjectAttributeValue(fieldName, vec3);
                    });

                    dim3.onEndEdit.AddListener((value) =>
                    {
                        vec3.z = float.Parse(value);
                        setObjectAttributeValue(fieldName, vec3);
                    });
                }

                if (fieldTypes[i] == typeof(Vector2))
                {
                    GameObject attributeSubPanel = Instantiate(deuxDPrefab, transform);
                    InputField dim1 = attributeSubPanel.transform.Find("dim1/InputDim").GetComponent<InputField>();
                    InputField dim2 = attributeSubPanel.transform.Find("dim2/InputDim").GetComponent<InputField>();
                    Vector2 vec2 = (Vector2)getObjectAttributeValue(fields[i].Name);
                    dim1.text = vec2.x.ToString();
                    dim2.text = vec2.y.ToString();
                    Text dim1Name = dim1.GetComponentInChildren<Text>();
                    string fieldName = fields[i].Name;
                    dim1.onEndEdit.AddListener((value) =>
                    {
                        vec2.x = float.Parse(value);
                        setObjectAttributeValue(fieldName, vec2);
                    });

                    dim2.onEndEdit.AddListener((value) =>
                    {
                        vec2.y = float.Parse(value);
                        setObjectAttributeValue(fieldName, vec2);
                    });
                }

                if (fieldTypes[i] == typeof(int) ||
                   fieldTypes[i] == typeof(float) ||
                   fieldTypes[i] == typeof(double))
                {
                    GameObject attributeSubPanel = Instantiate(unDPrefab, transform);
                    InputField dim1 = attributeSubPanel.transform.Find("dim1/InputDim").GetComponent<InputField>();
                    Text dimName = attributeSubPanel.transform.Find("dim1/nameDim").GetComponent<Text>();
                    string fieldName = fields[i].Name;
                    dimName.text = fieldName;

                    if (fieldTypes[i] == typeof(int))
                    {
                        int attributeValue = (int)getObjectAttributeValue(fields[i].Name);
                        dim1.text = attributeValue.ToString();
                        dim1.onEndEdit.AddListener((value) =>
                        {
                            attributeValue = int.Parse(value);
                            setObjectAttributeValue(fieldName, attributeValue);
                        });
                    }

                    if (fieldTypes[i] == typeof(float))
                    {
                        float attributeValue = (float)getObjectAttributeValue(fields[i].Name);
                        dim1.text = attributeValue.ToString();
                        dim1.onEndEdit.AddListener((value) =>
                        {
                            attributeValue = float.Parse(value);
                            setObjectAttributeValue(fieldName, attributeValue);
                        });
                    }

                    if (fieldTypes[i] == typeof(double))
                    {
                        double attributeValue = (double)getObjectAttributeValue(fields[i].Name);
                        dim1.text = attributeValue.ToString();
                        dim1.onEndEdit.AddListener((value) =>
                        {
                            attributeValue = double.Parse(value);
                            setObjectAttributeValue(fieldName, attributeValue);
                        });
                    }

                }
                this.attributePanelHasElements = true;
            }
        }

    }




    void resetPanel()
    {
        foreach(Transform child in transform)
        {
            Destroy(child.gameObject);
        }
            this.attributePanelHasElements = false;
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

        foreach(Type type in types)
        {
            compatibleTypes.Add(type);
        }
    }
}
