using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseGameManager : MonoBehaviour
{
    public enum GAMESTATE
    {
        INIT,
        PLAYERMOVE,
        BATTLEINIT,
        BATTLE,
        BATTLEEND,
        PLAYERLOSE,
        PLAYERWIN,
        STAGEEND
    }

    public List<Unit> unitsPlayer;
    public List<Unit> unitsEnemy;

    public List<Unit> turnOrder;

    public Transform[] UnitPos = new Transform[8];

    public GAMESTATE gamestate = GAMESTATE.INIT;

    public virtual void Start()
    {
        InitializeGame();
    }

    public virtual void InitializeGame()
    {
        // 게임 초기화 작업 수행
        // 유닛 생성, 배치 등을 수행할 수 있음

        gamestate = GAMESTATE.PLAYERMOVE;
    }

    public virtual void Update()
    {
       
    }

    public void SortTurnOrder()
    {
        turnOrder = new List<Unit>();
        foreach (Unit unit in unitsPlayer)
        {
            turnOrder.Add(unit);
        }
        foreach (Unit unit in unitsEnemy)
        {
            turnOrder.Add(unit);
        }

        // 턴 우선순위에 따라 정렬
        turnOrder.Sort((a, b) => b.GetTurnPriority().CompareTo(a.GetTurnPriority()));
    }    
}
