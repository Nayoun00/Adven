using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseUnit : MonoBehaviour
{
    public enum UNITTYPE
    {
        PLAYER,
        ENEMY,
    }


    public string unitName;

    public int unitLevel = 1;

    public int maxHealth = 10;
    public int currentHealth = 10;

    public int maxAttackPower = 2;
    public int currentAttackPower = 2;

    public int maxSpeed = 2;
    public int currentSpeed = 2;

    protected Vector3 targetPos;
    protected Vector3 OwnPos;

    public UNITTYPE unitType = UNITTYPE.PLAYER;

    public float GetTurnPriority()
    {
        // �� �켱���� ���
        float priority = currentSpeed;
        // �÷��̾� ������ ��� �߰����� �켱���� ���� ����

        return priority;
    }

    public int GetAttackPower()
    {
        return currentAttackPower;
    }

    public int GetHealth()
    {
        return currentHealth;
    }

    public void Damaged(int attackPower)
    {
        currentHealth -= attackPower;
    }

}
