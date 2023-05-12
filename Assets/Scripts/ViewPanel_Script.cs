using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewPanel_Script : MonoBehaviour
{

    private int nbElementsInPanel;
    private VerticalScrollBar_Script scrollbarScript;
    private RectTransform rectTransform;

    private void Awake()
    {
        rectTransform = gameObject.GetComponent<RectTransform>();
        scrollbarScript = transform.parent.GetComponentInChildren<VerticalScrollBar_Script>();
        transmitNbElementsToScrollbar();
    }
    void Start()
    {
    }

    void Update()
    {
        
    }

    public void setNbElementsInPanel(int nbElements)
    {
        nbElementsInPanel = nbElements;
    }

    public void transmitNbElementsToScrollbar()
    {
        scrollbarScript.setNbElementsInTargetPanel(nbElementsInPanel);
    }

    public void updateScrollbar()
    {
        scrollbarScript.setNbElementsInTargetPanel(nbElementsInPanel);
        scrollbarScript.updateScrollbar();
    }

    public void resetPanelView()
    {
        rectTransform.anchoredPosition = Vector3.zero;
        scrollbarScript.resetScrollbar();

        foreach(Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        updateScrollbar();
    }
}
