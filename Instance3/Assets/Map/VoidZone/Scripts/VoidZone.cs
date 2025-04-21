using System;
using UnityEngine;

public class VoidZone : MonoBehaviour
{
    public static Action onUse { get; set; }
    
    private PlayerController playerControllerInZone = null;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.TryGetComponent<Enemy>(out _))
            return;
        
        other.gameObject.transform.parent.TryGetComponent<PlayerController>(out PlayerController player);
        if (!player)
            return;

        playerControllerInZone = player;
        UseVoidZone();
    }

    private void UseVoidZone()
    {
        PlayerInputScript.onDisableInput?.Invoke();

        playerControllerInZone.stat.AddHp(-1);
        onUse?.Invoke(); // FX / son / UI
        if (playerControllerInZone.stat.health > 0)
        {
            RoomManager.Instance.ChangeRoomWithFade(
                playerControllerInZone.GetLastDoorUsed.room,
                playerControllerInZone.transform,
                playerControllerInZone.GetLastDoorUsed.position);
            Debug.Log("✅ VoidZone utilisée. Sauvegarde et reset du joueur.");
        }
        else
            playerControllerInZone.Defeat();
    }
}