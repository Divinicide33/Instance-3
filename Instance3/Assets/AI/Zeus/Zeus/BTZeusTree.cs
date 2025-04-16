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
        public List<Pattern> attackPatterns;

        [Header("Cooldown")]
        public float minCooldown = 3f;
        public float maxCooldown = 5f;

        [HideInInspector] public Pattern currentPattern;
        public int currentPatternIndex = 0;
        public float patternStartTime;
        [HideInInspector] public float lastPatternEndTime = Mathf.NegativeInfinity;
        public bool isPatternRunning = false;

        public float nextAttackTime;

        public List<GameObject> activeClouds = new List<GameObject>();

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
    }
}
