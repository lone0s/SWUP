using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_script : MonoBehaviour
{
    private float speed = 5.0f;  // la dur�e de la transition en secondes

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetSpeed(float newSpeed)
    {
        this.speed = newSpeed;
    }

    public void MoveToTarget(Vector3 target)
    {
        StartCoroutine(MoveToTargetCoroutine(target));
    }

    private IEnumerator MoveToTargetCoroutine(Vector3 target)
    {
        Vector3 startPosition = transform.position;  // la position actuelle de la cam�ra
        float elapsedTime = 0.0f;  // le temps �coul� depuis le d�but de la transition

        while (elapsedTime < speed)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / speed);  // la progression de la transition (entre 0 et 1)

            // Interpolation lin�aire de la position de la cam�ra
            transform.position = Vector3.Lerp(startPosition, target, t);

            yield return null;  // Attendre le prochain frame
        }

        // Fixer la position de la cam�ra � la nouvelle position
        transform.position = target;
    }
}
