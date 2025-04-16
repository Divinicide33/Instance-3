using UnityEngine;

namespace AI.Zeus
{
    public class ZeusCloudBehavior : MonoBehaviour
    {
        public CloudType cloudType;
        public float lightningDelay = 2.5f;
        public float despawnDelay = 5f;
        public GameObject lightningPrefab;   
        public Transform spawnPoint;         

        public int numberOfLightnings = 3;   
        public float lightningSpacing = 20f; 

        private float timeElapsed = 0f;     
        private bool isSpawningLightnings = false; 

        private void Start()
        {
            isSpawningLightnings = true;
        }

        void Update()
        {
            timeElapsed += Time.deltaTime;
            if (isSpawningLightnings)
            {
                if (timeElapsed >= lightningDelay) 
                {
                    SpawnLightnings();
                }
            }
            else
            {
                if (timeElapsed >= despawnDelay)
                {
                    Destroy(gameObject);
                }
            }
        }

        void SpawnLightnings()
        {
            isSpawningLightnings = false; 
            Debug.Log("Spawning lightnings...");
            for (int i = 0; i < numberOfLightnings; i++)
            {
                Vector2 offset = Vector2.zero;
                if (cloudType == CloudType.Top)
                {
                    offset = new Vector2(0, -i * lightningSpacing);
                }
                else if (cloudType == CloudType.Side)
                {
                    offset = new Vector2(i * lightningSpacing, 0);
                }
                Vector2 finalPosition = (Vector2)spawnPoint.position + offset;
                GameObject lightning = Instantiate(lightningPrefab, finalPosition, Quaternion.identity);
                Vector2 direction = cloudType == CloudType.Top ? Vector2.down : Vector2.right;
                lightning.GetComponent<ZeusLightningBehavior>().StartLightning(direction);
            }
            isSpawningLightnings = false;
        }
    }
}
