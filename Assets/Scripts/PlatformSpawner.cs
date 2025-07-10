using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformSpawner : MonoBehaviour
{
    public GameObject platformPrefab;
    public float offset = 0.5f;
    public Transform spawnPoint;

    // 생성된 플랫폼 저장 리스트
    public List<GameObject> spawnedPlatforms = new List<GameObject>();

    // 플랫폼 생성 함수
    public void Spawn(Transform centerPoint, string tag = "Platform", bool clearBeforeSpawn = true)
    {
        if (clearBeforeSpawn)
        {
            ClearPreviousPlatforms();
        }

        Vector3 basePosition = centerPoint.position;

        Vector3[] offsets = new Vector3[]
        {
            new Vector3(-offset, 0f, -offset),
            new Vector3(-offset, 0f, offset),
            new Vector3(offset, 0f, offset),
            new Vector3(offset, 0f, -offset)
        };

        foreach (Vector3 offsetPos in offsets)
        {
            Vector3 spawnPosition = basePosition + offsetPos;
            GameObject platform = Instantiate(platformPrefab, spawnPosition, Quaternion.identity);
            platform.tag = tag;
            spawnedPlatforms.Add(platform);
        }
    }

    // 기존 플랫폼에 Rigidbody 추가 및 리스트 초기화
    public void ClearPreviousPlatforms()
    {
        foreach (GameObject platform in spawnedPlatforms)
        {
            if (platform != null)
            {
                Rigidbody rb = platform.GetComponent<Rigidbody>();
                if (rb == null)
                {
                    rb = platform.AddComponent<Rigidbody>();
                }
                rb.useGravity = true;
                rb.isKinematic = false;
            }
        }

        spawnedPlatforms.Clear();
    }
}
