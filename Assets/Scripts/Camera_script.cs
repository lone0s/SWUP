using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Camera_script : MonoBehaviour
{
    private float transition = 5.0f;  // la durée de la transition en secondes
    private bool isMove = false;
    private Vector3 initPos;
    private Planet_script objet_followed;
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

    private void FixedUpdate()
    {
        if (!isMove)
            if(objet_followed != null)
                transform.position = objet_followed.GetPosCam();
    }

    public void Reset()
    {
        isMove = false; // je dis d'arrêter le mouvement
        transform.position = initPos;
        objet_followed = null;
    }

    public void setObjetFollowed(GameObject objet)
    {
        objet_followed = objet.GetComponent<Planet_script>();
    }

    public void SetTransition(float newTime)
    {
        this.transition = newTime;
    }

    public void SetMove(int value)
    {
        this.move = value;
    }

    public void MoveToTarget(GameObject objet)
    {
        setObjetFollowed(objet);

        switch (move)
        {
            case 0:
                StartCoroutine(MoveToTargetCoroutine());
                break;
            case 1:
                StartCoroutine(ZoomMoveToTargetCoroutine());
                break;
            default:
                break;
        }        
    }

    private IEnumerator MoveToTargetCoroutine()
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
            transform.position = Vector3.Lerp(startPosition, objet_followed.GetPosCam(), t);

            yield return null;  //j'attend le prochain frame
        }

        if (isMove)//si j'ai toujours la main
            // je dis que j'ai fini
            isMove = false;
    }

    private IEnumerator ZoomMoveToTargetCoroutine()
    {
        if (isMove)// si la fonction est déjà appelé
        {
            isMove = false; // je dis à l'autre d'arrêté
            yield return null;  //j'attend le prochain frame
        }

        isMove = true;  // je dis que je commence

        //////////////////////////////////////////////////////////////////////////////
        Vector3 startPosition = transform.position;  // la position actuelle de la caméra
        float elapsedTime = 0.0f;  // le temps écoulé depuis le début de la transition

        while (elapsedTime < (transition / 2) && isMove)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / (transition / 2));  // la progression de la transition (entre 0 et 1)

            // Interpolation linéaire de la position de la caméra
            transform.position = Vector3.Lerp(startPosition, initPos, t);

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
            transform.position = Vector3.Lerp(startPosition, objet_followed.GetPosCam(), t);

            yield return null;  //j'attend le prochain frame
        }
        ///////////////////////////////////////////////////////////////////////////////
        

        if (isMove)//si j'ai toujours la main
            // je dis que j'ai fini
            isMove = false;
    }
}
