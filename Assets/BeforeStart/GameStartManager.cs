using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameStartManager : MonoBehaviour
{
    public GameObject StartToEnterText; // "Enter�� ���� ����" �ؽ�Ʈ ������Ʈ
    public GameObject inGameRoot;       // �ΰ��� ������Ʈ���� ��� �ִ� �θ� ������Ʈ

    private bool gameStarted = false;   // ���� ���� ���θ� �����ϴ� �÷���

    void Start()
    {
        // ���� ���� ��: �ؽ�Ʈ�� ���̰�, �ΰ��� ������Ʈ�� ����
        if (StartToEnterText != null)
            StartToEnterText.SetActive(true);

        if (inGameRoot != null)
            inGameRoot.SetActive(false);
    }

    void Update()
    {
        // Enter Ű�� ������ ���� ����
        if (!gameStarted && Input.GetKeyDown(KeyCode.Return))
        {
            gameStarted = true;

            // �ؽ�Ʈ �����
            if (StartToEnterText != null)
                StartToEnterText.SetActive(false);

            // �ΰ��� ������Ʈ Ȱ��ȭ
            if (inGameRoot != null)
                inGameRoot.SetActive(true);
        }
    }
}
