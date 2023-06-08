using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageManager : MonoBehaviour
{
    public GameObject canvas;
    public Button buttonPrefab; // 스테이지 버튼 프리팹
    public Transform buttonContainer; // 버튼들이 생성될 컨테이너
    public int[] DialogSelectIndex = new int[50];

    private int numButtons = 50; // 생성할 버튼 개수

    protected SceneChanger SceneChanger => SceneChanger.Instance;           //싱글톤 불러오기

    private void Start()
    {
        // 버튼들 생성 및 이벤트 함수 등록
        for (int i = 0; i < numButtons; i++)
        {
            CreateButton(i);
        }
    }

    private void CreateButton(int index)
    {
        // 버튼 인스턴스 생성
        Button button = Instantiate(buttonPrefab, buttonContainer);

        // 버튼 텍스트 설정
        button.GetComponentInChildren<Text>().text = "Stage " + (index + 1);

        // 버튼 클릭 이벤트에 함수 등록
        button.onClick.AddListener(() => OnStageButtonClick(index));
    }

    private void OnStageButtonClick(int index)
    {
        Debug.Log("Stage " + (index + 1) + " 버튼이 클릭되었습니다!");
        SceneChanger.currentStateNum = index + 1;
        SceneChanger.currentDialogIndexNum = DialogSelectIndex[index];
        SceneChanger.LoadGameScene();
    }
}
