using System;
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
    private string objet_path = "";

    public MonoScript script_objet = null;
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

    public void setOnCLick(Action<GameObject> f)
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
        objet_path = select_script.GetSelectedFile();

        GameObject newObjet;
        if (!objet_path.IsUnityNull())
        {
            newObjet = Instantiate(PrefabUtility.LoadPrefabContents(objet_path));
        }
        else
        {
            newObjet = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        }
        
        var uObj = _functionUObj != null ? _functionUObj.Invoke(newObjet) : new UsableObject();


        _onglets.Add(addPrefab_script.AddPrefab(uObj, newObjet));
    }

    public void UpdateOnglets()
    {
        foreach (var onglet in _onglets)
        {
            onglet.UpdateTextOnglet();
        }
    }
}
