using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class HexSelector : MonoBehaviour
{
    private void Start()
    {
        transform.DOScale(new Vector3(200,200,200), 2).SetLoops(-1, LoopType.Yoyo);
    }

    void Update()
    {

    }
}
