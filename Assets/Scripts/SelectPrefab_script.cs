using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class SelectPrefab_script : MonoBehaviour
{

    private OpenFileDialog_Script script;
    public GameObject select_panel;
    private Button button;
    private string selectedFile = "";
    // Start is called before the first frame update
    void Start()
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
        select_panel.SetActive(true);

        StartCoroutine(WaitForFalse());

        selectedFile = script.getPathOfSelectedFile();

        select_panel.SetActive(false);
    }

    private System.Collections.IEnumerator WaitForFalse()
    {
        while (script.GetIsRun())
        {
            yield return null;
        }
    }
}
