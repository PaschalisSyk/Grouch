using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Node : MonoBehaviour
{
    public bool isTaken;

    public Pawn Pawn;

    [SerializeField] GameObject _gameObject;

    private void Start()
    {

    }

    private void Update()
    {
        SetLayerOnAll(this.gameObject , gameObject.layer);
        CheckLayer();
    }
    public void SetLayerOnAll(GameObject obj, int layer)
    {
        foreach (Transform trans in obj.GetComponentsInChildren<Transform>(true))
        {
            trans.gameObject.layer = layer;
        }
    }

    void CheckLayer()
    {
        if (gameObject.layer == 9)
        {
            _gameObject.SetActive(true);
            //transform.DOScale(new Vector3(0.8f, 0.1f, 0.8f), 2).SetEase(Ease.Unset).SetLoops(-1, LoopType.Yoyo);
        }
        if(gameObject.layer == 7)
        {
            _gameObject.SetActive(false);
            //transform.DOScale(new Vector3(0.6f,0.1f,0.6f), 0.5f);
        }

    }

    /*static void SetLayerOnAllRecursive(GameObject obj, int layer)
    {
        obj.layer = layer;
        foreach (Transform child in obj.transform)
        {
            SetLayerOnAllRecursive(child.gameObject, layer);
        }
    }*/
}
