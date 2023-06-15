using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;


public class Unit : BaseUnit
{
    public HUDTextManager hudTextManager;

    public List<Unit> targetList = new List<Unit>();
    public List<GameObject> UIList = new List<GameObject>();

    public Vector3 basePos;
    public Vector3 skillPos;
    public GameObject HUDNumber;

    Text uiHP;
    Text uiName;
    GameObject uiHPObject;
    GameObject uiNameObject;

    public GameManager gameManager;

    public string unitPath = "";
    public SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.FindFirstObjectByType<GameManager>();
        spriteRenderer = this.gameObject.GetComponent<SpriteRenderer>();
        basePos = transform.position;

        hudTextManager = GameObject.Find("HUDTextManager").GetComponent<HUDTextManager>();

        SetUnitUI();

        if(unitPath != "")
        {
            spriteRenderer.sprite = Resources.Load<Sprite>(unitPath);
        }
        
    }

    private void Update()
    {
        if(uiHP != null)
        {
            uiHP.text = "HP : " + currentHealth.ToString() + " / " + maxHealth.ToString();
            Vector3 screenPosition = Camera.main.WorldToScreenPoint(this.gameObject.transform.position + new Vector3(0.0f, 1.0f ,0.0f));
            uiHPObject.transform.position = screenPosition;
        }
        if (uiName != null)
        {            
            Vector3 screenPosition = Camera.main.WorldToScreenPoint(this.gameObject.transform.position + new Vector3(0.0f, -1.0f, 0.0f));
            uiNameObject.transform.position = screenPosition;
        }
    }

    public void SetUnitUI()
    {
        Vector3 TargetPosition = transform.position;
        Vector3 screenPosition = Camera.main.WorldToScreenPoint(TargetPosition);         //3D Position -> 2D 
        GameObject temp = (GameObject)Instantiate(HUDNumber);
        uiHPObject = temp;
        UIList.Add(temp);
        temp.transform.SetParent(hudTextManager.canvasObject.transform, false);
        temp.transform.position = screenPosition;
        uiHP = temp.GetComponent<Text>();

        temp = (GameObject)Instantiate(HUDNumber);
        uiNameObject = temp;
        UIList.Add(temp);
        temp.transform.SetParent(hudTextManager.canvasObject.transform, false);
        temp.transform.position = screenPosition;
        uiName = temp.GetComponent<Text>();
        uiName.text = unitName;
    }

    public IEnumerator CoAttack()
    {
        yield return new WaitForSeconds(0.3f);

        if (skill_LockOn)
        {
            yield return StartCoroutine(DoSkill((int)skillType));
            skill_LockOn = false;
        }
        else
        {
            int randIndex = Random.Range(0, targetList.Count);
            targetPos = targetList[randIndex].transform.position;
            OwnPos = transform.position;
            Sequence sequence;
            sequence = DOTween.Sequence();
            sequence.Append(transform.DOMove(targetPos, 0.1f));
            sequence.Append(transform.DOMove(OwnPos, 0.1f));
            Attack(randIndex);
            yield return new WaitForSeconds(0.3f);
        }
       
    }

    public void Attack(int randIndex)
    {
        targetList[randIndex].Damaged(GetAttackPower());
        hudTextManager.UpdateHUDTextSet(GetAttackPower().ToString(), targetList[randIndex].gameObject, new Vector3(0.0f, 2.0f, 0.0f));     
    }

    IEnumerator DoSkill(int skillNumber)
    {
        if(skillNumber == 1)
        {
            int randIndex = Random.Range(0, targetList.Count);
            targetPos = targetList[randIndex].transform.position;
            OwnPos = transform.position;
            Sequence sequence;
            sequence = DOTween.Sequence();
            sequence.Append(transform.DOMove(targetPos, 0.1f));
            sequence.Append(transform.DOMove(OwnPos, 0.1f));
            targetList[randIndex].Damaged(GetAttackPower() * 2);
            hudTextManager.UpdateHUDTextSet((GetAttackPower() * 2).ToString() , targetList[randIndex].gameObject, new Vector3(0.0f, 2.0f, 0.0f));            
        }

        if (skillNumber == 2)
        {
            int randIndex = Random.Range(0, targetList.Count);
            targetPos = targetList[randIndex].transform.position;
            OwnPos = transform.position;
            Sequence sequence;
            sequence = DOTween.Sequence();
            sequence.Append(transform.DOMove(targetPos, 0.1f));
            sequence.Append(transform.DOMove(OwnPos, 0.1f));
            targetList[randIndex].Damaged(GetAttackPower());
            hudTextManager.UpdateHUDTextSet((GetAttackPower()).ToString(), targetList[randIndex].gameObject, new Vector3(0.0f, 2.0f, 0.0f));

            randIndex = Random.Range(0, targetList.Count);
            targetPos = targetList[randIndex].transform.position;
            OwnPos = transform.position;
            sequence = DOTween.Sequence();
            sequence.Append(transform.DOMove(targetPos, 0.1f));
            sequence.Append(transform.DOMove(OwnPos, 0.1f));
            targetList[randIndex].Damaged(GetAttackPower());
            hudTextManager.UpdateHUDTextSet((GetAttackPower()).ToString(), targetList[randIndex].gameObject, new Vector3(0.0f, 2.0f, 0.0f));
        }

        if (skillNumber == 3)
        {
            int randIndex = Random.Range(0, targetList.Count);
            targetPos = targetList[randIndex].transform.position;
            OwnPos = transform.position;
            Sequence sequence;
            sequence = DOTween.Sequence();
            sequence.Append(transform.DOMove(targetPos, 0.1f));
            sequence.Append(transform.DOMove(OwnPos, 0.1f));
            targetList[randIndex].Damaged((int)(GetAttackPower() * 1.5f));
            hudTextManager.UpdateHUDTextSet(((int)(GetAttackPower() * 1.5f)).ToString(), targetList[randIndex].gameObject, new Vector3(0.0f, 2.0f, 0.0f));

            targetPos = targetList[randIndex].transform.position;
            OwnPos = transform.position;
            sequence = DOTween.Sequence();
            sequence.Append(transform.DOMove(targetPos, 0.1f));
            sequence.Append(transform.DOMove(OwnPos, 0.1f));
            targetList[randIndex].Damaged((int)(GetAttackPower() * 1.5f));
            hudTextManager.UpdateHUDTextSet(((int)(GetAttackPower() * 1.5f)).ToString(), targetList[randIndex].gameObject, new Vector3(0.0f, 2.0f, 0.0f));
        }

        yield return new WaitForSeconds(0.3f);
    }

    public void SetTarget(Unit targetUnit)
    {
        if (!targetList.Contains(targetUnit))
        {
            targetList.Add(targetUnit);
        }
    }

    public void ClearTarget()
    {
        targetList.Clear();
    }

    public void UnitDestory()
    {
        List<GameObject> UIToRemove = new List<GameObject>();

        foreach (GameObject uiGameObject in UIList)
        {            
            UIToRemove.Add(uiGameObject);
        }

        foreach (GameObject uiGameObject in UIToRemove)
        {
           
            Destroy(uiGameObject);
        }

        Destroy(this.gameObject);

    }



}
