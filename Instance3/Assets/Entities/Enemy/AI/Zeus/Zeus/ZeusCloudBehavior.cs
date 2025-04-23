using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;

namespace AI.Zeus
{
    public class ZeusCloudBehavior : MonoBehaviour
    {
        [Header("Cloud")]
        public CloudType cloudType;
        public float despawnDelay = 5f;

        [Header("Lightning")]
        public float lightningDelay = 2.5f;
        public float lightningDespawnDelay = 3f;
        public float lightningSpacing = 20f;
        public int numberOfLightnings = 3;
        public GameObject lightningPrefab;
        [HideInInspector] public int damage = 1;

        [Header("Spawn")]
        public Transform spawnPoint;

        [Header("Bool")]
        private bool isSpawningLightnings = true;
        private bool isDestroyingLightning = false;

        [HideInInspector] public List<GameObject> spawnedLightnings = new List<GameObject>();
        [HideInInspector] public bool spawnInterrupted = false;
        [HideInInspector] private float timeElapsed = 0f;
        [HideInInspector] public List<GameObject> spawnedGroup;
        [HideInInspector] public BTZeusTree tree;

        [Header("FX")]
        [HideInInspector] public ZeusAttackFX zeusAttackFX;

        private void Start()
        {
            damage = tree.stat.damage;
            zeusAttackFX = GetComponentInChildren<ZeusAttackFX>();
            SpawnLightnings();
        }

        void Update()
        {
            timeElapsed += Time.deltaTime;
            if (isSpawningLightnings)
            {
                if (timeElapsed >= lightningDelay)
                {
                    ActivateLightning();
                }
            }
            else if (isDestroyingLightning)
            {
                if (timeElapsed >= lightningDespawnDelay)
                {
                    DestroyLightnings();
                }
            }
            else if (timeElapsed >= despawnDelay)
            {
                Destroy(gameObject);
            }
        }

        void SpawnLightnings()
        {
            for (int i = 0; i < numberOfLightnings; i++)
            {
                Vector2 offset = cloudType == CloudType.Top
                    ? new Vector2(0, -i * lightningSpacing)
                    : new Vector2(i * lightningSpacing, 0);

                Vector2 finalPosition = (Vector2)spawnPoint.position + offset;
                Quaternion rotation = cloudType == CloudType.Side ? Quaternion.Euler(0, 0, 90) : Quaternion.identity;

                GameObject lightning = Instantiate(lightningPrefab, finalPosition, rotation, tree.lightningContainer);

                LightningDamageZone ltnDamage = lightning.GetComponent<LightningDamageZone>();
                ltnDamage.SetDamage(damage);
                ltnDamage.SetKnockbackPower(tree.knockbackPower);

                lightning.SetActive(false);
                spawnedLightnings.Add(lightning);
                spawnedGroup?.Add(lightning);

                ZeusLightningBehavior zlb = lightning.GetComponent<ZeusLightningBehavior>();
                zlb?.StartLightning(cloudType == CloudType.Top ? Vector2.down : Vector2.right, this);
            }
        }

        private void ActivateLightning()
        {
            isSpawningLightnings = false;
            isDestroyingLightning = true;
            zeusAttackFX?.ShowSFX(tree.sfxAttackName);
            for (int i = 0; i < spawnedLightnings.Count; i++)
            {
                if (!spawnedLightnings[i]) continue;
                spawnedLightnings[i].SetActive(true);
                spawnedLightnings[i].GetComponent<LightningDamageZone>().enabled = true;
                CloudDamage();
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
                    spawnedGroup?.Add(sideCloud);

                    Destroy(previousLightning);
                }
            }

            for (int i = hitIndex + 1; i < spawnedLightnings.Count; i++)
            {
                if (spawnedLightnings[i] != null)
                {
                    GameObject lightning = spawnedLightnings[i];
                    spawnedLightnings.RemoveAt(i);
                    Destroy(lightning);
                }
            }
        }

        private void DestroyLightnings()
        {
            if (spawnedLightnings.Count > 0)
            {
                foreach (var lightning in spawnedLightnings)
                {
                    if (lightning != null)
                    {
                        Destroy(lightning);
                    }
                }
            }
            isDestroyingLightning = false;
        }

        private void CloudDamage()
        {
            LightningDamageZone ltnDamage;

            if (!TryGetComponent<LightningDamageZone>(out ltnDamage))
            {
                ltnDamage = gameObject.AddComponent<LightningDamageZone>();
            }
            ltnDamage.SetDamage(damage);
            ltnDamage.SetKnockbackPower(tree.knockbackPower);
            ltnDamage.enabled = true;
        }
    }
}
