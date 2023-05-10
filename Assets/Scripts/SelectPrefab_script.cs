using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class SelectPrefab_script : MonoBehaviour
{
    private GameObject openFileDialogPrefab;
    private OpenFileDialog_Script script;
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
        script = openFileDialog.GetComponent<OpenFileDialog_Script>();
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
        while (script.GetIsRun())
        {
            yield return null;
        }
        selectedFile = script.getPathOfSelectedFile();
        Debug.Log("Received filepath : " + selectedFile);
        switchImageStateOfPanels();
        script.exit();
    }

    // Start is called before the first frame update
    /*        void Start()
            {
                select_panel.SetActive(false);
                button = GetComponent<Button>();
                script = select_panel.GetComponent<OpenFileDialog_Script>();
                button.onClick.AddListener(MyOnClickMethod);
            }

            // Update is called once per frame
            void Update()
            {

            }

            private void MyOnClickMethod()
            {
                Debug.Log("Vroom t'es dans le onClick");
                select_panel.SetActive(true);

                StartCoroutine(WaitForFalse());

                selectedFile = script.getPathOfSelectedFile();
                Debug.Log("Resultat: " + selectedFile);
                select_panel.SetActive(false);
            }
    */
}
