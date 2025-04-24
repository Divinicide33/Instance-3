using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AI.Zeus
{
    public class ZeusLightningBehavior : MonoBehaviour
    {
        private ZeusCloudBehavior cloudParent;

        public void StartLightning(Vector2 lightningDirection, ZeusCloudBehavior parent)
        {
            cloudParent = parent;
            
            List<RaycastHit2D> hits = Physics2D.BoxCastAll(transform.position, transform.localScale, 0, Vector2.zero).ToList();

            foreach (RaycastHit2D hit in hits)
            {
                if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Platform"))
                {
                    if (cloudParent != null && cloudParent.gameObject != null)
                    {
                        cloudParent.StopLightningSpawning(gameObject);
                    }
                    Destroy(gameObject);
                    return;
                }
            }
        }
    }
}
