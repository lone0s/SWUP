using System;
using UnityEngine;
using UnityEngine.UI;

public class Onglet_script : MonoBehaviour
{
    private Button mainButton;
    private Button deleteButton;

    private GameObject obj;

    private Action<GameObject> onClickOnglet;
    private Action<GameObject> onClickDelete;

    // Start is called before the first frame update
    void Start()
    {
        mainButton = GetComponent<Button>();
        if(onClickOnglet != null) mainButton.onClick.AddListener(() => { onClickOnglet.Invoke(obj); });
        
        deleteButton = this.transform.Find("Close_btn").GetComponent<Button>();
        deleteButton.onClick.AddListener(() => {
            Destroy(null);
            if(onClickDelete != null && obj != null) onClickDelete.Invoke(obj);
            if(obj != null) Destroy(obj);
            if(gameObject != null) Destroy(gameObject);
        });
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetOnClickOnglet(Action<GameObject> f)
    {
        onClickOnglet = f;
    }

    public void SetOnClickDelete(Action<GameObject> f)
    {
        onClickDelete = f;
    }

    public void UpdateTextOnglet()
    {
        gameObject.GetComponentInChildren<Text>().text = obj.name;
    }

    internal void SetObject(GameObject obj)
    {
        this.obj = obj;
    }
}
