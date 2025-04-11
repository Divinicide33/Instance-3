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
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.TryGetComponent<PlayerController>(out PlayerController playerController))
        {
            DontDestroyOnLoad(other.gameObject);
            PlayerInputScript.onDisableInput?.Invoke();
            Transform player = other.transform;
            RoomManager.onDoorUsed?.Invoke(destinationRoom, player, destination);
        }
    }
}