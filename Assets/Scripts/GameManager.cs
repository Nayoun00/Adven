using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GameManager : BaseGameManager
{
    public enum TURNSTATE
    {
        RUNNING,
        DOTURN,
        WAIT,
        DONE
    }

    public float playerMoveTime = 2.0f;
    public float moveTimeCheck = 0.0f;

    public TURNSTATE turnState = TURNSTATE.WAIT;

    public int battleCycle = 0;

    public Unit unit;


    public override void Start()
    {
        base.Start();
    }
    public override void InitializeGame()
    {
        
        base.InitializeGame();
        InitCharacter();
    }

    public void InitCharacter()
    {
        for(int i = 0; i < 4; i++)
        {
            GameObject temp = Instantiate(unit.gameObject);

            temp.transform.position = UnitPos[i].position;
            temp.GetComponent<Unit>().unitType = BaseUnit.UNITTYPE.PLAYER;
            temp.GetComponent<Unit>().maxHealth = 100;
            temp.GetComponent<Unit>().currentHealth = 100;
            unitsPlayer.Add(temp.GetComponent<Unit>());
        }
    }

    public void InitEmeny()
    {
        for (int i = 0; i < 4; i++)
        {
            GameObject temp = Instantiate(unit.gameObject);

            temp.transform.position = UnitPos[i+4].position + new Vector3(10.0f,0.0f,0.0f);
            temp.transform.DOMove(UnitPos[i + 4].position, 1.0f);
            temp.GetComponent<Unit>().unitType = BaseUnit.UNITTYPE.ENEMY;
            unitsEnemy.Add(temp.GetComponent<Unit>());
        }
    }

    public override void Update()
    {
        if(gamestate == GAMESTATE.PLAYERMOVE)
        {
            moveTimeCheck += Time.deltaTime;

            if(playerMoveTime >= moveTimeCheck)
            {
                gamestate = GAMESTATE.BATTLEINIT;
                moveTimeCheck = 0.0f;
                DoBattleInit();
            }
        }

        if(gamestate == GAMESTATE.BATTLE && turnState == TURNSTATE.WAIT)
        {            
            if(turnOrder.Count == 0)
            {
                SortTurnOrder();                
            }
            turnState = TURNSTATE.RUNNING;           
        }

        if (gamestate == GAMESTATE.BATTLE && turnState == TURNSTATE.RUNNING)
        {
            turnState = TURNSTATE.DOTURN;
            StartCoroutine(NextTurn());
        }
    }

    public void DoBattleInit()
    {

        InitEmeny();
        StartCoroutine(CoBattleInit());
    }

    IEnumerator CoBattleInit()
    {
        yield return new WaitForSeconds(1.0f);
        gamestate = GAMESTATE.BATTLE;
        
    }

    public IEnumerator NextTurn()
    {        
        Unit nextUnit = turnOrder[0];
        turnOrder.RemoveAt(0);
        if(nextUnit.unitType == BaseUnit.UNITTYPE.PLAYER)
        {            
            nextUnit.SetTarget(unitsEnemy[0]);
        }

        if (nextUnit.unitType == BaseUnit.UNITTYPE.ENEMY)
        {
            nextUnit.SetTarget(unitsPlayer[0]);
        }

        yield return StartCoroutine(nextUnit.CoAttack());

        turnState = TURNSTATE.DONE;

        yield return new WaitForSeconds(0.1f);
        DieCheck();

        turnState = TURNSTATE.WAIT;

        if (unitsEnemy.Count == 0)
        {
            gamestate = GAMESTATE.PLAYERMOVE;
        }

        if (unitsPlayer.Count == 0)
        {
            gamestate = GAMESTATE.PLAYERLOSE;
        }

        
    }

    public void DieCheck()
    {
        List<Unit> unitsToRemove = new List<Unit>();

        foreach (Unit unit in unitsPlayer)
        {
            if (unit.GetHealth() <= 0)
            {
                unitsToRemove.Add(unit);
            }
        }

        foreach (Unit unit in unitsToRemove)
        {
            unitsPlayer.Remove(unit);
            Destroy(unit.gameObject);
        }

        foreach (Unit unit in unitsEnemy)
        {
            if (unit.GetHealth() <= 0)
            {
                unitsToRemove.Add(unit);
            }
        }

        foreach (Unit unit in unitsToRemove)
        {
            unitsEnemy.Remove(unit);
            Destroy(unit.gameObject);
        }

        foreach (Unit unit in unitsToRemove)
        {
            turnOrder.Remove(unit);
        }
    }
}
