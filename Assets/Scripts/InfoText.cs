using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InfoText : MonoBehaviour
{
    public static InfoText instance;

    public TextMeshProUGUI infoText;


    private void Awake()
    {
        instance = this;
        infoText.text = "";
    }

    public void Message(string text)
    {
        infoText.text = text;
    }
}
