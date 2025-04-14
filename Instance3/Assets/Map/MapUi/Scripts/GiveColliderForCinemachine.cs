using System;
using UnityEngine;

public class GiveColliderForCinemachine : MonoBehaviour
{
    public static event Action<PolygonCollider2D> onColliderChange;

    private PolygonCollider2D polygonCollider2D;

    public void SendCollider(GameObject cinemachine)
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

    public System.Collections.IEnumerator ReloadCollider(GameObject cinemachine)
    {
        cinemachine.SetActive(false);
        yield return new WaitForEndOfFrame();
        cinemachine.SetActive(true);
    }
}