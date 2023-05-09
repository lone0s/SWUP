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
    // Start is called before the first frame update
    void Start()
    {
        scrollbar = GetComponent<Scrollbar>();
        targetPanelRectTransform = GameObject.Find(panelName).GetComponent<RectTransform>();
        contentHeight = targetPanelRectTransform.rect.height;
        maxNbElementsInPanel = Mathf.FloorToInt(contentHeight / objectMinHeight);
        if (nbElementsInTargetPanel > maxNbElementsInPanel)
        {
            scrollbar.interactable = true;
            updateScrollbar();
        }
        else
        {
            scrollbar.interactable = false;
        }

        scrollbar.onValueChanged.AddListener((float val) => onScrollbarValueChanged(val));
        Debug.Log("Scrollbar script detected " + nbElementsInTargetPanel + " elements in target panel");
    }

    public void updateScrollbar()
    {
        /*nbElementsInTargetPanel = targetPanelRectTransform.childCount;*/
        scrollbar.size = (float)maxNbElementsInPanel / nbElementsInTargetPanel;
        scrollbar.interactable = nbElementsInTargetPanel > maxNbElementsInPanel;
    }

    public void onScrollbarValueChanged(float scrollValue)
    {
        float scrollableContentHeight = objectMinHeight * (nbElementsInTargetPanel - maxNbElementsInPanel);
        float scrollableHeight = targetPanelRectTransform.rect.height - scrollableContentHeight;
        float newPos = Mathf.Lerp(0f, scrollableContentHeight, scrollValue);
        targetPanelRectTransform.localPosition = new Vector3(0, newPos, 0);
    }

    public void resetPanelView()
    {
        targetPanelRectTransform.anchoredPosition = Vector3.zero;
        scrollbar.value = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setNbElementsInTargetPanel(int nbElements)
    {
        this.nbElementsInTargetPanel = nbElements;
    }
}