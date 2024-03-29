using UnityEngine;
using Assets.DataClasses;

namespace Assets.Script
{
    
}
public class Planet_script : MonoBehaviour
{

    public Planet planet;
    public GameObject parent;
    private float timeFullRotationEarth = 10;
    private float timeAxisRotationEarth = 1000;

    void Start()
    {
        planet ??= new Planet();
        transform.position = planet.position;
        transform.localScale = new Vector3(planet.radius, planet.radius, planet.radius);
    }

    void FixedUpdate(){
        float timeFullRotation =  timeFullRotationEarth * planet.period / 365;// La terre fait une rotation complète en 10s, les autres planètes scale en fonction
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
}
