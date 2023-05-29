using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public class Unit : BaseUnit
{
    public HUDTextManager hudTextManager;

    public List<Unit> targetList = new List<Unit>();
 
    public Vector3 basePos;
    public Vector3 skillPos;

    // Start is called before the first frame update
    void Start()
    {
        basePos = transform.position;

        hudTextManager = GameObject.Find("HUDTextManager").GetComponent<HUDTextManager>();

    }


    public IEnumerator CoAttack()
    {

        if(targetList[0] == null)
        {
            targetList.RemoveAt(0);
        }

        targetPos = targetList[0].transform.position;



        OwnPos = transform.position;

        Sequence sequence;
        sequence = DOTween.Sequence();
        sequence.Append(transform.DOMove(targetPos, 0.1f));
        sequence.Append(transform.DOMove(OwnPos, 0.1f));
        Attack();
        yield return new WaitForSeconds(0.3f);
    }

    public void Attack()
    {
        foreach (Unit unit in targetList)
        {
            unit.Damaged(GetAttackPower());
            hudTextManager.UpdateHUDTextSet(GetAttackPower().ToString(), unit.gameObject, new Vector3(0.0f, 2.0f, 0.0f));
        }
    }

    public void SetTarget(Unit targetUnit)
    {
        if (!targetList.Contains(targetUnit))
        {
            targetList.Add(targetUnit);
        }

    }



}
