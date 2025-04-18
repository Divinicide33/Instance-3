using UnityEngine;
using System.Collections;

public class DoorBoss : Door
{
    
    [SerializeField] private RoomId nextRoom; // Salle de destination pour cette porte
    public RoomId NextRoom => nextRoom;
    [SerializeField] private string selectedDoorName; // Nom de la porte sélectionnée dans la scène cible (renseigné via l'éditeur personnalisé)
    [SerializeField] private Vector3 targetPosition; // Position cible de la porte (renseignée par le DoorEditor)
    public Vector3 TargetPosition
    {
        get { return targetPosition; }
        set { targetPosition = value; }
    }
    
    
    
    //private bool isActive
    
    
    
    
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.TryGetComponent<Enemy>(out _))
            return;
            
        if (!other.gameObject.transform.parent.TryGetComponent<PlayerController>(out _))
            return;
        
        EnterDoor(other.transform.parent);
    }

    private void EnterDoor(Transform player)
    {
        //Debug.Log($"Position cible définie (targetPosition) : {targetPosition}");
        PlayerController.onSaveDoor?.Invoke(new DoorData(nextRoom, targetPosition));
        RoomManager.Instance.ChangeRoomWithFade(nextRoom, player, targetPosition);
        
        // Vous pouvez ajouter d'autres actions après la transition, par exemple révéler la mini-map
        // MiniMapRoomManager.instance.RevealRoom();
    }
    
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawSphere(targetPosition, 0.2f);
        Gizmos.DrawLine(transform.position, targetPosition);
    }
}