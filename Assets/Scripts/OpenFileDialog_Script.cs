using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class OpenFileDialog_Script : MonoBehaviour
{
    // Start is called before the first frame update
    public string directoryPath;
    private string[] files;
    private string[] subdirectories;

    public string fileIconPath;
    public string folderIconPath;
    public string goBackIcon;
    public string exitIcon;

    private Transform viewPanelTransform;
    private ViewPanel_Script viewPanelScript;
    private int nbElementsInTargetFolder;
    private Image searchPanelSpecialActionIcon;
    private Image searchPanelSpecialActionIcon2;
    private InputField pathText;
    private string selectedFile;
    private bool userIsChoosingFile = true;

    private void Awake()
    {
        directoryPath = Path.Combine(Application.dataPath, "Resources");
        updateFilesAndDirectories(directoryPath);
        setNbElementsInFolder();
        viewPanelScript = GameObject.Find("ViewPanel").GetComponent<ViewPanel_Script>();
        viewPanelScript.setNbElementsInPanel(nbElementsInTargetFolder);
        viewPanelTransform = GameObject.Find("ViewPanel").transform;
    }

    private void setNbElementsInFolder()
    {
        nbElementsInTargetFolder = files.Length + subdirectories.Length;
    }
    public int getNbElementsInFolder()
    {
        return nbElementsInTargetFolder;
    }
    void Start()
    {
        directoryPath = Path.Combine(Application.dataPath, "Resources");
        searchPanelSpecialActionIcon = GameObject.Find("SearchPanel/SpecialActionIcon").GetComponent<Image>();
        searchPanelSpecialActionIcon2 = GameObject.Find("SearchPanel/SpecialActionIcon2").GetComponent<Image>();
        float width, height;
        ImagedText_Script.getWidthHeightOfImg(searchPanelSpecialActionIcon, out width, out height);
        searchPanelSpecialActionIcon.sprite = ImagedText_Script.makeSpriteOfPngFile(goBackIcon, width, height);
        ImagedText_Script.getWidthHeightOfImg(searchPanelSpecialActionIcon2, out width, out height);
        searchPanelSpecialActionIcon2.sprite = ImagedText_Script.makeSpriteOfPngFile(exitIcon, width, height);
        pathText = GameObject.Find("SearchPanel/PathInput/Path/InputDim").GetComponent<InputField>();
        pathText.text = correctPathString(directoryPath);
        insufflateLines();
        EventTrigger exitImgTrigger = searchPanelSpecialActionIcon2.gameObject.AddComponent<EventTrigger>();
        EventTrigger.Entry exitEntry = new EventTrigger.Entry();
        exitEntry.eventID = EventTriggerType.PointerClick;
        exitEntry.callback.AddListener((data) =>
        {
            selectedFile = "";
            exit();
        });
        exitImgTrigger.triggers.Add(exitEntry);

        EventTrigger imgTrigger = searchPanelSpecialActionIcon.gameObject.AddComponent<EventTrigger>();
        EventTrigger.Entry imgTriggerEntry = new EventTrigger.Entry();
        imgTriggerEntry.eventID = EventTriggerType.PointerClick;
        imgTriggerEntry.callback.AddListener((data) =>
        {
            string newPath = RemovePathEntry(directoryPath);
        if (newPath != null) { 
            directoryPath = newPath;
            pathText.text = newPath;
            resetViewPanel();
            updateFilesAndDirectories(newPath);
            insufflateLines();
            viewPanelScript.setNbElementsInPanel(getNbElementsInFolder());
            viewPanelScript.updateScrollbar();
            viewPanelScript.resetPanelView();
            }
        });
        imgTrigger.triggers.Add(imgTriggerEntry);
        pathText.onEndEdit.AddListener((text) =>
        {
            string inputPath = Path.GetFullPath(text);
            if (Directory.Exists(inputPath))
            {
                directoryPath = inputPath;
                pathText.text = inputPath;
                resetViewPanel();
                updateFilesAndDirectories(inputPath);
                insufflateLines();
                viewPanelScript.setNbElementsInPanel(getNbElementsInFolder());
                viewPanelScript.updateScrollbar();
                viewPanelScript.resetPanelView();
            }
        });
    }

    string correctPathString(string path)
    {
        return path.Replace('/', '\\');
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void updateFilesAndDirectories(string path)
    {
        files = Directory.GetFiles(path);
        subdirectories = Directory.GetDirectories(path);
        setNbElementsInFolder();

        int pathLength = path.Length;
        if (path.EndsWith("\\"))
        {
            pathLength--;
        }

        for (int i = 0; i < files.Length; ++i)
        {
            files[i] = files[i].Substring(pathLength + 1);
        }
        for (int i = 0; i < subdirectories.Length; ++i)
        {
            subdirectories[i] = subdirectories[i].Substring(pathLength + 1);
        }
    }

    void resetViewPanel()
    {
        foreach (Transform child in viewPanelTransform)
        {
            Destroy(child.gameObject);
        }
    }

    void insufflateLines()
    {
        GameObject linePrefab = Resources.Load<GameObject>("Prefabs/ImagedElementWithAttributes");
        for(int i = 0; i < files.Length; ++i)
        {
            GameObject line = Instantiate(linePrefab, viewPanelTransform);
            MultipleAttributesWithIcon_Script lineScript = line.GetComponent<MultipleAttributesWithIcon_Script>();
            lineScript.iconPath = fileIconPath;
            lineScript.attributeNames[0] = "File";
            lineScript.name = "file" + (i + 1) ;
            lineScript.text = files[i];
            lineScript.initialize();
            EventTrigger onClickEventTrigger = line.gameObject.AddComponent<EventTrigger>();
            EventTrigger.Entry clickEvent = new EventTrigger.Entry();
            clickEvent.eventID = EventTriggerType.PointerClick;
            string currentPath = Path.Combine(directoryPath, files[i]);
            clickEvent.callback.AddListener((data) =>
            {
                selectedFile = currentPath;
                hide();
                userIsChoosingFile = false;
            });
            onClickEventTrigger.triggers.Add(clickEvent);
        }
        for (int i = 0; i < subdirectories.Length; ++i)
        {
            GameObject line = Instantiate(linePrefab, viewPanelTransform);
            MultipleAttributesWithIcon_Script lineScript = line.GetComponent<MultipleAttributesWithIcon_Script>();
            lineScript.iconPath = folderIconPath;
            lineScript.attributeNames[0] = "Directory";
            lineScript.name = "folder" + (i + 1);
            lineScript.text = subdirectories[i];
            lineScript.initialize();

            EventTrigger onClickEventTrigger = line.gameObject.AddComponent<EventTrigger>();
            EventTrigger.Entry clickEvent = new EventTrigger.Entry();
            clickEvent.eventID = EventTriggerType.PointerClick;
            string newPath = Path.GetFullPath(Path.Combine(directoryPath,  subdirectories[i])) ;
            clickEvent.callback.AddListener((data) =>
            {
                resetViewPanel();
                updateFilesAndDirectories(newPath);
                pathText.text = correctPathString(newPath);
                directoryPath = newPath;
                viewPanelScript.setNbElementsInPanel(getNbElementsInFolder());
                viewPanelScript.updateScrollbar();
                viewPanelScript.resetPanelView();
                insufflateLines();
            });
            onClickEventTrigger.triggers.Add(clickEvent);
        }
    }


    //Pour permettre la récupération de l'adresse du fichier selectionné 
    void hide()
    {
        gameObject.SetActive(false);
        Debug.Log(selectedFile);
    }

    public string getPathOfSelectedFile()
    {
        return selectedFile;
    }
    //Pour supprimer la fenetre de dialogue
    public void exit()
    {
        Destroy(gameObject);
    }

    public static string RemovePathEntry(string path)
    {
        return Path.GetDirectoryName(path.TrimEnd(Path.DirectorySeparatorChar));
    }

    public bool getUserChoiceStatus()
    {
        return this.userIsChoosingFile;
    }
}
