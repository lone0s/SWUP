using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Planet_script : MonoBehaviour
{

    private Vector3 posCam;
    private float distance = -2.5f;

    // Start is called before the first frame update
    void Start()
    {
        //GameObject planet = GetComponent<GameObject>();
        this.SetPosCam(gameObject.transform);
    }

    // Update is called once per frame
    void Update()
    {
    }
    internal void SetPosCam(Transform target)
    {
        posCam = target.position;
        posCam.y = target.position.y + distance;
    }

    internal Vector3 GetPosCam()
    {
        return posCam;
    }
}
