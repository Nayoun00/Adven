using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageManager : MonoBehaviour
{
    public GameObject canvas;
    public Button buttonPrefab; // �������� ��ư ������
    public Transform buttonContainer; // ��ư���� ������ �����̳�
    public int[] DialogSelectIndex = new int[50];

    private int numButtons = 50; // ������ ��ư ����

    protected SceneChanger SceneChanger => SceneChanger.Instance;           //�̱��� �ҷ�����

    private void Start()
    {
        // ��ư�� ���� �� �̺�Ʈ �Լ� ���
        for (int i = 0; i < numButtons; i++)
        {
            CreateButton(i);
        }
    }

    private void CreateButton(int index)
    {
        // ��ư �ν��Ͻ� ����
        Button button = Instantiate(buttonPrefab, buttonContainer);

        // ��ư �ؽ�Ʈ ����
        button.GetComponentInChildren<Text>().text = "Stage " + (index + 1);

        // ��ư Ŭ�� �̺�Ʈ�� �Լ� ���
        button.onClick.AddListener(() => OnStageButtonClick(index));
    }

    private void OnStageButtonClick(int index)
    {
        Debug.Log("Stage " + (index + 1) + " ��ư�� Ŭ���Ǿ����ϴ�!");
        SceneChanger.currentStateNum = index + 1;
        SceneChanger.currentDialogIndexNum = DialogSelectIndex[index];
        SceneChanger.LoadGameScene();
    }
}
