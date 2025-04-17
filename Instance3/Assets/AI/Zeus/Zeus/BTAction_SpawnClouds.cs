using AI.Zeus;
using BehaviorTree;
using System.Collections.Generic;
using UnityEngine;

public class BTAction_SpawnClouds : BTNode
{
    private BTZeusTree tree;
    private int currentSpawnIndex = 0;
    private float spawnTimer = 0f;
    private float cleanupTimer = 0f;
    private bool waitingToCleanup = false;
    private bool initialized = false;

    private List<GameObject> currentCloudsAndLightnings = new List<GameObject>(); 

    public BTAction_SpawnClouds(BTZeusTree btParent)
    {
        tree = btParent;
    }

    public override BTNodeState Evaluate()
    {
        if (tree.currentPattern == null || tree.currentPattern.cloudSpawnsWithDurations.Count == 0)
            return BTNodeState.FAILURE;

        if (!tree.isPatternRunning)
        {
            Debug.Log("Spawning clouds...");
            currentSpawnIndex = 0;
            spawnTimer = Time.time + tree.currentPattern.cloudSpawnsWithDurations[0].duration;
            //cleanupTimer = 0f;
            //waitingToCleanup = false;
            //currentCloudsAndLightnings.Clear();

            //tree.patternStartTime = Time.time;
            tree.isPatternRunning = true;
        }

        if (currentSpawnIndex >= tree.currentPattern.cloudSpawnsWithDurations.Count)
        {
            // Nettoyage des nuages et éclairs spécifiques au pattern actuel
            //CleanupCurrentPatternObjects();

            // Nettoyer les objets de l'activeClouds pour le pattern actuel seulement
            //tree.activeClouds.RemoveAll(cloud => cloud == null || currentCloudsAndLightnings.Contains(cloud));

            // Passer au pattern suivant après nettoyage
            tree.isPatternRunning = false;
            tree.currentPatternIndex++; // Incrémentation ici

            Debug.Log("1 Attack finished.");
            return BTNodeState.SUCCESS;
        }
        
        if (currentSpawnIndex < tree.currentPattern.cloudSpawnsWithDurations.Count)
        {
            if (Time.time >= spawnTimer)
            {
                Debug.Log("Spawn");
                SpawnCloudGroup(tree.currentPattern.cloudSpawnsWithDurations[currentSpawnIndex]);
                spawnTimer = Time.time + tree.currentPattern.cloudSpawnsWithDurations[currentSpawnIndex].duration;
                currentSpawnIndex++;
            }

            return BTNodeState.RUNNING;
        }

        /*
        if (!waitingToCleanup)
        {
            waitingToCleanup = true;
            float totalDuration = tree.currentPattern.cloudSpawnsWithDurations[^1].duration;
            float elapsedTime = Time.time - tree.patternStartTime;
            float remainingTime = totalDuration - elapsedTime;
            cleanupTimer = Time.time + Mathf.Max(remainingTime, 0f);
            return BTNodeState.RUNNING;
        }*/
        
        

        return BTNodeState.RUNNING;
    }

    private void CleanupCurrentPatternObjects()
    {
        foreach (GameObject obj in currentCloudsAndLightnings)
        {
            // Ne pas essayer de détruire un objet null
            if (obj != null)
            {
                GameObject.Destroy(obj);  // Supprime tous les nuages et éclairs du pattern actuel
            }
        }

        currentCloudsAndLightnings.Clear();  // Vide la liste pour ce pattern
    }

    private void SpawnCloudGroup(ZeusCloudSpawnWithDuration spawnData)
    {
        //List<GameObject> groupObjects = new List<GameObject>();

        foreach (var cloud in spawnData.cloudSpawns)
        {
            Vector3 spawnPosition = ConvertGridToWorldPosition(cloud.line, cloud.colum, tree.gridSize);
            GameObject prefab = cloud.type == CloudType.Top ? tree.topCloudPrefab : tree.sideCloudPrefab;

            GameObject cloudInstance = GameObject.Instantiate(prefab, spawnPosition, Quaternion.identity);

            // Vérifier si l'instance est bien instanciée
            if (cloudInstance != null)
            {
                cloudInstance.transform.parent = tree.cloudContainer;

                ZeusCloudBehavior zcb = cloudInstance.GetComponent<ZeusCloudBehavior>();
                if (zcb != null)
                {
                    zcb.tree = tree;
                    zcb.despawnDelay = tree.currentPattern.cloudSpawnsWithDurations[currentSpawnIndex].duration;
                    //zcb.spawnedGroup = groupObjects;
                }

                //groupObjects.Add(cloudInstance);
            }
            else
            {
                Debug.LogError("Cloud instance failed to instantiate.");
            }
        }
        //currentCloudsAndLightnings.AddRange(groupObjects);
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
