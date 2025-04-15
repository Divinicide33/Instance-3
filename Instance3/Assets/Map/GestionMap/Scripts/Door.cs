using UnityEngine;
using System.Collections;

public class Door : MonoBehaviour
{
    [SerializeField] private RoomId nextRoom;
    public RoomId NextRoom => nextRoom;

    // Nom de la porte sélectionnée dans la scène cible (renseigné via l'éditeur personnalisé)
    [SerializeField] private string selectedDoorName;
    public string SelectedDoorName => selectedDoorName;

    // Position cible pour la téléportation dans la nouvelle scène (renseignée par le DoorEditor)
    [SerializeField] private Vector3 targetPosition;
    public Vector3 TargetPosition => targetPosition;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.gameObject.transform.parent.TryGetComponent<PlayerController>(out _))
            return;
        
        StartCoroutine(EnterDoor(other.transform.parent));
    }

    private IEnumerator EnterDoor(Transform player)
    {
        yield return RoomManager.Instance.ChangeRoom(nextRoom, player, targetPosition);
        
        // on peut faire d'autres actions si nécessaire (révéler la mini-map, etc.)
        //MiniMapRoomManager.instance.RevealRoom();
    }
}