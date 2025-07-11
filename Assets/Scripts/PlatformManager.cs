using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformManager : MonoBehaviour
{
    public PlatformSpawner platformSpawner;
    public float offset = 0.5f;
    public List<int> colorStack = new List<int>();
    public int score = 0;

    void Start()
    {
        platformSpawner.SpawnCenterPlatforms(Vector3.zero);
        PreSpawnNextStageCandidates();
    }

    public void AddColorToStack(int colorCode)
    {
        if (colorStack.Count > 0 && colorStack[0] != colorCode)
        {
            Debug.Log("색상 불일치! 즉시 초기화.");
            ResetAllPlatforms();
            return;
        }

        colorStack.Add(colorCode);
        if (colorStack.Count == 4)
        {
            if (IsStackMatched())
            {
                Debug.Log("색상 일치! Enter 키로 다음 스테이지로 이동하세요.");
            }
            else
            {
                Debug.Log("색상 불일치! 스택 초기화 및 플랫폼 초기화.");
                ResetAllPlatforms();
            }
        }
    }

    public bool IsStackMatched()
    {
        if (colorStack.Count == 0) return false;
        int first = colorStack[0];
        foreach (int code in colorStack)
        {
            if (code != first) return false;
        }
        return true;
    }

    public void ResetAllPlatforms()
    {
        colorStack.Clear();
        foreach (GameObject platform in platformSpawner.centerPlatforms)
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

    public IEnumerator TransitionToNextStage()
    {
        platformSpawner.ClearPlatforms(platformSpawner.centerPlatforms);
        yield return StartCoroutine(MoveCandidatePlatformsToCenter());
        score += 10;
        Debug.Log("점수: " + score);
        colorStack.Clear();
        PreSpawnNextStageCandidates();
    }

    IEnumerator MoveCandidatePlatformsToCenter()
    {
        float duration = 0.5f;
        float timer = 0f;

        Vector3[] targetOffsets = new Vector3[]
        {
            new Vector3(-platformSpawner.offset, 0f, -platformSpawner.offset),
            new Vector3(-platformSpawner.offset, 0f, platformSpawner.offset),
            new Vector3(platformSpawner.offset, 0f, platformSpawner.offset),
            new Vector3(platformSpawner.offset, 0f, -platformSpawner.offset)
        };

        List<Vector3> startPositions = new List<Vector3>();
        for (int i = 0; i < platformSpawner.candidatePlatforms.Count; i++)
        {
            startPositions.Add(platformSpawner.candidatePlatforms[i].transform.position);
        }

        while (timer < duration)
        {
            float t = timer / duration;
            for (int i = 0; i < platformSpawner.candidatePlatforms.Count && i < targetOffsets.Length; i++)
            {
                Vector3 targetPos = targetOffsets[i];
                platformSpawner.candidatePlatforms[i].transform.position = Vector3.Lerp(startPositions[i], targetPos, t);
            }
            timer += Time.deltaTime;
            yield return null;
        }

        for (int i = 0; i < platformSpawner.candidatePlatforms.Count && i < targetOffsets.Length; i++)
        {
            platformSpawner.candidatePlatforms[i].transform.position = targetOffsets[i];
            platformSpawner.candidatePlatforms[i].tag = "Platform";

            Platform p = platformSpawner.candidatePlatforms[i].GetComponent<Platform>();
            if (p != null)
            {
                p.isLocked = false;
                p.colorChangeInterval = 2f;
            }

            platformSpawner.centerPlatforms.Add(platformSpawner.candidatePlatforms[i]);
        }

        platformSpawner.candidatePlatforms.Clear();
    }

    void PreSpawnNextStageCandidates()
    {
        Vector3[] casePositions = new Vector3[]
        {
            new Vector3(10f, 5f, 0f),
            new Vector3(-10f, 5f, 0f),
            new Vector3(0f, 5f, 10f),
            new Vector3(0f, 5f, -10f)
        };

        int index = Random.Range(0, casePositions.Length);
        platformSpawner.PreSpawnCandidatePlatforms(casePositions[index]);
    }
}
