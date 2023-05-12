using UnityEngine;
using UnityEngine.UI;

public class VerticalScrollBar_Script : MonoBehaviour
{
    public string panelName;
    public int objectMinHeight;

    private int maxNbElementsInPanel;
    private Scrollbar scrollbar;
    private RectTransform targetPanelRectTransform;
    private float contentHeight;
    private int nbElementsInTargetPanel;

    private void Awake()
    {
        scrollbar = GetComponent<Scrollbar>();
        targetPanelRectTransform = GameObject.Find(panelName).GetComponent<RectTransform>();
        contentHeight = targetPanelRectTransform.rect.height;
        scrollbar.onValueChanged.AddListener((float val) => onScrollbarValueChanged(val));
    }
    void Start()
    {



        updateScrollbar();

        
    }

    void Update()
    {

    }

    public void updateScrollbar()
    {
        maxNbElementsInPanel = Mathf.FloorToInt(contentHeight / objectMinHeight);
        scrollbar.size = (float)maxNbElementsInPanel / nbElementsInTargetPanel;
        scrollbar.interactable = nbElementsInTargetPanel > maxNbElementsInPanel;
    }

    public void onScrollbarValueChanged(float scrollValue)
    {
        float scrollableContentHeight = objectMinHeight * (nbElementsInTargetPanel - maxNbElementsInPanel);
        float newPos = Mathf.Lerp(0f, scrollableContentHeight, scrollValue);
        targetPanelRectTransform.localPosition = new Vector3(0, newPos, 0);
    }

    public void resetScrollbar()
    {
        scrollbar.value = 0;
    }



    public void setNbElementsInTargetPanel(int nbElements)
    {
        this.nbElementsInTargetPanel = nbElements;
    }
}