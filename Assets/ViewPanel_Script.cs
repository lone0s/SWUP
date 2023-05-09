using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewPanel_Script : MonoBehaviour
{
    // Start is called before the first frame update
    private int nbElementsInPanel;
    private VerticalScrollBar_Script scrollbarScript;
    void Start()
    {
        Debug.Log("View panel has " + nbElementsInPanel + " elements in panel");
        scrollbarScript = transform.parent.GetComponentInChildren<VerticalScrollBar_Script>();
        transmitNbElementsToScrollbar();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setNbElementsInPanel(int nbElements)
    {
        nbElementsInPanel = nbElements;
    }

    public void transmitNbElementsToScrollbar()
    {
        Debug.Log("Script named : " + scrollbarScript.name);
        scrollbarScript.setNbElementsInTargetPanel(nbElementsInPanel);
    }

    public void updateScrollbar()
    {
        scrollbarScript.setNbElementsInTargetPanel(nbElementsInPanel);
        scrollbarScript.updateScrollbar();
    }

    public void resetPanelView()
    {
        scrollbarScript.resetPanelView();
    }
}
