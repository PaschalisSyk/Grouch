using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BoardLineColor : MonoBehaviour
{

    LineRenderer lineRenderer;

    [ColorUsage(true, true)]
    [SerializeField] Color[] color;
    [SerializeField] Material emissiveMat;
    //[SerializeField] Renderer objectRenderer;

    private Color _color;
    private float intensity =10f;


    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        emissiveMat = lineRenderer.material;
        //LineColorUpdate(GameManager.instance.activePlayer);

    }

    // Update is called once per frame
    void Update()
    {
        LineColorUpdate(GameManager.instance.activePlayer);
    }

    public void LineColorUpdate(int colorNum)
    {

        _color = color[colorNum];
        emissiveMat.DOColor(_color * intensity, "_EmissionColor", 1.5f);
        
    }
}
