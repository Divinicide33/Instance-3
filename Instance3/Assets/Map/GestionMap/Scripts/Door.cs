using UnityEngine;

public class Door : MonoBehaviour
{
    /*[SerializeField] private RoomId destinationRoom;
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
    }*/




    [SerializeField] private RoomId destinationRoom;
    [SerializeField] private Transform destinationSpawnPoint;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.gameObject.TryGetComponent<PlayerController>(out _))
            return;

        StartCoroutine(EnterDoor(other.transform));
    }

    private System.Collections.IEnumerator EnterDoor(Transform player)
    {
        yield return RoomManager1.Instance.ChangeRoom(destinationRoom, player, destinationSpawnPoint.position);

    }
}