using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [System.Serializable]
    public class Agent
    {
        public string playerName;
        public Pawn[] pawns;
        public bool hasTurn;
        public bool hasMove;
        public enum PlayerTypes
        {
            HUMAN,
            AI,
            NO_PLAYER
        }
        public PlayerTypes playerType;
        public bool hasWon;
    }

    public List<Agent> playerList = new List<Agent>();

    public enum States
    {
        WAITING,
        DICE_ROLL,
        SWITCH_PLAYER
    }

    public States state;

    public int activePlayer;
    public string playerID;
    public int dice;
    public bool switchingPlayer;

    public int playerDiceResult;
    public GameObject button;
    public GameObject diceCube;
    GameObject _dice;
    DiceAnim diceAnim;
    public bool diceOn;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        diceAnim = diceCube.GetComponent<DiceAnim>();
        diceOn = true;

        ActivateDice(false);
        //button.SetActive(false);

        StartCoroutine(IntroDelay());

    }


    private void Update()
    {
        if(Time.timeSinceLevelLoad < 4)
        {
            return;
        }

        playerID = playerList[activePlayer].playerName;
        if (playerList[activePlayer].playerType == Agent.PlayerTypes.AI)
        {
            switch (state)
            {
                case States.DICE_ROLL:
                    {
                        StartCoroutine(AIPlayDelay());
                        state = States.WAITING;
                    }
                    break;

                case States.WAITING:
                    {

                    }
                    break;

                case States.SWITCH_PLAYER:
                    {
                        StartCoroutine(SwitchPlayer());
                        Invoke("DiceScale", 1);

                        state = States.WAITING;
                    }
                    break;
            }
        }

        if (playerList[activePlayer].playerType == Agent.PlayerTypes.HUMAN)
        {
            switch (state)
            {
                case States.DICE_ROLL:
                    {
                        StartCoroutine(HumanDelay());
                        HumanDiceRoll(diceOn);
                        state = States.WAITING;
                    }
                    break;

                case States.WAITING:
                    {
                        if (Input.GetKeyDown(KeyCode.G))
                        {
                            playerDiceResult = 6;
                        }
                    }
                    break;

                case States.SWITCH_PLAYER:
                    {
                        DeactivateInput();
                        button.SetActive(false);

                        StartCoroutine(SwitchPlayer());
                        Invoke("DiceScale", 1);


                        state = States.WAITING;
                    }
                    break;
            }
        }

        if(Input.GetKeyDown(KeyCode.Backspace))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }

    void DiceRoll()
    {
        int diceResult = Random.Range(1, 7);
        dice = diceResult;


        //int diceResult = 6;

        if (diceResult == 6)
        {
            // Check if the base node is not occupied from a friendly pawn. If it is check for movement. Else spawn pawn if possible.
            CheckSpawnOrMovement(diceResult);
            Invoke("DiceScale", 1);

        }

        if (diceResult <6)
        {
            PawnMovement(diceResult);
        }


    }

    IEnumerator AIPlayDelay()
    {
        yield return new WaitForSeconds(1);
        DiceScale();
        DiceRoll();
        _dice = Instantiate(diceCube,diceCube.transform.position, transform.rotation);
        DiceText(dice);

    }

    void CheckSpawnOrMovement(int diceResult)
    {
        Agent currentAgent = playerList[activePlayer];
        bool baseNodeFull = false;
        for (int i = 0; i < currentAgent.pawns.Length; i++)
        {
            //CHECK IF START NODE IS FULL
            if (currentAgent.pawns[i].currentPos == currentAgent.pawns[i].startPos)
            {
                baseNodeFull = true;
                break;
            }
        }
        if (baseNodeFull)   //BASE NODE OCCUPIED
        {
            Debug.Log("START NODE FULL");

            PawnMovement(diceResult);

            /*if (!currentAgent.hasMove)
            {
                state = States.SWITCH_PLAYER;
                return;
            }*/

        }
        else
        {

            for (int i = 0; i < currentAgent.pawns.Length; i++)     //IF PAWNS IN FOUNTAIN > 0
            {
                if (!currentAgent.pawns[i].hasSpawned)
                {
                    currentAgent.pawns[i].GetSpawned();             //SPAWN PAWN
                    state = States.WAITING;
                    return;
                }
            }

            PawnMovement(diceResult);
        }
    }

    void PawnMovement(int diceResult)
    {
        Agent currentAgent = playerList[activePlayer];

        List<Pawn> movablePawns = new List<Pawn>();


        for (int i = 0; i < currentAgent.pawns.Length; i++)
        {
            if(currentAgent.pawns[i].hasSpawned)
            {
                if (currentAgent.pawns[i].CheckForAttack(currentAgent.pawns[i].pawnID, diceResult))
                {
                    movablePawns.Add(currentAgent.pawns[i]);
                    currentAgent.pawns[i].value += 100;
                }
                else if(currentAgent.pawns[i].CheckPossibleMovement(diceResult))
                {
                    movablePawns.Add(currentAgent.pawns[i]);
                    currentAgent.pawns[i].value += 20;
                }
            }
        }

        if(movablePawns.Count > 0)
        {
            movablePawns.OrderByDescending(x => x.value).FirstOrDefault().StartMovement(diceResult);
            //movablePawns[0].StartMovement(diceResult);

            //int num = Random.Range(0, movablePawns.Count);
            //movablePawns[num].StartMovement(diceResult);

            currentAgent.hasMove = true;
            //state = States.SWITCH_PLAYER;
            return;
        }
        if (movablePawns.Count < 1 && diceResult == 6)
        {
            currentAgent.hasMove = false;
            state = States.DICE_ROLL;
        }
        else state = States.SWITCH_PLAYER;
    }

    IEnumerator SwitchPlayer()
    {
        if(switchingPlayer)
        {
            yield break;
        }

        switchingPlayer = true;

        yield return new WaitForSeconds(2);
        SetNextAgent();

        switchingPlayer = false;
    }

    void SetNextAgent()
    {
        activePlayer++;
        activePlayer %= playerList.Count;

        int stillPlaying = 0;

        for (int i = 0; i < playerList.Count; i++)
        {
            if(!playerList[i].hasWon)
            {
                stillPlaying++;
            }
        }

        if(playerList[activePlayer].hasWon && stillPlaying>2)
        {
            SetNextAgent();
            return;
        }
        else if(stillPlaying<3)
        {
            //GAME OVER SCREEN
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            state = States.WAITING;
            return;
        }

        InfoText.instance.Message(playerList[activePlayer].playerName + "'s turn!");
        state = States.DICE_ROLL;
    }

    public void Winning()
    {
        //Winning VFX, SFX
        playerList[activePlayer].hasWon = true;
        for (int i = 0; i < SaveSettings.winners.Length; i++)
        {
            if(SaveSettings.winners[i] == "")
            {
                SaveSettings.winners[i] = playerList[activePlayer].playerName;
                break;
            }
        }

    }

    IEnumerator IntroDelay()
    {

        int randomPlayer = Random.Range(0, playerList.Count);
        activePlayer = randomPlayer;
        InfoText.instance.Message(playerList[activePlayer].playerName + " starts first");
        yield return new WaitForSeconds(3);
    }

    /*public void DestroyDice()
    {
        _dice.GetComponent<DiceAnim>().DestroyCube();
    }*/

    public void DiceScale()
    {

        if (_dice != null)
        {
            _dice.GetComponent<DiceAnim>().Scale();

        }
    }

    void DiceText(int diceResult)
    {
        Debug.Log("Dice result" + diceResult);
        InfoText.instance.Message(playerList[activePlayer].playerName + " has rolled " + diceResult);
    }

    #region Human Input

    void HumanDiceRoll(bool diceBool)
    {
        diceBool = diceOn;
        if(!diceBool)
        {
            return;
        }
        ActivateDice(true);
        dice = playerDiceResult;

        //playerDiceResult = 6;
        Agent currentAgent = playerList[activePlayer];

        List<Pawn> movablePawns = new List<Pawn>();

        bool baseNodeFull = false;
        for (int i = 0; i < currentAgent.pawns.Length; i++)
        {
            //CHECK IF START NODE IS FULL
            if (currentAgent.pawns[i].currentPos == currentAgent.pawns[i].startPos)
            {
                baseNodeFull = true;
                break;
            }
        }

        if(playerDiceResult < 6)
        {
            Debug.Log("1");
            movablePawns.AddRange(PossiblePawns());
            
        }


        if(playerDiceResult == 6)
        {
            if (!baseNodeFull)
            {
                for (int i = 0; i < currentAgent.pawns.Length; i++)
                {
                    if (!currentAgent.pawns[i].hasSpawned)
                    {
                        movablePawns.Add(currentAgent.pawns[i]);
                    }
                }
                movablePawns.AddRange(PossiblePawns());

            }
            else if (baseNodeFull)
            {

                movablePawns.AddRange(PossiblePawns());

            }
        }

        if (movablePawns.Count < 1)
        {
            Invoke("Button", 1);

            Debug.Log("2");
            //state = States.SWITCH_PLAYER;
        }

        if (movablePawns.Count > 0)
        {
            for (int i = 0; i < movablePawns.Count; i++)
            {
                movablePawns[i].HasTurn(true);
                Debug.Log("3");
            }
        }
        if (movablePawns.Count == 0 && playerDiceResult == 6)
        {
            currentAgent.hasMove = false;
            state = States.DICE_ROLL;
            Invoke("DiceScale", 1);

        }
        else
        {
            currentAgent.hasMove = false;
            state = States.SWITCH_PLAYER;
            //button.SetActive(true);
            Debug.Log("4");
        }

        Invoke("PlayerDiceText", 1);

        Debug.Log("5");
    }

    List <Pawn> PossiblePawns()
    {
        Agent currentAgent = playerList[activePlayer];

        List<Pawn> tempList = new List<Pawn>();

        for (int i = 0; i < currentAgent.pawns.Length; i++)
        {
            if (currentAgent.pawns[i].hasSpawned)
            {
                if (currentAgent.pawns[i].CheckForAttack(currentAgent.pawns[i].pawnID, playerDiceResult))
                {
                    tempList.Add(currentAgent.pawns[i]);
                    //continue;
                }
                if (currentAgent.pawns[i].CheckPossibleMovement(playerDiceResult))
                {
                    tempList.Add(currentAgent.pawns[i]);
                }
            }
        }

        return tempList;
    }

    void ActivateDice(bool on)
    {
        if(on)
        {
            //DiceScale();
            playerDiceResult = Random.Range(1, 7);
        }
        if(playerDiceResult == 6)
        {
            diceOn = false;
        }

    }

    public void DeactivateInput()
    {
        for (int i = 0; i < playerList.Count; i++)
        {
            for (int j = 0; j < playerList[i].pawns.Length; j++)
            {
                playerList[i].pawns[j].HasTurn(false);
            }
        }
    }

    public void Button()
    {
        state = States.SWITCH_PLAYER;
        Invoke("DiceScale",1);
    }

    void PlayerDiceText()
    {
        InfoText.instance.Message("You have rolled " + playerDiceResult);
    }

    IEnumerator HumanDelay()
    {
        yield return new WaitForSeconds(1);
        _dice = Instantiate(diceCube, diceCube.transform.position, transform.rotation);

    }

    void SetButtonActive()
    {
        button.SetActive(true);
     
    }

    #endregion
}
