using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class GameManager : BaseGameManager
{
    protected SceneChanger SceneChanger => SceneChanger.Instance;           //싱글톤 불러오기
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

    public int StageNum = 100;
    public Entity_EnemyData enemyData;
    public Entity_PlayerData PlayerData;
    public Text uiStage;

    public DialogSystem dialogSystem;
    public int dialogIndex;

    public override void Start()
    {
        if(SceneChanger.currentStateNum == 0)
        {
            uiStage.text = "테스트 스테이지";
            uiStage.DOFade(0f, 1.0f).OnComplete(() =>
            {
                // 애니메이션이 완료되면 UI Text를 비활성화합니다.
                uiStage.gameObject.SetActive(false);
            });
            StageNum = 100;
        }
        else
        {        

            uiStage.text = "Stage " + SceneChanger.currentStateNum.ToString();
            uiStage.DOFade(0f, 3.0f).OnComplete(() =>
            {
                // 애니메이션이 완료되면 UI Text를 비활성화합니다.
                uiStage.gameObject.SetActive(false);
            });
            StageNum = SceneChanger.currentStateNum * 100;
        }

      

        //base.Start();
        dialogIndex = SceneChanger.currentDialogIndexNum;
        StartCoroutine(StartDialog());
    }

    private IEnumerator StartDialog()
    {
        yield return new WaitUntil(() => dialogSystem.UpdateDialog(dialogIndex, true)); //기다리는 함수 , 다이얼로그 시스템이 완료 될때 까지 
        InitializeGame();
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

            Entity_PlayerData.Param desiredParam = PlayerData.sheets[0].list.Find(param => param.index == i + 1);

            temp.transform.position = UnitPos[i].position;
            temp.GetComponent<Unit>().unitType = BaseUnit.UNITTYPE.PLAYER;
            temp.GetComponent<Unit>().maxHealth = desiredParam.hp;
            temp.GetComponent<Unit>().currentHealth = desiredParam.hp;
            temp.GetComponent<Unit>().maxAttackPower = desiredParam.attack;
            temp.GetComponent<Unit>().currentAttackPower = desiredParam.attack;
            temp.GetComponent<Unit>().maxSpeed = desiredParam.speed;
            temp.GetComponent<Unit>().currentSpeed = desiredParam.speed;
            temp.GetComponent<Unit>().unitName = desiredParam.name;

            unitsPlayer.Add(temp.GetComponent<Unit>());
        }
    }

    public void InitEmeny()
    {
        for (int i = 0; i < 4; i++)
        {
            GameObject temp = Instantiate(unit.gameObject);

            int newindex = StageNum + roundIndex * 10 + i + 1;
            Debug.Log("Enemy Index : " + newindex);
            // 해당하는 index의 데이터 가져오기
            Entity_EnemyData.Param desiredParam = enemyData.sheets[0].list.Find(param => param.index == newindex);
            
            temp.transform.position = UnitPos[i+4].position + new Vector3(10.0f,0.0f,0.0f);
            temp.transform.DOMove(UnitPos[i + 4].position, 1.0f);
            temp.GetComponent<Unit>().unitType = BaseUnit.UNITTYPE.ENEMY;
            temp.GetComponent<Unit>().maxHealth = desiredParam.hp;
            temp.GetComponent<Unit>().currentHealth = desiredParam.hp;
            temp.GetComponent<Unit>().maxAttackPower = desiredParam.attack;
            temp.GetComponent<Unit>().currentAttackPower = desiredParam.attack;
            temp.GetComponent<Unit>().maxSpeed = desiredParam.speed;
            temp.GetComponent<Unit>().currentSpeed = desiredParam.speed;
            temp.GetComponent<Unit>().unitName = desiredParam.name;
            
            unitsEnemy.Add(temp.GetComponent<Unit>());
        }
    }

    public override void Update()
    {
        if(gamestate == GAMESTATE.PLAYERMOVE)
        {
            moveTimeCheck += Time.deltaTime;

            if(playerMoveTime <= moveTimeCheck)
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

        if (gamestate == GAMESTATE.ROUNDEND)
        {
            RoundEndCheck();
        }

        if (gamestate == GAMESTATE.STAGEENDINIT)
        {
            SceneChanger.LoadStageScene();
            gamestate = GAMESTATE.STAGEENDDONE;
        }
    }

    public void RoundEndCheck()
    {
        roundIndex += 1;

        if(roundIndex > 4)
        {
            gamestate = GAMESTATE.STAGEENDINIT;
        }
        else
        {
            gamestate = GAMESTATE.PLAYERMOVE;
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
        nextUnit.ClearTarget();
        turnOrder.RemoveAt(0);
        if (nextUnit.unitType == BaseUnit.UNITTYPE.PLAYER)
        {
            
            for (int i = 0; i < unitsEnemy.Count; i++)
            {
                nextUnit.SetTarget(unitsEnemy[i]);
            }
          
        }

        if (nextUnit.unitType == BaseUnit.UNITTYPE.ENEMY)
        {
            for (int i = 0; i < unitsPlayer.Count; i++)
            {
                nextUnit.SetTarget(unitsPlayer[i]);
            }
        }

        yield return StartCoroutine(nextUnit.CoAttack());

        turnState = TURNSTATE.DONE;

        yield return new WaitForSeconds(0.1f);
        DieCheck();

        turnState = TURNSTATE.WAIT;

        if (unitsEnemy.Count == 0)
        {
            gamestate = GAMESTATE.ROUNDEND;
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
            unit.UnitDestory();
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
            unit.UnitDestory();
        }

        foreach (Unit unit in unitsToRemove)
        {
            turnOrder.Remove(unit);
        }
    }
}
