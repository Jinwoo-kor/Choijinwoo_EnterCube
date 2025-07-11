using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformSpawner : MonoBehaviour
{
    public GameObject platformPrefab;
    public float offset = 0.5f;

    public List<GameObject> centerPlatforms = new List<GameObject>();
    public List<GameObject> candidatePlatforms = new List<GameObject>();

    public void SpawnCenterPlatforms(Vector3 centerPoint)
    {
        ClearPlatforms(centerPlatforms);

        Vector3[] offsets = new Vector3[]
        {
            new Vector3(-offset, 0f, -offset),
            new Vector3(-offset, 0f, offset),
            new Vector3(offset, 0f, offset),
            new Vector3(offset, 0f, -offset)
        };

        foreach (Vector3 offsetPos in offsets)
        {
            Vector3 spawnPosition = centerPoint + offsetPos;
            GameObject platform = Instantiate(platformPrefab, spawnPosition, Quaternion.identity);
            platform.tag = "Platform";
            centerPlatforms.Add(platform);
        }
    }

    public void PreSpawnCandidatePlatforms(Vector3 basePosition)
    {
        ClearPlatforms(candidatePlatforms);

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
            platform.tag = "Candidate";
            candidatePlatforms.Add(platform);
        }
    }

    public void ClearPlatforms(List<GameObject> platforms)
    {
        foreach (GameObject platform in platforms)
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
                Destroy(platform, 3f);
            }
        }
        platforms.Clear();
    }
}
