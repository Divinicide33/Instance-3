using BehaviorTree;
using UnityEngine;
using System.Collections.Generic;

namespace AI.Zeus
{
    public class BTZeusTree : BTTree
    {
        [Header("References")]
        public Transform zeus;

        [Header("Clouds Prefabs")]
        public GameObject topCloudPrefab;
        public GameObject sideCloudPrefab;

        [Header("Patterns")]
        public List<ZeusPattern> attackPatterns;

        [Header("Cooldown")]
        public float minCooldown = 3f;
        public float maxCooldown = 5f;

        [Header("Damage")]
        public int knockbackPower = 0;

        [Header("Grid")]
        public Transform roomTransform;        
        public Vector2 gridSize = new Vector2(10f, 5f);  
        public int columns = 10;
        public int rows = 5;

        [Header("TpZone")]
        public List<Transform> tpZones = new List<Transform>();
        public int zone;
        public int lastZone;
        public bool haveChangeZone = true;

        [Header("Parent")]
        public Transform cloudContainer;      
        public Transform lightningContainer;  

        [HideInInspector] public ZeusPattern currentPattern;
        [HideInInspector] public int currentPatternIndex = 0;
        [HideInInspector] public float patternStartTime;
        [HideInInspector] public float lastPatternEndTime = Mathf.NegativeInfinity;
        [HideInInspector] public bool isPatternRunning = false;
        [HideInInspector] public float nextAttackTime;

        protected override BTNode SetupTree()
        {
            BTSelector root = new BTSelector(new List<BTNode>
            {
                new BTSequence(new List<BTNode>
                {
                    new BTAction_Cooldown(this),
                    new BTAction_SpawnClouds(this)
                }),
                new BTAction_ChoosePattern(this),
            });
            return root;
        }

        public Vector3 GetWorldPosition(int col, int row)
        {
            Vector2 size = gridSize; // (width, height)
            Vector3 origin = roomTransform.position - new Vector3(size.x / 2f, size.y / 2f, 0f);
            float cellWidth = size.x / columns;
            float cellHeight = size.y / rows;

            return origin + new Vector3(cellWidth * col + cellWidth / 2f, cellHeight * row + cellHeight / 2f, 0f);
        }

#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            if (roomTransform == null) return;

            Vector2 size = gridSize;
            Vector3 origin = roomTransform.position - new Vector3(size.x / 2f, size.y / 2f, 0f);
            float cellWidth = size.x / columns;
            float cellHeight = size.y / rows;

            Gizmos.color = Color.cyan;

            for (int x = 0; x < columns; x++)
            {
                for (int y = 0; y < rows; y++)
                {
                    Vector3 center = origin + new Vector3(cellWidth * x + cellWidth / 2f, cellHeight * y + cellHeight / 2f, 0f);
                    Gizmos.DrawWireCube(center, new Vector3(cellWidth, cellHeight, 0.01f)); // fine Z
                }
            }
        }
#endif
    }
}
