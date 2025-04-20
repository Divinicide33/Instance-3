using System;
using UnityEngine;

public class GivePlayerForEnnemy : MonoBehaviour
{
    public static event Action<Transform> onSetPlayerTarget;
    [SerializeField] private LayerMask playerLayer;
    
    private void Start()
    {
        TryFindAndSendPlayer();
    }

    public void TryFindAndSendPlayer()
    {
        // Recherche tous les objets dans la scène
        GameObject[] allObjects = FindObjectsOfType<GameObject>();
        foreach (GameObject obj in allObjects)
        {
            if (((1 << obj.layer) & playerLayer.value) != 0)
            {
                if (obj.TryGetComponent<PlayerController>(out PlayerController player))
                {
                    onSetPlayerTarget?.Invoke(player.transform);
                    return;
                }
            }
        }

        Debug.LogWarning("Aucun PlayerController trouvé dans la scène sur le layer joueur.");
    }
}