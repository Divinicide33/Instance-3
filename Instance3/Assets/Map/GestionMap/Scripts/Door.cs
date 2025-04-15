using UnityEngine;

public class Door : MonoBehaviour
{
    //[SerializeField] private RoomId destinationRoom;
    //[SerializeField] private Transform destination;
    [SerializeField] private bool goLeft = false;
    [SerializeField] private RoomId destinationRoom;
    [SerializeField] private Transform destinationSpawnPoint;

    private void Start()
    {
        if (goLeft)
        {
            destinationSpawnPoint.localPosition = new Vector3(-destinationSpawnPoint.localPosition.x, destinationSpawnPoint.localPosition.y, destinationSpawnPoint.localPosition.z);
        }
        MiniMapRoomManager.instance.RevealRoom();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.gameObject.transform.parent.TryGetComponent<PlayerController>(out _))
            return;

        StartCoroutine(EnterDoor(other.transform.parent));
        MiniMapRoomManager.instance.RevealRoom();
    }

    private System.Collections.IEnumerator EnterDoor(Transform player)
    {

        yield return RoomManager.Instance.ChangeRoom(destinationRoom, player, destinationSpawnPoint.position);
    }

    //private void OnTriggerEnter2D(Collider2D other)
    //{
    //    
    //    if (playerController != null)
    //    {
    //        PlayerInputScript.onDisableInput?.Invoke();
    //        Transform player = playerController.transform;
    //        
    //    }
    //}
}