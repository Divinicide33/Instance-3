using System;
using UnityEngine;

public class VoidZone : MonoBehaviour
{
    [Header("Destination")]
    [SerializeField] private DoorData voidZoneData;
    
    public static Action onUse { get; set; }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.TryGetComponent<Enemy>(out _))
            return;

        if (other.transform.parent.TryGetComponent<PlayerController>(out PlayerController player))
        {
            UseVoidZone(player);
        }
    }

    private void UseVoidZone(PlayerController player)
    {
        PlayerInputScript.onDisableInput?.Invoke();

        voidZoneData.position = transform.position;
        PlayerController.onSaveDoor?.Invoke(voidZoneData);

        player.stats.SetHpToHpMax(); // si besoin
        PlayerPotion.onRecharge?.Invoke(); // si besoin

        onUse.Invoke(); // FX / son / UI

        Debug.Log("✅ VoidZone utilisée. Sauvegarde et reset du joueur.");
    }
}