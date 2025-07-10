using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformSpawner : MonoBehaviour
{
    public GameObject platformPrefab;
    public float offset = 0.5f;
    public Transform spawnPoint;

    // ������ �÷��� ���� ����Ʈ
    public List<GameObject> spawnedPlatforms = new List<GameObject>();

    // �÷��� ���� �Լ�
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

    // ���� �÷����� Rigidbody �߰� �� ����Ʈ �ʱ�ȭ
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
