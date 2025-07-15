using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject[] enemyPrefabs; // G1_Black, G2_Red, G3_Yellow
    public Transform enemyParent;

    private List<Vector3> recentSpawnPositions = new List<Vector3>();
    private List<int> recentDirections = new List<int>();
    private int maxRecent = 5;

    void Start()
    {
        int initialCount = Random.Range(3, 6);
        for (int i = 0; i < initialCount; i++)
        {
            SpawnEnemy();
        }

        InvokeRepeating(nameof(SpawnEnemy), Random.Range(0f, 4f), Random.Range(0f, 4f));
    }

    void SpawnEnemy()
    {
        int caseIndex;
        Vector3 spawnPos = Vector3.zero;
        Vector3 moveDir = Vector3.zero;
        int attempts = 0;

        do
        {
            caseIndex = Random.Range(0, 8);

            switch (caseIndex)
            {
                case 0: spawnPos = new Vector3(0.5f, 10f, Random.Range(3, 11)); moveDir = new Vector3(0f, 0f, -1f); break;
                case 1: spawnPos = new Vector3(Random.Range(3, 11), 10f, 0.5f); moveDir = new Vector3(-1f, 0f, 0f); break;
                case 2: spawnPos = new Vector3(0.5f, 10f, Random.Range(-10, -2)); moveDir = new Vector3(0f, 0f, 1f); break;
                case 3: spawnPos = new Vector3(Random.Range(-10, -2), 10f, 0.5f); moveDir = new Vector3(1f, 0f, 0f); break;
                case 4: spawnPos = new Vector3(-0.5f, 10f, Random.Range(3, 11)); moveDir = new Vector3(0f, 0f, -1f); break;
                case 5: spawnPos = new Vector3(Random.Range(3, 11), 10f, -0.5f); moveDir = new Vector3(-1f, 0f, 0f); break;
                case 6: spawnPos = new Vector3(-0.5f, 10f, Random.Range(-10, -2)); moveDir = new Vector3(0f, 0f, 1f); break;
                case 7: spawnPos = new Vector3(Random.Range(-10, -2), 10f, -0.5f); moveDir = new Vector3(1f, 0f, 0f); break;
            }

            attempts++;
        } while ((IsOppositeDirection(caseIndex) || IsConflictingSpawn(spawnPos, moveDir)) && attempts < 10);

        // ±â·Ï
        recentSpawnPositions.Add(spawnPos);
        recentDirections.Add(caseIndex);
        if (recentSpawnPositions.Count > maxRecent)
        {
            recentSpawnPositions.RemoveAt(0);
            recentDirections.RemoveAt(0);
        }

        int prefabIndex = Random.Range(0, enemyPrefabs.Length);
        GameObject enemy = Instantiate(enemyPrefabs[prefabIndex], spawnPos, Quaternion.identity, enemyParent);
        EnemyController controller = enemy.GetComponent<EnemyController>();
        controller.moveDirection = moveDir.normalized;
    }

    bool IsOppositeDirection(int newDir)
    {
        foreach (int dir in recentDirections)
        {
            if ((dir == 0 && newDir == 2) || (dir == 2 && newDir == 0) ||
                (dir == 1 && newDir == 3) || (dir == 3 && newDir == 1) ||
                (dir == 4 && newDir == 6) || (dir == 6 && newDir == 4) ||
                (dir == 5 && newDir == 7) || (dir == 7 && newDir == 5))
            {
                return true;
            }
        }
        return false;
    }

    bool IsConflictingSpawn(Vector3 newPos, Vector3 newDir)
    {
        for (int i = 0; i < recentSpawnPositions.Count; i++)
        {
            Vector3 oldPos = recentSpawnPositions[i];
            Vector3 oldDir = GetDirectionFromIndex(recentDirections[i]);

            bool sameX = Mathf.Approximately(oldPos.x, newPos.x);
            bool sameZ = Mathf.Approximately(oldPos.z, newPos.z);
            bool oppositeX = Mathf.Approximately(oldDir.x, -newDir.x);
            bool oppositeZ = Mathf.Approximately(oldDir.z, -newDir.z);

            if ((sameX && oppositeZ) || (sameZ && oppositeX))
                return true;
        }
        return false;
    }

    Vector3 GetDirectionFromIndex(int index)
    {
        switch (index)
        {
            case 0: return new Vector3(0f, 0f, -1f);
            case 1: return new Vector3(-1f, 0f, 0f);
            case 2: return new Vector3(0f, 0f, 1f);
            case 3: return new Vector3(1f, 0f, 0f);
            case 4: return new Vector3(0f, 0f, -1f);
            case 5: return new Vector3(-1f, 0f, 0f);
            case 6: return new Vector3(0f, 0f, 1f);
            case 7: return new Vector3(1f, 0f, 0f);
            default: return Vector3.zero;
        }
    }
}
