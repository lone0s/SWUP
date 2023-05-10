using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Planet_script : MonoBehaviour
{

    public Planet planet;
    public GameObject parent;
    private Vector3 posCam;
    private float distance = -2.5f;
    private float timeFullRotationEarth = 10;
    private float timeAxisRotationEarth = 1000;

    void Start()
    {
        distance *= planet.radius;
    }

    void FixedUpdate(){
        float timeFullRotation =  timeFullRotationEarth * this.planet.period / 365;// La terre fait une rotation complète en 10s, les autres planètes scale en fonction
        float fullRotationSpeed = 360 / timeFullRotation; // degrées par seconde

        float timeAxisRotation = timeAxisRotationEarth * this.planet.rotation_days / 365;
        float axisRotationSpeed = 360 / timeAxisRotation;



        gameObject.transform.Rotate(gameObject.transform.up * axisRotationSpeed * Time.deltaTime);


        Vector3 axisRotation = parent != null ? parent.transform.position : Vector3.zero;

        gameObject.transform.RotateAround(axisRotation, Vector3.up, fullRotationSpeed * Time.deltaTime);

    }

    void Update()
    {
    }

    internal Vector3 GetPosCam()
    {
        posCam = gameObject.transform.position;
        posCam.z = gameObject.transform.position.z + distance;
        return posCam;
    }
}
