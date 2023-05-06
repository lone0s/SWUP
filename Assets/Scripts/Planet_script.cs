using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Planet_script : MonoBehaviour
{

    public Planet planet;
    private Vector3 posCam;
    private float distance = -2.5f;

    void Start()
    {
        this.SetPosCam(gameObject.transform);
    }

    void FixedUpdate(){
        //gameObject.transform.Rotate(gameObject.transform.up * this.planet.period * Time.deltaTime);
        //gameObject.transform.RotateAround (Vector3.zero, Vector3.up, this.planet.period * Time.deltaTime * 5);
    }

    void Update()
    {
    }
    internal void SetPosCam(Transform target)
    {
        posCam = target.position;
        posCam.z = target.position.z + distance;
    }

    internal Vector3 GetPosCam()
    {
        return posCam;
    }
}
