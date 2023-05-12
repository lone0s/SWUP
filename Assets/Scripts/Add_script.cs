using System;
using System.Collections.Generic;
using Assets.DataClasses;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class Add_script : MonoBehaviour
{
    private GameObject parent;
    private AddPrefab_script addPrefab_script;
    private SelectPrefab_script select_script;
    private Button addButton;
    private string object_path = "";

    public MonoScript object_script = null;
    private Func<GameObject,UsableObject> _functionUObj;
    private Action<GameObject> _functionOnClick;
    private Action<GameObject> _functionOnClickDelete;

    private List<Onglet_script> _onglets = new ();

    // Start is called before the first frame update
    void Start()
    {
        parent = transform.parent.gameObject;
        if (parent == null)
            Debug.Log("Cet objet n'a pas de parent.");
        addButton = GetComponent<Button>();
        addButton.onClick.AddListener(CreateObject);
        addPrefab_script = parent.GetComponent<AddPrefab_script>();
        
        

        GameObject select_btn = GameObject.Find("Select_btn");
        select_script = select_btn.GetComponent<SelectPrefab_script>();
    }

    public void setFunOnCLick(Action<GameObject> f)
    {
        _functionOnClick = f;
        addPrefab_script.setFunctionOnClick(_functionOnClick);
    }
    
    public void setFuncObj(Func<GameObject,UsableObject> f)
    {
        _functionUObj = f;
    }

    public void setFunOnClickDelete(Action<GameObject> f)
    {
        _functionOnClickDelete = f;
        addPrefab_script.setFunctionOnClickDelete(_functionOnClickDelete);
    }
    
    
    // Update is called once per frame
    void Update()
    {
        
    }

    private void CreateObject(){
        object_path = select_script.GetSelectedFile();

        GameObject newObject;
        if (!object_path.IsUnityNull())
        {
            newObject = Instantiate(PrefabUtility.LoadPrefabContents(object_path));
        }
        else
        {
            newObject = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        }
        
        var uObj = _functionUObj != null ? _functionUObj.Invoke(newObject) : new UsableObject();


        _onglets.Add(addPrefab_script.AddPrefab(uObj, newObject));
    }

    public void UpdateOnglets()
    {
        foreach (var onglet in _onglets)
        {
            if(!onglet.IsUnityNull())
                onglet.UpdateTextOnglet();
        }
    }
}
