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

    //Useless, not used anymore
    void switchImageStateOfPanels()
    {
        foreach (Image panel in panels)
        {
            panel.enabled = !panel.IsActive();
        }
    }

    private System.Collections.IEnumerator WaitForFalse()
    {
        while (openfile_script.getUserChoiceStatus())
        {
            yield return null;
        }
        selectedFile = openfile_script.getPathOfSelectedFile();
        openfile_script.exit();
    }

    public string GetSelectedFile()
    {
        return selectedFile;
    }
}
