using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class SelectPrefab_script : MonoBehaviour
{
    private GameObject openFileDialogPrefab;
    private OpenFileDialog_Script openfile_script;
    public GameObject select_panel;
    private Button button;
    private string selectedFile;
    public Image[] panels;
    private Transform MainPanelTransform;

    private void Start()
    {
        MainPanelTransform = GameObject.Find("Main_panel").transform;
        loadImageComponentOfPanels();
        button = GetComponent<Button>();
        button.onClick.AddListener(() =>
        {
            switchImageStateOfPanels();
            insufflateOpenFileDialogPrefab();
            StartCoroutine(WaitForFalse());
        });
    }
    private void Update()
    {
        
    }

    void loadImageComponentOfPanels()
    {
        panels = new Image[]
        {
            GameObject.Find("Menus_panel").GetComponent<Image>(),
            GameObject.Find("Config_panel").GetComponent<Image>(),
            GameObject.Find("Create_panel").GetComponent<Image>() 
        };
    }

    void insufflateOpenFileDialogPrefab()
    {
        openFileDialogPrefab = Resources.Load<GameObject>("Prefabs/OpenFileDialog");
        GameObject openFileDialog = Instantiate(openFileDialogPrefab, MainPanelTransform);
        openfile_script = openFileDialog.GetComponent<OpenFileDialog_Script>();
    }

    void switchImageStateOfPanels()
    {
        foreach (Image panel in panels)
        {
            panel.enabled = !panel.IsActive();
        }
    }

    private System.Collections.IEnumerator WaitForFalse()
    {
        while (openfile_script.GetIsRun())
        {
            yield return null;
        }
        selectedFile = openfile_script.getPathOfSelectedFile();
        Debug.Log("Received filepath : " + selectedFile);
        switchImageStateOfPanels();
        openfile_script.exit();
    }

    public string GetSelectedFile()
    {
        return selectedFile;
    }
}
