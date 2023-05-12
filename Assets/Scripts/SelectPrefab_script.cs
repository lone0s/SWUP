using UnityEngine;
using UnityEngine.UI;

public class SelectPrefab_script : MonoBehaviour
{
    private GameObject openFileDialogPrefab;
    private OpenFileDialog_Script openfile_script;
    private Button button;
    private string selectedFile;
    private Transform CanvasTransform;
    public GameObject obj;

    private void Start()
    {
        CanvasTransform = FindObjectOfType<Canvas>().transform;
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

    void insufflateOpenFileDialogPrefab()
    {
        openFileDialogPrefab = Resources.Load<GameObject>("Prefabs/OpenFileDialog");
        GameObject openFileDialog = Instantiate(openFileDialogPrefab, CanvasTransform);
        openfile_script = openFileDialog.GetComponent<OpenFileDialog_Script>();
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

    public GameObject GetObject()
    {
        return obj;
    }
}
