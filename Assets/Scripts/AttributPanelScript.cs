using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Reflection;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AttributPanelScript : MonoBehaviour
{
    public Type objectType;
    public object objectInstance;
    private HashSet<Type> compatibleTypes;
    private FieldInfo[] fields;
    private Type[] fieldTypes;

    public Func<object, object> function;

    bool attributePanelHasElements = false;

    private char correctNumberDecimalSeparator;
    private char incorrectNumberDecimalSeparator;

    private string selectedFile;

    private void Awake()
    {
        fillCompatibleTypes();
        setCorrectAndIncorrectDecimalSeparators();
    }

    void Start(){}

    void Update(){}

    void callFunction() {
        if (this.function != null) this.function.Invoke(this.objectInstance);
    }


    public void initPanel(object objInstance)
    {
        this.objectInstance = objInstance;
        this.objectType = objInstance.GetType();
        this.fields = objectType.GetFields();
        this.fieldTypes = new Type[fields.Length];
        for (int i = 0; i < fields.Length; ++i)
        {
            fieldTypes[i] = fields[i].FieldType;
        }
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

    public static string correctDecimalSeparator(string str, char correctDecimalSep, char incorrectDecimalSep)
    {
        if (str.Contains(incorrectDecimalSep))
        {
            return str.Replace(incorrectDecimalSep, correctDecimalSep);
        }
        return str;
    }

    /* /!\ Penser a faire un try cast et en cas de catch, juste remplacer le mauvais char /!\ */
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
                    dim1.contentType = InputField.ContentType.DecimalNumber;
                    dim2.contentType = InputField.ContentType.DecimalNumber;
                    dim3.contentType = InputField.ContentType.DecimalNumber;

                    Vector3 vec3 = (Vector3)getObjectAttributeValue(fields[i].Name);
                    dim1.text = vec3.x.ToString();
                    dim2.text = vec3.y.ToString();
                    dim3.text = vec3.z.ToString();
                    Text dim1Name = dim1.GetComponentInChildren<Text>();
                    string fieldName = fields[i].Name;
                    dim1.onEndEdit.AddListener((value) =>
                    {
                        string val = correctDecimalSeparator(value, correctNumberDecimalSeparator,
                            incorrectNumberDecimalSeparator);
                        vec3.x = float.Parse(val);
                        setObjectAttributeValue(fieldName, vec3);
                        callFunction();
                    });

                    dim2.onEndEdit.AddListener((value) =>
                    {
                        string val = correctDecimalSeparator(value, correctNumberDecimalSeparator,
                            incorrectNumberDecimalSeparator);
                        vec3.y = float.Parse(val);
                        setObjectAttributeValue(fieldName, vec3);
                        callFunction();
                    });

                    dim3.onEndEdit.AddListener((value) =>
                    {
                        string val = correctDecimalSeparator(value, correctNumberDecimalSeparator,
                            incorrectNumberDecimalSeparator);
                        vec3.z = float.Parse(val);
                        setObjectAttributeValue(fieldName, vec3);
                        callFunction();
                    });
                }

                if (fieldTypes[i] == typeof(Vector2))
                {
                    GameObject attributeSubPanel = Instantiate(deuxDPrefab, transform);
                    InputField dim1 = attributeSubPanel.transform.Find("dim1/InputDim").GetComponent<InputField>();
                    InputField dim2 = attributeSubPanel.transform.Find("dim2/InputDim").GetComponent<InputField>();
                    dim1.contentType = InputField.ContentType.DecimalNumber;
                    dim2.contentType = InputField.ContentType.DecimalNumber;

                    Vector2 vec2 = (Vector2)getObjectAttributeValue(fields[i].Name);
                    dim1.text = vec2.x.ToString();
                    dim2.text = vec2.y.ToString();
                    Text dim1Name = dim1.GetComponentInChildren<Text>();
                    string fieldName = fields[i].Name;
                    dim1.onEndEdit.AddListener((value) =>
                    {
                        string val = correctDecimalSeparator(value, correctNumberDecimalSeparator,
                            incorrectNumberDecimalSeparator);
                        vec2.x = float.Parse(val);
                        setObjectAttributeValue(fieldName, vec2);
                        callFunction();
                    });

                    dim2.onEndEdit.AddListener((value) =>
                    {
                        string val = correctDecimalSeparator(value, correctNumberDecimalSeparator,
                            incorrectNumberDecimalSeparator);
                        vec2.y = float.Parse(val);
                        setObjectAttributeValue(fieldName, vec2);
                        callFunction();
                    });
                }

                if (fieldTypes[i] == typeof(int) ||
                    fieldTypes[i] == typeof(float) ||
                    fieldTypes[i] == typeof(double) ||
                    fieldTypes[i] == typeof(string) ||
                    fieldTypes[i] == typeof(char))

                {
                    GameObject attributeSubPanel = Instantiate(unDPrefab, transform);
                    InputField dim1 = attributeSubPanel.transform.Find("dim1/InputDim").GetComponent<InputField>();
                    Text dimName = attributeSubPanel.transform.Find("dim1/nameDim").GetComponent<Text>();
                    string fieldName = fields[i].Name;
                    dimName.text = fieldName;

                    if (fieldTypes[i] == typeof(int))
                    {
                        dim1.contentType = InputField.ContentType.IntegerNumber;
                        int attributeValue = (int)getObjectAttributeValue(fields[i].Name);
                        dim1.text = attributeValue.ToString();
                        dim1.onEndEdit.AddListener((value) =>
                        {
                            attributeValue = int.Parse(value);
                            setObjectAttributeValue(fieldName, attributeValue);
                            callFunction();
                        });
                    }

                    if (fieldTypes[i] == typeof(float))
                    {
                        dim1.contentType = InputField.ContentType.DecimalNumber;
                        float attributeValue = (float)getObjectAttributeValue(fields[i].Name);
                        dim1.text = attributeValue.ToString();
                        dim1.onEndEdit.AddListener((value) =>
                        {
                            string val = correctDecimalSeparator(value, correctNumberDecimalSeparator,
                                incorrectNumberDecimalSeparator);
                            attributeValue = float.Parse(val);
                            setObjectAttributeValue(fieldName, attributeValue);
                            callFunction();
                        });
                    }

                    if (fieldTypes[i] == typeof(double))
                    {
                        dim1.contentType = InputField.ContentType.DecimalNumber;
                        double attributeValue = (double)getObjectAttributeValue(fields[i].Name);
                        dim1.text = attributeValue.ToString();
                        dim1.onEndEdit.AddListener((value) =>
                        {
                            string val = correctDecimalSeparator(value, correctNumberDecimalSeparator,
                                incorrectNumberDecimalSeparator);
                            attributeValue = double.Parse(val);
                            setObjectAttributeValue(fieldName, attributeValue);
                            callFunction();
                        });
                    }

                    if (fieldTypes[i] == typeof(char))
                    {
                        char attributeValue = (char)getObjectAttributeValue(fields[i].Name);
                        dim1.characterLimit = 1;
                        dim1.text = attributeValue.ToString();
                        dim1.onEndEdit.AddListener((value) =>
                        {
                            attributeValue = char.Parse(value);
                            setObjectAttributeValue(fieldName, attributeValue);
                            callFunction();
                        });
                    }

                    if (fieldTypes[i] == typeof(String))
                    {
                        string attributeValue = getObjectAttributeValue(fields[i].Name).ToString();
                        dim1.text = attributeValue;
                        dim1.onEndEdit.AddListener((value) =>
                        {
                            attributeValue = value;
                            setObjectAttributeValue(fieldName, attributeValue);
                            callFunction();
                        });
                    }
                }

                if (fieldTypes[i] == typeof(Material))
                {
                    GameObject objToShowTextureLive = new GameObject("TextureObject");
                    Image imgToShowTextureLive = objToShowTextureLive.AddComponent<Image>();
                    Material attributeMaterial = (Material)getObjectAttributeValue(fields[i].Name);
                    Debug.Log("Material Name : " + attributeMaterial.name);

                    UpdateSprite(attributeMaterial, imgToShowTextureLive);
                    objToShowTextureLive.transform.SetParent(transform);
                    var fieldName = fields[i].Name;
                    var imgTrigger = imgToShowTextureLive.AddComponent<EventTrigger>();
                    var imgEntry = new EventTrigger.Entry();
                    imgEntry.eventID = EventTriggerType.PointerClick;
                    imgEntry.callback.AddListener((data) =>
                    {
                        StartCoroutine(WaitForUpdate(fieldName, imgToShowTextureLive));
                    });
                    imgTrigger.triggers.Add(imgEntry);
                }

                attributePanelHasElements = true;
            }
        }
    }

    private void UpdateSprite(Material m, Image img)
    {
        var attributeTexture = (Texture2D)m.mainTexture;

        var textureWidth = attributeTexture.width;
        var textureHeight = attributeTexture.height;

        Sprite newSprite = Sprite.Create(attributeTexture, new Rect(0, 0, textureWidth, textureHeight), Vector2.zero);

        img.sprite = newSprite;
    }
    private IEnumerator WaitForUpdate(string fieldName, Image img)
    {
        var openFileDialogPrefab = Resources.Load<GameObject>("Prefabs/OpenFileDialog");
        var rootCanvas = FindObjectOfType<Canvas>();
        var openFileDialog = Instantiate(openFileDialogPrefab, rootCanvas.transform);
        var ofdScript = openFileDialog.GetComponent<OpenFileDialog_Script>();
        string path = ofdScript.correctPath(Path.Combine(Application.dataPath, "Resources/Materials"));
        ofdScript.updateOFD(path, "mat", path);
        while (ofdScript.getUserChoiceStatus())
        {
            yield return null;
        }
        selectedFile = ofdScript.getPathOfSelectedFile();
        ofdScript.exit();
        if (selectedFile != null)
        {
            string absolutePath = ofdScript.getPathFromRessources(selectedFile);
            var resourcesPath = Path.ChangeExtension(absolutePath, null);
            var m = Resources.Load<Material>(resourcesPath);
            Debug.LogWarning(m);
            setObjectAttributeValue(fieldName, m);
            UpdateSprite(m, img);
            callFunction();
        }
        
    }

    public void resetPanel()
    {
        foreach(Transform child in transform)
        {
            Destroy(child.gameObject);
        }
            this.attributePanelHasElements = false;
    }

    void fillCompatibleTypes()
    {
        this.compatibleTypes = new HashSet<Type>();
        Type[] types = {
                typeof(int), typeof(double),typeof(float), // Types numeriques
                typeof(Vector2), typeof(Vector3), //Types vecteurs
                typeof(char), typeof(string), //Types texte
                typeof(Material) //Type pas comme les autres 
        };

        foreach(Type type in types)
        {
            compatibleTypes.Add(type);
        }
    }

    private void setCorrectAndIncorrectDecimalSeparators()
    {
        Char[] buffer = CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator.ToCharArray();
        correctNumberDecimalSeparator = buffer[0];
        incorrectNumberDecimalSeparator =
            correctNumberDecimalSeparator == ',' ?
                '.' :
                ',';
    }
}
