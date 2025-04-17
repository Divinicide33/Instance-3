using UnityEngine;

namespace AI.Zeus
{
    public class ZeusLightningBehavior : MonoBehaviour
    {
        public float speed = 10f;
        public int damage = 1;

        private ZeusCloudBehavior cloudParent;
        private BTZeusTree zeusTree;

        public void StartLightning(Vector2 lightningDirection, ZeusCloudBehavior parent)
        {
            cloudParent = parent;
        }

        void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Platform"))
            {
                if (cloudParent != null && cloudParent.gameObject != null)
                {
                    cloudParent.StopLightningSpawning(gameObject);
                }
                Destroy(gameObject);
            }
        }
    }
}
