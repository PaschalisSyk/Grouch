using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameOverMenu : MonoBehaviour
{
    public TextMeshProUGUI first;
    public TextMeshProUGUI second;
    //public TextMeshProUGUI third;

    private void Start()
    {
        first.text = "1. " + SaveSettings.winners[0];
        second.text = "2. " + SaveSettings.winners[1];
        //third.text = "3rd" + SaveSettings.winners[2];
    }

    public void ReturnButton()
    {
        SceneManager.LoadScene(0);
    }
}