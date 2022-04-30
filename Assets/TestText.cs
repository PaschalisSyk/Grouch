using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TestText : MonoBehaviour
{
    public TextMeshProUGUI text;
    string pawnId;
    int dice;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        pawnId = GameManager.instance.playerID;
        dice = GameManager.instance.dice;
        text.text = pawnId + " D-" + dice;
    }
}
