using AI.Zeus;
using BehaviorTree;
using System.Collections.Generic;
using UnityEngine;

public class BTAction_SpawnClouds : BTNode
{
    private BTZeusTree tree;
    private int currentSpawnIndex = 0;
    private float spawnTimer = 0f;
    private List<GameObject> currentCloudsAndLightnings = new List<GameObject>();

    public BTAction_SpawnClouds(BTZeusTree btParent)
    {
        tree = btParent;
    }

    public override BTNodeState Evaluate()
    {
        if (tree.currentPattern == null)
        {
            Debug.LogWarning("BTAction_SpawnClouds: currentPattern is null.");
            return BTNodeState.FAILURE;
        }

        if (tree.currentPattern.cloudSpawnsWithDurations == null || tree.currentPattern.cloudSpawnsWithDurations.Count == 0)
        {
            Debug.LogWarning("BTAction_SpawnClouds: No cloud spawns defined in currentPattern.");
            return BTNodeState.FAILURE;
        }

        if (!tree.isPatternRunning)
        {
            currentSpawnIndex = 0;
            spawnTimer = Time.time + tree.currentPattern.cloudSpawnsWithDurations[0].duration;
            tree.isPatternRunning = true;
        }

        if (Time.time >= spawnTimer && currentSpawnIndex <= tree.currentPattern.cloudSpawnsWithDurations.Count)
        {
            if (currentSpawnIndex == tree.currentPattern.cloudSpawnsWithDurations.Count)
            {
                tree.isPatternRunning = false;
                tree.currentPattern = null;
                tree.currentPatternIndex++;
                return BTNodeState.FAILURE;
            }
            SpawnCloudGroup(tree.currentPattern.cloudSpawnsWithDurations[currentSpawnIndex]);
            spawnTimer = Time.time + tree.currentPattern.cloudSpawnsWithDurations[currentSpawnIndex].duration;
            currentSpawnIndex++;
        }

        return BTNodeState.RUNNING;
    }

    private void SpawnCloudGroup(ZeusCloudSpawnWithDuration spawnData)
    {
        if (spawnData == null || spawnData.cloudSpawns == null || spawnData.cloudSpawns.Count == 0)
        {
            Debug.LogWarning("BTAction_SpawnClouds: Empty cloud group.");
            return;
        }

        foreach (var cloud in spawnData.cloudSpawns)
        {
            Vector3 spawnPosition = ConvertGridToWorldPosition(cloud.line, cloud.colum, tree.gridSize);

            GameObject prefab = cloud.type == CloudType.Top ? tree.topCloudPrefab : tree.sideCloudPrefab;

            if (prefab == null)
            {
                Debug.LogError($"BTAction_SpawnClouds: Prefab is null for CloudType {cloud.type}. Assign it in BTZeusTree.");
                continue;
            }

            GameObject cloudInstance = GameObject.Instantiate(prefab, spawnPosition, Quaternion.identity);

            if (cloudInstance != null)
            {
                cloudInstance.transform.parent = tree.cloudContainer;

                ZeusCloudBehavior zcb = cloudInstance.GetComponent<ZeusCloudBehavior>();
                if (zcb != null)
                {
                    zcb.tree = tree;
                }
                currentCloudsAndLightnings.Add(cloudInstance);
            }
            else
            {
                Debug.LogError("BTAction_SpawnClouds: Cloud instance failed to instantiate.");
            }
        }
    }

    private Vector3 ConvertGridToWorldPosition(int row, int column, Vector2 gridSize)
    {
        float cellWidth = gridSize.x / tree.columns;
        float cellHeight = gridSize.y / tree.rows;
        float xOffset = -gridSize.x / 2f + (cellWidth / 2f);
        float yOffset = -gridSize.y / 2f + (cellHeight / 2f);

        float xPosition = tree.roomTransform.position.x + (column * cellWidth) + xOffset;
        float yPosition = tree.roomTransform.position.y + (row * cellHeight) + yOffset;
        float zPosition = tree.roomTransform.position.z;

        return new Vector3(xPosition, yPosition, zPosition);
    }
}
