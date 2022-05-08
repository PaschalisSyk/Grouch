using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TestText : MonoBehaviour
{
    public TextMeshProUGUI text;
    string pawnId;
    int dice;
    int humandice;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        pawnId = GameManager.instance.playerID;
        dice = GameManager.instance.dice;
        humandice = GameManager.instance.playerDiceResult;
        if (pawnId == "P1")
        {
            dice = humandice;
        }
        text.text = pawnId + " D-" + dice;
    }
}
