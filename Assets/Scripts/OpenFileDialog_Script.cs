using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class OpenFileDialog_Script : MonoBehaviour
{
    // Start is called before the first frame update
    public string directoryPath;

    public string fileIconPath;
    public string folderIconPath;
    public string goBackIcon;
    public string exitIcon;

    private string baseDirLockPath;
    private string[] files;
    private string[] subdirectories;

    private Transform viewPanelTransform;
    private ViewPanel_Script viewPanelScript;
    private Image searchPanelSpecialActionIcon;
    private Image searchPanelSpecialActionIcon2;
    private InputField pathInput;
    private GameObject linePrefab;

    private int nbElementsInDirectory;
    private string selectedFile;
    private string fileFilterFormat;

    private bool triggerFileFilter = false;
    private bool triggerLockOnBaseDirectory = false;
    private bool triggerMetaFileFilter = true;
    private bool userIsChoosingFile = true;


    private char correctDirSeparator;
    private char wrongDirSeparator;

    private void Awake()
    {

        correctDirSeparator = Path.DirectorySeparatorChar;
        wrongDirSeparator = 
            correctDirSeparator == '\\' ? 
                '/' : 
                '\\';

        if (directoryPath == null || directoryPath == "")
            directoryPath = correctPath(Path.Combine(Application.dataPath, "Resources"));
        //Du traitement redondant just in case
        directoryPath = correctPath(Path.GetFullPath(directoryPath));
        baseDirLockPath = correctPath(Application.dataPath);

        viewPanelScript = GameObject.Find("ViewPanel").GetComponent<ViewPanel_Script>();
        viewPanelTransform = GameObject.Find("ViewPanel").transform;

        verifyCoherenceDirLockAndBaseDir();
        updateFilesAndDirectories(directoryPath);

        linePrefab = Resources.Load<GameObject>("Prefabs/ImagedElementWithAttributes");
    }

    void Start()
    {

        searchPanelSpecialActionIcon = GameObject.Find("SearchPanel/SpecialActionIcon").GetComponent<Image>();
        searchPanelSpecialActionIcon2 = GameObject.Find("SearchPanel/SpecialActionIcon2").GetComponent<Image>();

        float width, height;

        ImagedText_Script.getWidthHeightOfImg(searchPanelSpecialActionIcon, out width, out height);
        searchPanelSpecialActionIcon.sprite = ImagedText_Script.makeSpriteOfPngFile(goBackIcon, width, height);
        
        ImagedText_Script.getWidthHeightOfImg(searchPanelSpecialActionIcon2, out width, out height);
        searchPanelSpecialActionIcon2.sprite = ImagedText_Script.makeSpriteOfPngFile(exitIcon, width, height);
        
        pathInput = GameObject.Find("SearchPanel/PathInput/Path/InputDim").GetComponent<InputField>();
        pathInput.text = correctPath(directoryPath);
        

        EventTrigger exitImgTrigger = searchPanelSpecialActionIcon2.gameObject.AddComponent<EventTrigger>();
        EventTrigger goBackImgTrigger = searchPanelSpecialActionIcon.gameObject.AddComponent<EventTrigger>();

        EventTrigger.Entry exitEntry = new EventTrigger.Entry();
        exitEntry.eventID = EventTriggerType.PointerClick;
        exitEntry.callback.AddListener((data) =>
        {
            selectedFile = "";
            exit();
        });
        exitImgTrigger.triggers.Add(exitEntry);

        
        EventTrigger.Entry goBackEntry = new EventTrigger.Entry();
        goBackEntry.eventID = EventTriggerType.PointerClick;
        goBackEntry.callback.AddListener((data) =>
        {
            string newPath = RemovePathEntry(directoryPath);
            if (newPath != null )
            {
                directoryPath = triggerLockOnBaseDirectory ?
                    isAValidSubPath(baseDirLockPath, newPath) ?
                        newPath :
                        baseDirLockPath :
                    newPath;
                pathInput.text = directoryPath;
                doRoutine();
            }
        });
        goBackImgTrigger.triggers.Add(goBackEntry);

        pathInput.onEndEdit.AddListener((text) =>
        {

            if (text != string.Empty)
            {
                string newInputPath = Directory.Exists(text) ?
                    Path.GetFullPath(text) :
                    directoryPath;
                directoryPath = triggerLockOnBaseDirectory ?
                    isAValidSubPath(baseDirLockPath, newInputPath) ?
                        newInputPath :
                        baseDirLockPath :
                    newInputPath;
                doRoutine();
            }
            pathInput.text = directoryPath;
        });

        insufflateLines();
        verifyCoherenceDirLockAndBaseDir();
    }

    public void update(string newPath = null, string fileFilter = null, string dirLock = null)
    {
        if (newPath != null && newPath != directoryPath)
        {
            directoryPath = newPath;
        }
        if (fileFilter != null && fileFilter != fileFilterFormat)
            setFileFilter(fileFilter);
        if (dirLock != null && dirLock != baseDirLockPath)
            setDirLock(dirLock); 
        verifyCoherenceDirLockAndBaseDir();
        doRoutine();
    }

    void doRoutine()
    {

        updateFilesAndDirectories(directoryPath);
        resetView();
        insufflateLines();
    }

    void updateFilesAndDirectories(string path)
    {
        
        files = Directory.GetFiles(path);
        subdirectories = Directory.GetDirectories(path);
        
        if(triggerFileFilter)
            files = files.Where(file => file.EndsWith(fileFilterFormat)).ToArray();

        if(triggerMetaFileFilter)
            files = files.Where(file => (!file.Contains(".meta"))).ToArray();

        updateNbElementsInDirectory();

        int pathLength = path.EndsWith("\\") ? path.Length -1 : path.Length ;

        for (int i = 0; i < files.Length; ++i)
        {
            files[i] = files[i].Substring(pathLength + 1);
        }
        for (int i = 0; i < subdirectories.Length; ++i)
        {
            subdirectories[i] = subdirectories[i].Substring(pathLength + 1);
        }

        viewPanelScript.setNbElementsInPanel(nbElementsInDirectory);
    }


    void insufflateLines()
    {
        
        for(int i = 0; i < files.Length; ++i)
        {
            GameObject line = Instantiate(linePrefab, viewPanelTransform);
            MultipleAttributesWithIcon_Script lineScript = line.GetComponent<MultipleAttributesWithIcon_Script>();
            EventTrigger onClickEventTrigger = line.gameObject.AddComponent<EventTrigger>();
            EventTrigger.Entry clickEvent = new EventTrigger.Entry();

            lineScript.iconPath = fileIconPath;
            lineScript.attributeNames[0] = "File";
            lineScript.name = "file" + (i + 1) ;
            lineScript.text = files[i];
            lineScript.initialize();

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
            EventTrigger onClickEventTrigger = line.gameObject.AddComponent<EventTrigger>();
            EventTrigger.Entry clickEvent = new EventTrigger.Entry();

            string newPath = correctPath(Path.GetFullPath(Path.Combine(directoryPath, subdirectories[i])));

            clickEvent.eventID = EventTriggerType.PointerClick;
            clickEvent.callback.AddListener((data) =>
            {
                pathInput.text = newPath;
                directoryPath = newPath;
                doRoutine();
            });
            onClickEventTrigger.triggers.Add(clickEvent);

            lineScript.iconPath = folderIconPath;
            lineScript.attributeNames[0] = "Directory";
            lineScript.name = "folder" + (i + 1);
            lineScript.text = subdirectories[i];
            lineScript.initialize();
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
        return correctPath(selectedFile);
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
        return userIsChoosingFile;
    }

    public void setFileFilter(string fileFormat)
    {
        triggerFileFilter = true;
        fileFilterFormat = fileFormat.Contains(".") ? fileFormat : "." + fileFormat;
    }
    public void setMetaFilter(bool newState)
    {
        Debug.Log(triggerMetaFileFilter + "|" + newState);
        if (newState != triggerMetaFileFilter)
        {
            Debug.Log("Vroom");
            triggerMetaFileFilter = newState;
            update();
        }
    }

    public void setDirLock(string baseDir)
    {
        baseDirLockPath = baseDir;
        triggerLockOnBaseDirectory = true;
    }

    //Si jamais..
    public void resetFilters()
    {
        triggerFileFilter = false;
        triggerMetaFileFilter = false;
        fileFilterFormat = null;
        update();
    }

    public void resetDirLock()
    {
        baseDirLockPath = null;
        triggerLockOnBaseDirectory = false;
        update();
    }

    public static bool isAValidSubPath(string basePath, string otherPath)
    {
        return (
            basePath != otherPath && 
            (otherPath.StartsWith(basePath)) 
            || basePath == otherPath);
    }

    private void verifyCoherenceDirLockAndBaseDir()
    {
        if(triggerLockOnBaseDirectory && !isAValidSubPath(baseDirLockPath, directoryPath))
        {
            Debug.LogWarning("Incohérence entre le path servant de lock, et le path de la fenetre de dialogue, lock desactive ://");
            triggerLockOnBaseDirectory = false;
        }
    }
    private void updateNbElementsInDirectory()
    {
        nbElementsInDirectory = files.Length + subdirectories.Length;
    }

    public string correctPath(string path)
    {
        return path.Replace(wrongDirSeparator, correctDirSeparator);
    }

    void resetView()
    {
        viewPanelScript.resetPanelView();
    }

    public string getRessourcesPath()
    {
        return correctPath(Path.Combine(Application.dataPath, "Resources")); 
    }

    public string getPathFromRessources(string path)
    {
        if (path.StartsWith(getRessourcesPath()))
            return path.Substring(getRessourcesPath().Length + 1);
        return path;
    }

}
