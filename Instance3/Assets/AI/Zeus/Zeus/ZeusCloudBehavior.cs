using UnityEngine;
using System.Collections.Generic;

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
        [HideInInspector] public List<GameObject> spawnedLightnings = new List<GameObject>();
        [HideInInspector] public bool spawnInterrupted = false;

        [HideInInspector] public List<GameObject> spawnedGroup;
        [HideInInspector] public BTZeusTree tree;

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
            else if (timeElapsed >= despawnDelay)
            {
                DestroyCloudAndLightnings();
            }
            
        }

        void SpawnLightnings()
        {
            isSpawningLightnings = false;

            for (int i = 0; i < numberOfLightnings; i++)
            {
                Vector2 offset = cloudType == CloudType.Top
                    ? new Vector2(0, -i * lightningSpacing)
                    : new Vector2(i * lightningSpacing, 0);

                Vector2 finalPosition = (Vector2)spawnPoint.position + offset;
                GameObject lightning = Instantiate(lightningPrefab, finalPosition, Quaternion.identity, tree.lightningContainer);

                if (!tree.activeLightnings.Contains(lightning))
                    tree.activeLightnings.Add(lightning);

                spawnedLightnings.Add(lightning);
                spawnedGroup?.Add(lightning);

                ZeusLightningBehavior zlb = lightning.GetComponent<ZeusLightningBehavior>();
                zlb?.StartLightning(cloudType == CloudType.Top ? Vector2.down : Vector2.right, this);
            }
        }

        public void StopLightningSpawning(GameObject hittingLightning)
        {
            if (spawnInterrupted) return;
            spawnInterrupted = true;

            int hitIndex = spawnedLightnings.IndexOf(hittingLightning);

            if (cloudType == CloudType.Side && hitIndex > 0)
            {
                GameObject previousLightning = spawnedLightnings[hitIndex - 1];
                if (previousLightning != null)
                {
                    Vector3 pos = previousLightning.transform.position;
                    GameObject sideCloud = Instantiate(gameObject, pos, Quaternion.identity);
                    ZeusCloudBehavior zcb = sideCloud.GetComponent<ZeusCloudBehavior>();
                    if (zcb != null)
                    {
                        zcb.numberOfLightnings = 0;
                        zcb.tree = tree;
                        zcb.spawnedGroup = spawnedGroup;
                    }

                    tree.activeClouds.Add(sideCloud);
                    spawnedGroup?.Add(sideCloud);

                    Destroy(previousLightning);
                }
            }

            for (int i = hitIndex + 1; i < spawnedLightnings.Count; i++)
            {
                if (spawnedLightnings[i] != null)
                {
                    Destroy(spawnedLightnings[i]);
                }
            }

            spawnedLightnings.Clear();
        }

        private void DestroyCloudAndLightnings()
        {
            if (spawnedLightnings.Count > 0)
            {
                foreach (var lightning in spawnedLightnings)
                {
                    if (lightning != null)
                    {
                        tree.activeLightnings.Remove(lightning);
                        Destroy(lightning);
                    }
                }
            }
            //tree.activeClouds.Remove(gameObject);
            Destroy(gameObject);
        }
    }
}
