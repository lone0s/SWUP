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
    private float timeFullRotationEarth = 10;

    void Start()
    {
    }

    void FixedUpdate(){
        float timeRotation =  timeFullRotationEarth * this.planet.period / 365;// La terre fait une rotation complète en 10s, les autres planètes scale en fonction
        float rotationSpeed = 360 / timeRotation; // degrees par seconde

        gameObject.transform.Rotate(gameObject.transform.up * rotationSpeed * Time.deltaTime);
        gameObject.transform.RotateAround(Vector3.zero, Vector3.up, rotationSpeed * Time.deltaTime);

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
