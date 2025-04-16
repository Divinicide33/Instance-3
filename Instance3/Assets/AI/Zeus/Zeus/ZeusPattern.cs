using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UIElements;

namespace AI.Zeus
{
    public enum CloudType { Top, Side }

    [System.Serializable]
    public class ZeusCloudSpawn
    {
        public List<int> colum;
        public List<int> line;
        public CloudType type;        

        public ZeusCloudSpawn(Vector2 pos, CloudType cloudType)
        {
            type = cloudType;
        }
    }

    [System.Serializable]
    public class Pattern
    {
        public List<ZeusAttackPattern> patterns;
    }

    [System.Serializable]
    public class ZeusAttackPattern
    {
        public string name;                  
        public float duration;              
        public List<ZeusCloudSpawn> cloudSpawns;
             
        public ZeusAttackPattern(string patternName, float attackDuration, List<ZeusCloudSpawn> spawns)
        {
            name = patternName;
            duration = attackDuration;
            cloudSpawns = spawns;
        }     
    }
}
