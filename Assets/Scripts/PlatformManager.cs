using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformManager : MonoBehaviour
{
    [Header("케이스 위치들")]
    public Vector3[] casePositions = new Vector3[4]
    {
        new Vector3(10f, 5f, 0f),
        new Vector3(-10f, 5f, 0f),
        new Vector3(0f, 5f, 10f),
        new Vector3(0f, 5f, -10f)
    };

    [Header("스폰 포인트")]
    public Transform spawnPoint;

    [Header("플랫폼 생성기")]
    public PlatformSpawner platformSpawner;

    private int currentCaseIndex = -1;
    private GameObject nextStagePlatform;
    private int score = 0;
    public List<int> colorStack = new List<int>();

    void Start()
    {
        spawnPoint.position = Vector3.zero;
        platformSpawner.Spawn(spawnPoint, "Platform", false); // 중심 플랫폼 4개 생성
        SpawnCasePlatforms(); // 공중 플랫폼 4세트 생성
    }

    public IEnumerator TransitionToNextStage()
    {
        foreach (GameObject platform in platformSpawner.spawnedPlatforms)
        {
            if (platform != null)
            {
                Rigidbody rb = platform.GetComponent<Rigidbody>();
                if (rb == null) rb = platform.AddComponent<Rigidbody>();
                rb.useGravity = true;
                rb.isKinematic = false;
                Destroy(platform, 3f);
            }
        }
        platformSpawner.spawnedPlatforms.Clear();

        yield return StartCoroutine(MoveSelectedPlatformToCenter());

        score += 10;
        Debug.Log("점수 증가! 현재 점수: " + score);

        ResetStack();
        SpawnCasePlatforms();
    }

    private IEnumerator MoveSelectedPlatformToCenter()
    {
        if (nextStagePlatform == null)
        {
            Debug.LogWarning("다음 스테이지 플랫폼이 없습니다.");
            yield break;
        }

        float duration = 0.5f;
        float timer = 0f;
        Vector3 startPos = nextStagePlatform.transform.position;

        while (timer < duration)
        {
            float t = timer / duration;
            nextStagePlatform.transform.position = Vector3.Lerp(startPos, Vector3.zero, t);
            timer += Time.deltaTime;
            yield return null;
        }

        nextStagePlatform.transform.position = Vector3.zero;
        nextStagePlatform.tag = "Platform";

        Platform p = nextStagePlatform.GetComponent<Platform>();
        if (p != null)
        {
            p.isLocked = false;
            p.colorChangeInterval = 2f;
        }

        platformSpawner.spawnedPlatforms.Add(nextStagePlatform);
        nextStagePlatform = null;
    }

    public void AddColorToStack(int colorCode)
    {
        colorStack.Add(colorCode);
        if (colorStack.Count == 4)
        {
            if (IsStackMatched())
            {
                Debug.Log("색상 일치 성공! Enter 키로 다음 스테이지로 이동 가능");
            }
            else
            {
                Debug.Log("색상 불일치! 초기화");
                ResetAllPlatforms();
            }
        }
    }

    public bool IsStackMatched()
    {
        int first = colorStack[0];
        foreach (int color in colorStack)
        {
            if (color != first) return false;
        }
        return true;
    }

    public void ResetAllPlatforms()
    {
        colorStack.Clear();
        foreach (GameObject platform in platformSpawner.spawnedPlatforms)
        {
            if (platform != null)
            {
                Platform p = platform.GetComponent<Platform>();
                if (p != null)
                {
                    p.isLocked = false;
                    p.colorChangeInterval = 2f;
                }
            }
        }
    }

    public void ResetStack()
    {
        colorStack.Clear();
    }

    public void SpawnCasePlatforms()
    {
        GameObject[] oldCases = GameObject.FindGameObjectsWithTag("CasePlatform");
        foreach (GameObject obj in oldCases)
        {
            Destroy(obj);
        }

        List<GameObject> casePlatforms = new List<GameObject>();
        foreach (Vector3 pos in casePositions)
        {
            GameObject tempSpawn = new GameObject("TempSpawn");
            tempSpawn.transform.position = pos;
            platformSpawner.Spawn(tempSpawn.transform, "CasePlatform", false);
            casePlatforms.AddRange(platformSpawner.spawnedPlatforms);
        }

        int nextCaseIndex = Random.Range(0, casePlatforms.Count);
        currentCaseIndex = nextCaseIndex;
        nextStagePlatform = casePlatforms[nextCaseIndex];
    }
}
