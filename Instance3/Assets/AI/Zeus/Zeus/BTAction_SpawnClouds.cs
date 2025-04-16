using BehaviorTree;
using UnityEngine;
using System.Collections.Generic;

namespace AI.Zeus
{
    public class BTAction_SpawnClouds : BTNode
    {
        private BTZeusTree tree;

        public BTAction_SpawnClouds(BTZeusTree btParent)
        {
            tree = btParent;
        }

        public override BTNodeState Evaluate()
        {
            if (tree.currentPattern == null || tree.currentPattern.patterns == null)
            {
                return BTNodeState.FAILURE;
            }

            float elapsedTime = Time.time - tree.patternStartTime;

            if (!tree.isPatternRunning)
            {
                Debug.Log("Spawning clouds...");

                foreach (var attackPattern in tree.currentPattern.patterns[tree.currentPatternIndex].cloudSpawns)
                {
                    foreach (var column in attackPattern.colum)
                    {
                        foreach (var line in attackPattern.line)
                        {
                            Vector3 spawnPosition = ConvertGridToWorldPosition(line, column);
                            GameObject prefab = attackPattern.type == CloudType.Top ? tree.topCloudPrefab : tree.sideCloudPrefab;
                            GameObject cloudInstance = GameObject.Instantiate(prefab, spawnPosition, Quaternion.identity);
                        }
                    }
                }

                tree.isPatternRunning = true;
                tree.patternStartTime = Time.time;
                return BTNodeState.RUNNING;
            }

            if (elapsedTime >= tree.currentPattern.patterns[tree.currentPatternIndex].duration)
            {
                Debug.Log("Pattern ended, starting new one");
                tree.currentPatternIndex++;

                tree.isPatternRunning = false;

                return BTNodeState.SUCCESS;
            }

            return BTNodeState.RUNNING;
        }
        private Vector3 ConvertGridToWorldPosition(int row, int column)
        {
            float cellSize = 1f;
            float xPosition = column * cellSize;
            float yPosition = row * cellSize;
            float zPosition = 0;

            return new Vector3(xPosition, yPosition, zPosition);
        }
    }
}
