using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class GameUI : MonoBehaviour
{
    [SerializeField] Image _img;
    // Start is called before the first frame update
    void Start()
    {
        Invoke("Fade", 1);   
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Fade()
    {
        _img.DOFade(0, 3f);
    }
}
