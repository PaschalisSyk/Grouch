using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceLabel : MonoBehaviour
{
    public Texture2D numberOneDisplay;

    private void OnGUI()
    {
        GUI.Box(new Rect(0, 0, Screen.width, Screen.height), numberOneDisplay);
    }

    
}
