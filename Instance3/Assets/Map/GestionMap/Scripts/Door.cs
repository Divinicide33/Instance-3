using UnityEngine;
using System.Collections;

public class Door : MonoBehaviour
{
    // Salle de destination pour cette porte
    [SerializeField] private RoomId nextRoom;
    public RoomId NextRoom => nextRoom;

    // Nom de la porte sélectionnée dans la scène cible (renseigné via l'éditeur personnalisé)
    [SerializeField] private string selectedDoorName;
    public string SelectedDoorName => selectedDoorName;

    // Position cible de la porte (renseignée par le DoorEditor)
    [SerializeField] private Vector3 targetPosition;
    public Vector3 TargetPosition
    {
        get { return targetPosition; }
        set { targetPosition = value; }
    }

    [SerializeField] private Transform spawnPoint;
    public Transform SpawnPoint
    {
        get { return spawnPoint; }
        set { spawnPoint = value; }
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.gameObject.transform.parent.TryGetComponent<PlayerController>(out _))
            return;
        
        StartCoroutine(EnterDoor(other.transform.parent));
    }

    private IEnumerator EnterDoor(Transform player)
    {
        Debug.Log($"Position cible définie (targetPosition) : {targetPosition}");

        yield return RoomManager.Instance.ChangeRoom(nextRoom, player, targetPosition);

        // Vous pouvez ajouter d'autres actions après la transition, par exemple révéler la mini-map
        // MiniMapRoomManager.instance.RevealRoom();
    }
    
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawSphere(targetPosition, 0.2f);
        Gizmos.DrawLine(transform.position, targetPosition);
        Debug.Log($"target position : {targetPosition}");
    }
}