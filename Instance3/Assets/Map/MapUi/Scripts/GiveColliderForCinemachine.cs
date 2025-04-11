using System;
using UnityEngine;

public class GiveColliderForCinemachine : MonoBehaviour
{
    public static event Action<PolygonCollider2D> onColliderChange;

    private PolygonCollider2D polygonCollider2D;

    public void SendCollider()
    {
        if (polygonCollider2D == null)
        {
            polygonCollider2D = GetComponent<PolygonCollider2D>();
        }

        if (polygonCollider2D != null)
        {
            onColliderChange?.Invoke(polygonCollider2D);
        }
        else
        {
            Debug.LogWarning("Aucun PolygonCollider2D trouvé dans " + gameObject.name);
        }
    }
}