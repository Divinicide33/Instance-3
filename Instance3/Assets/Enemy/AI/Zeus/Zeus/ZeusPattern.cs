using System.Collections.Generic;

namespace AI.Zeus
{
    public enum CloudType { Top, Side }

    [System.Serializable]
    public class ZeusCloud
    {
        public int colum;
        public int line;
        public CloudType type;
    }

    [System.Serializable]
    public class ZeusCloudSpawnWithDuration
    {
        public List<ZeusCloud> cloudSpawns;   
        public float duration;                 
    }

    [System.Serializable]
    public class ZeusPattern
    {
        public string name;                                                                  
        public List<ZeusCloudSpawnWithDuration> cloudSpawnsWithDurations;  

        public ZeusPattern(string patternName, float attackDuration, List<ZeusCloudSpawnWithDuration> spawnsWithDurations)
        {
            name = patternName;
 
            cloudSpawnsWithDurations = spawnsWithDurations;
        }
    }

    [System.Serializable]
    public class Pattern
    {
        public List<ZeusPattern> patterns;
    }
}
