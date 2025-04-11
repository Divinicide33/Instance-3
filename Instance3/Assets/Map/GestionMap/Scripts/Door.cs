using UnityEngine;

public class DoorTrigger : MonoBehaviour
{
    [SerializeField] private RoomId destinationRoom;
    [SerializeField] private Transform destination;
    [SerializeField] private bool goLeft = false;


    private void Start()
    {
        if (goLeft)
        {
            destination.localPosition = new Vector3(-destination.localPosition.x, destination.localPosition.y, destination.localPosition.z);
        }
        MiniMapRoomManager.instance.RevealRoom();
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        PlayerController playerController = other.GetComponentInParent<PlayerController>();
        if (playerController != null)
        {
            PlayerInputScript.onDisableInput?.Invoke();
            Transform player = playerController.transform;
            RoomManager.onDoorUsed?.Invoke(destinationRoom, player, destination);
        }
    }
}