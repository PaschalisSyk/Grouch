using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

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
    bool switchingPlayer;


    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
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
                        state = States.WAITING;
                    }
                    break;
            }
        }
    }

    void DiceRoll()
    {
        int diceResult = Random.Range(1, 7);
        dice = diceResult;
        //int diceResult = 6;

        if(diceResult == 6)
        {
            // Check if the base node is not occupied from a friendly pawn. If it is check for movement. Else spawn pawn if possible.
            CheckSpawnOrMovement(diceResult);
        }

        if (diceResult <6)
        {
            PawnMovement(diceResult);
        }
        Debug.Log("Dice result" + diceResult);

    }

    IEnumerator AIPlayDelay()
    {
        yield return new WaitForSeconds(1);
        DiceRoll();
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
            movablePawns = movablePawns.OrderByDescending(x => x.value).ToList();
            movablePawns[0].StartMovement(diceResult);

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

        yield return new WaitForSeconds(1);
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
            if(!playerList[activePlayer].hasWon)
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
            state = States.WAITING;
            return;
        }

        state = States.DICE_ROLL;
    }

    public void Winning()
    {
        //Winning VFX, SFX
        playerList[activePlayer].hasWon = true;
    }
}
