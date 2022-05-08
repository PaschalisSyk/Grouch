using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    public bool isTaken;

    public Pawn Pawn;

    private void Update()
    {
        SetLayerOnAll(this.gameObject , gameObject.layer);
    }
    public void SetLayerOnAll(GameObject obj, int layer)
    {
        foreach (Transform trans in obj.GetComponentsInChildren<Transform>(true))
        {
            trans.gameObject.layer = layer;
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
