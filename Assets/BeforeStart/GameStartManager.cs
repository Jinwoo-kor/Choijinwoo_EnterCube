using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameStartManager : MonoBehaviour
{
    public GameObject StartToEnterText; // "Enter를 눌러 시작" 텍스트 오브젝트
    public GameObject inGameRoot;       // 인게임 오브젝트들을 담고 있는 부모 오브젝트

    private bool gameStarted = false;   // 게임 시작 여부를 저장하는 플래그

    void Start()
    {
        // 게임 시작 전: 텍스트는 보이게, 인게임 오브젝트는 숨김
        if (StartToEnterText != null)
            StartToEnterText.SetActive(true);

        if (inGameRoot != null)
            inGameRoot.SetActive(false);
    }

    void Update()
    {
        // Enter 키를 누르면 게임 시작
        if (!gameStarted && Input.GetKeyDown(KeyCode.Return))
        {
            gameStarted = true;

            // 텍스트 숨기기
            if (StartToEnterText != null)
                StartToEnterText.SetActive(false);

            // 인게임 오브젝트 활성화
            if (inGameRoot != null)
                inGameRoot.SetActive(true);
        }
    }
}
