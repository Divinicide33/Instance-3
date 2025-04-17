using System.Collections.Generic;
using UnityEngine;

namespace AI.Zeus
{
    public enum CloudType { Top, Side }

    // Classe modifi�e pour ajouter la duration � chaque spawn de nuage
    [System.Serializable]
    public class ZeusCloud
    {
        public int colum;
        public int line;
        public CloudType type;
    }

    // Nouvelle classe qui lie un ZeusCloudSpawn avec une duration
    [System.Serializable]
    public class ZeusCloudSpawnWithDuration
    {
        public List<ZeusCloud> cloudSpawns;    // Contient le spawn du nuage
        public float duration;                 // Dur�e du nuage
    }

    [System.Serializable]
    public class ZeusPattern
    {
        public string name;                                                // Nom du pattern                    
        public List<ZeusCloudSpawnWithDuration> cloudSpawnsWithDurations;  // Liste des nuages avec leur dur�e

        public ZeusPattern(string patternName, float attackDuration, List<ZeusCloudSpawnWithDuration> spawnsWithDurations)
        {
            name = patternName;
 
            cloudSpawnsWithDurations = spawnsWithDurations;
        }
    }

    // Classe Pattern qui contient des ZeusPattern
    [System.Serializable]
    public class Pattern
    {
        public List<ZeusPattern> patterns; // Liste des patterns
    }
}
