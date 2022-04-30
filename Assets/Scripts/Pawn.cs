using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pawn : MonoBehaviour
{
    public Route commonRoute;
    public Route finalRoute;

    public Node startPos;
    public Node currentPos;
    public Node goalNode;
    public Node foundainPos;

    public List<Node> fullPath = new List<Node>();

    int routePos;
    int startPosIndex;

    public int steps;
    int doneSteps;

    public int pawnID;
    public int value;

    bool isMoving;
    public bool hasSpawned;
    public bool canSpawn = false;

    private void Start()
    {
        startPosIndex = commonRoute.GetPosition(startPos.gameObject.transform);
        FillFullPath();
    }


    private void Update()
    {
        PawnValue();


       /*if(Input.GetKeyDown(KeyCode.Space) && !isMoving)
        {
            steps = Random.Range(1, 7);
            Debug.Log("Rolled" + steps);

            if(routePos + steps <= fullPath.Count)
            {
                StartCoroutine(Move());
            }
        } */
    }

    IEnumerator Move(int diceResult)
    {
        if(isMoving)
        {
            yield break;
        }
        isMoving = true;

        while(steps > 0)
        {
            routePos++;

            Vector3 nextPos = fullPath[routePos].gameObject.transform.position;
            while(MoveToNext(nextPos,8))
            {
                yield return null;
            }

            yield return new WaitForSeconds(0.01f);
            steps--;
            doneSteps++;
        }

        goalNode = fullPath[routePos];
        if(goalNode.isTaken && goalNode.Pawn.pawnID != this.pawnID)
        {
            goalNode.Pawn.ReturnToFountain();
        }

        currentPos.Pawn = null;
        currentPos.isTaken = false;

        goalNode.Pawn = this;
        goalNode.isTaken = true;

        currentPos = goalNode;
        goalNode = null;

        if(Win())
        {
            GameManager.instance.Winning();
        }

        if (diceResult == 6)
        {
            Invoke("ReRoll", 1);
        }
        if (diceResult < 6)
        {
            Invoke("NextPlayer", 2);
        }

        isMoving = false;
    }

    private bool MoveToNext(Vector3 target , float speed)
    {
        return target != (transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime));
    }

    void FillFullPath()
    {
        for (int i = 0; i < commonRoute.childNodeList.Count; i++)
        {
            int tempPos = startPosIndex + i;
            tempPos %= commonRoute.childNodeList.Count;

            fullPath.Add(commonRoute.childNodeList[tempPos].GetComponent<Node>());
        }

        for (int i = 0; i < finalRoute.childNodeList.Count; i++)
        {
            fullPath.Add(finalRoute.childNodeList[i].GetComponent<Node>());
        }
    }

    public void GetSpawned()
    {
        steps = 1;
        hasSpawned = true;
        routePos = 0;

        StartCoroutine(Spawn());
    }

    IEnumerator Spawn()
    {
        if (isMoving)
        {
            yield break;
        }
        isMoving = true;

        while (steps > 0)
        {
            Vector3 nextPos = fullPath[routePos].gameObject.transform.position;
            while (MoveToNext(nextPos,8))
            {
                yield return null;
            }

            yield return new WaitForSeconds(0.01f);
            steps--;
            doneSteps++;
        }

        //UPDATE THE NODES AND MANAGER
        //currentPos = commonRoute.childNodeList[0].GetComponent<Node>();


        goalNode = fullPath[routePos];

        if(goalNode.isTaken && goalNode.Pawn.pawnID != this.pawnID)
        {
            goalNode.Pawn.ReturnToFountain();
        }

        goalNode.Pawn = this;
        goalNode.isTaken = true;
        currentPos = goalNode;
        goalNode = null;

        GameManager.instance.state = GameManager.States.DICE_ROLL;

        isMoving = false;
    }

    public bool CheckPossibleMovement(int diceResult)
    {
        int tempPos = routePos + diceResult;
        if(tempPos>=fullPath.Count)
        {
            return false;
        }

        return !fullPath[tempPos].isTaken;
    }

    public bool CheckForAttack(int pawnID , int diceResult)
    {
        int tempPos = routePos + diceResult;
        if (tempPos >= fullPath.Count)
        {
            return false;
        }
        if(fullPath[tempPos].isTaken)
        {
            if(pawnID == fullPath[tempPos].Pawn.pawnID)
            {
                return false;
            }
            return true;
        }
        return false;
    }

    public void StartMovement(int diceResult)
    {
        steps = diceResult;

        StartCoroutine(Move(diceResult));
    }

    public void PawnValue()
    {
        int posMultiplier = 1;
        int spawnMultiplier = 0;
        int closeToEndMultiplier = 0;
        int disToCover = fullPath.Count - routePos * posMultiplier;
        if(!hasSpawned)
        {
            spawnMultiplier = 1;
        }
        if(disToCover<18)
        {
            closeToEndMultiplier = 1;
            
        }
        int spawnValue = 100 * spawnMultiplier;

        value = disToCover + spawnValue + (100*closeToEndMultiplier);  
    }

    /*public bool CheckForEnemiesBehind(int pawnID)
    {
        
        for (int i = 0; i < 5; i++)
        {
            int tempPos = routePos - i;
            tempPos %= commonRoute.childNodeList.Count;

            
        }

    }*/

    void ReturnToFountain()
    {
        StartCoroutine(Return());
    }

    IEnumerator Return()
    {
        routePos = 0;
        currentPos = null;
        goalNode = null;
        hasSpawned = false;
        doneSteps = 0;

        Vector3 fountainPos = foundainPos.gameObject.transform.position;
        while (MoveToNext(fountainPos , 10))
        { yield return null; }
    }

    bool Win()
    {
        for (int i = 0; i < finalRoute.childNodeList.Count; i++)
        {
            if(!finalRoute.childNodeList[i].GetComponent<Node>().isTaken)
            {
                return false;
            }
        }
        return true;
    }
    void ReRoll()
    {
        GameManager.instance.state = GameManager.States.DICE_ROLL;
    }
    void NextPlayer()
    {
        GameManager.instance.state = GameManager.States.SWITCH_PLAYER;
    }
}
