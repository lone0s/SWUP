using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Camera_script : MonoBehaviour
{
    private float transition = 5.0f;  // la dur�e de la transition en secondes
    private bool isMove = false;
    public float x = 0;
    public float y = 0;
    public float z = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Reset()
    {
        isMove = false; // je dis d'arr�ter le mouvement
        Vector3 position = new Vector3 (x, y, z);
        transform.position = position;
    }

    public void SetTransition(float newTime)
    {
        this.transition = newTime;
    }

    public void MoveToTarget(Vector3 target)
    {
        //MoveToTargetCoroutine(target);
        StartCoroutine(ZoomMoveToTargetCoroutine(target));
    }

    private IEnumerator MoveToTargetCoroutine(Vector3 target)
    {
        if (isMove)// si la fonction est d�j� appel�
        {
            isMove = false; // je dis � l'autre d'arr�t�
        }

        isMove = true;  // je dis que je commence

        Vector3 startPosition = transform.position;  // la position actuelle de la cam�ra
        float elapsedTime = 0.0f;  // le temps �coul� depuis le d�but de la transition

        while (elapsedTime < transition && isMove)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / transition);  // la progression de la transition (entre 0 et 1)

            // Interpolation lin�aire de la position de la cam�ra
            transform.position = Vector3.Lerp(startPosition, target, t);

            yield return null;  //j'attend le prochain frame
        }

        if (isMove)//si j'ai toujours la main
        {
            // je fixe la position de la cam�ra � la nouvelle position
            transform.position = target;

            // je dis que j'ai fini
            isMove = false;
        }
    }

    private IEnumerator ZoomMoveToTargetCoroutine(Vector3 target)
    {
        if (isMove)// si la fonction est d�j� appel�
        {
            isMove = false; // je dis � l'autre d'arr�t�
        }

        isMove = true;  // je dis que je commence

        //////////////////////////////////////////////////////////////////////////////
        Vector3 middle = (target - transform.position) / 2 + transform.position;
        middle.y -= 2;

        Vector3 startPosition = transform.position;  // la position actuelle de la cam�ra
        float elapsedTime = 0.0f;  // le temps �coul� depuis le d�but de la transition

        while (elapsedTime < (transition / 2) && isMove)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / (transition / 2));  // la progression de la transition (entre 0 et 1)

            // Interpolation lin�aire de la position de la cam�ra
            transform.position = Vector3.Lerp(startPosition, middle, t);

            yield return null;  //j'attend le prochain frame
        }
        ///////////////////////////////////////////////////////////////////////////////
        
        ///////////////////////////////////////////////////////////////////////////////
        startPosition = transform.position;  // la position actuelle de la cam�ra
        elapsedTime = 0.0f;  // le temps �coul� depuis le d�but de la transition

        while (elapsedTime < (transition / 2) && isMove)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / (transition / 2));  // la progression de la transition (entre 0 et 1)

            // Interpolation lin�aire de la position de la cam�ra
            transform.position = Vector3.Lerp(startPosition, target, t);

            yield return null;  //j'attend le prochain frame
        }
        ///////////////////////////////////////////////////////////////////////////////
        

        if (isMove)//si j'ai toujours la main
        {
            // je fixe la position de la cam�ra � la nouvelle position
            transform.position = target;

            // je dis que j'ai fini
            isMove = false;
        }
    }
}
