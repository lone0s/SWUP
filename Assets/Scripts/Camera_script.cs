using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Camera_script : MonoBehaviour
{
    private float transition = 5.0f;  // la durée de la transition en secondes
    private bool isMove = false;
    private Vector3 initPos;
    private int move;

    // Start is called before the first frame update
    void Start()
    {
        initPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Reset()
    {
        isMove = false; // je dis d'arrêter le mouvement
        transform.position = initPos;
    }

    public void SetTransition(float newTime)
    {
        this.transition = newTime;
    }

    public void SetMove(int value)
    {
        this.move = value;
    }

    public void MoveToTarget(Vector3 target)
    {
        switch (move)
        {
            case 0:
                StartCoroutine(MoveToTargetCoroutine(target));
                break;
            case 1:
                StartCoroutine(ZoomMoveToTargetCoroutine(target));
                break;
            default:
                break;
        }        
    }

    private IEnumerator MoveToTargetCoroutine(Vector3 target)
    {
        if (isMove)// si la fonction est déjà appelé
        {
            isMove = false; // je dis à l'autre d'arrêté
            yield return null;  //j'attend le prochain frame
        }

        isMove = true;  // je dis que je commence

        Vector3 startPosition = transform.position;  // la position actuelle de la caméra
        float elapsedTime = 0.0f;  // le temps écoulé depuis le début de la transition

        while (elapsedTime < transition && isMove)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / transition);  // la progression de la transition (entre 0 et 1)

            // Interpolation linéaire de la position de la caméra
            transform.position = Vector3.Lerp(startPosition, target, t);

            yield return null;  //j'attend le prochain frame
        }

        if (isMove)//si j'ai toujours la main
        {
            // je fixe la position de la caméra à la nouvelle position
            transform.position = target;

            // je dis que j'ai fini
            isMove = false;
        }
    }

    private IEnumerator ZoomMoveToTargetCoroutine(Vector3 target)
    {
        if (isMove)// si la fonction est déjà appelé
        {
            isMove = false; // je dis à l'autre d'arrêté
            yield return null;  //j'attend le prochain frame
        }

        isMove = true;  // je dis que je commence

        //////////////////////////////////////////////////////////////////////////////
        Vector3 middle = (target - transform.position) / 2 + transform.position;
        middle.z -= 2;

        Vector3 startPosition = transform.position;  // la position actuelle de la caméra
        float elapsedTime = 0.0f;  // le temps écoulé depuis le début de la transition

        while (elapsedTime < (transition / 2) && isMove)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / (transition / 2));  // la progression de la transition (entre 0 et 1)

            // Interpolation linéaire de la position de la caméra
            transform.position = Vector3.Lerp(startPosition, middle, t);

            yield return null;  //j'attend le prochain frame
        }
        ///////////////////////////////////////////////////////////////////////////////
        
        ///////////////////////////////////////////////////////////////////////////////
        startPosition = transform.position;  // la position actuelle de la caméra
        elapsedTime = 0.0f;  // le temps écoulé depuis le début de la transition

        while (elapsedTime < (transition / 2) && isMove)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / (transition / 2));  // la progression de la transition (entre 0 et 1)

            // Interpolation linéaire de la position de la caméra
            transform.position = Vector3.Lerp(startPosition, target, t);

            yield return null;  //j'attend le prochain frame
        }
        ///////////////////////////////////////////////////////////////////////////////
        

        if (isMove)//si j'ai toujours la main
        {
            // je fixe la position de la caméra à la nouvelle position
            transform.position = target;

            // je dis que j'ai fini
            isMove = false;
        }
    }
}
