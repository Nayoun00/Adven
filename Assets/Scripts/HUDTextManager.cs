using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class HUDTextManager : MonoBehaviour
{
    public Vector3 offset;

    public GameObject HudTextUp;
    public GameObject canvasObject;

    public void UpdateHUDTextSet(string newText, GameObject target, Vector3 TargetOffset)
    {
        Vector3 TargetPosition = target.transform.position;
        Vector3 screenPosition = Camera.main.WorldToScreenPoint(TargetPosition);         //3D Position -> 2D 
        GameObject temp = (GameObject)Instantiate(HudTextUp);

        temp.transform.SetParent(canvasObject.transform, false);
        temp.transform.position = screenPosition + TargetOffset;
        temp.GetComponent<HUDMove>().textUI.text = newText;
        temp.transform.DOPunchScale(new Vector3(1.1f, 1.1f, 1.1f), 0.1f);
    }
}