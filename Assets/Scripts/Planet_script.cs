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
        this.planet = this.planet != null ? this.planet : new Planet();
        planet.radius = transform.localScale.x;
        distance *= planet.radius;
    }

    void FixedUpdate(){
        float timeFullRotation =  timeFullRotationEarth * this.planet.period / 365;// La terre fait une rotation complète en 10s, les autres planètes scale en fonction
        float fullRotationSpeed = timeFullRotation != 0 ? 360 / timeFullRotation : 0; // degrées par seconde

        float timeAxisRotation = timeAxisRotationEarth * this.planet.rotation_days / 365;
        float axisRotationSpeed = timeAxisRotation != 0 ? 360 / timeAxisRotation : 0;

        Vector3 axisRotation = parent != null ? parent.transform.position : Vector3.zero;

        gameObject.transform.Rotate(gameObject.transform.up * axisRotationSpeed * Time.deltaTime);
        gameObject.transform.RotateAround(axisRotation, Vector3.up, fullRotationSpeed * Time.deltaTime);

    }

    void Update()
    {
    }

    internal Vector3 GetPosCam()
    {
        float child = parent != null ? 2 : 1;
        posCam = transform.position;
        posCam.z = transform.position.z + distance * child;
        return posCam;
        
    }
}
