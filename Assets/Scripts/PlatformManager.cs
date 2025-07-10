using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformManager : MonoBehaviour
{
    [Header("���̽� ��ġ��")]
    public Vector3[] casePositions = new Vector3[4]
    {
        new Vector3(10f, 5f, 0f),
        new Vector3(-10f, 5f, 0f),
        new Vector3(0f, 5f, 10f),
        new Vector3(0f, 5f, -10f)
    };

    [Header("���� ����Ʈ")]
    public Transform spawnPoint;

    [Header("�÷��� ������")]
    public PlatformSpawner platformSpawner;

    private int currentCaseIndex = -1;
    private GameObject nextStagePlatform;
    private int score = 0;
    public List<int> colorStack = new List<int>();

    void Start()
    {
        spawnPoint.position = Vector3.zero;
        platformSpawner.Spawn(spawnPoint, "Platform", false); // �߽� �÷��� 4�� ����
        SpawnCasePlatforms(); // ���� �÷��� 4��Ʈ ����
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
        Debug.Log("���� ����! ���� ����: " + score);

        ResetStack();
        SpawnCasePlatforms();
    }

    private IEnumerator MoveSelectedPlatformToCenter()
    {
        if (nextStagePlatform == null)
        {
            Debug.LogWarning("���� �������� �÷����� �����ϴ�.");
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
                Debug.Log("���� ��ġ ����! Enter Ű�� ���� ���������� �̵� ����");
            }
            else
            {
                Debug.Log("���� ����ġ! �ʱ�ȭ");
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
