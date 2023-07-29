using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class HexColorChange : MonoBehaviour
{
    [ColorUsage(true, true)]
    [SerializeField] private Color[] _color;
    private Color color;
    MeshRenderer _renderer;
    float intensity = 4f;
    // Start is called before the first frame update
    void Start()
    {
        _renderer = GetComponent<MeshRenderer>();
        InvokeRepeating("ColorChange", 0, 5f);
        InvokeRepeating("ShakeShape", 3, 6f);
        
    }

    // Update is called once per frame
    void ColorChange()
    {
        color = _color[Random.Range(0,_color.Length)];
        _renderer.material.DOColor(color * intensity, "_EmissionColor", 3);

    }

    void ShakeShape()
    {
        transform.DOShakeScale(0.5f,0.2f,5);
    }
}
