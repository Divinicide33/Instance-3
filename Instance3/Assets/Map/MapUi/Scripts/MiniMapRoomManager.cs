using UnityEngine;

public class MiniMapRoomManager : MonoBehaviour
{
    public static MiniMapRoomManager instance;

    private MapContainerData[] rooms;

    [SerializeField] private RoomManager roomManager;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        rooms = GetComponentsInChildren<MapContainerData>(true);
    }

    private void Start()
    {
        RevealRoom();
    }

    private void Update()
    {
        RevealRoom();
    }

    public void RevealRoom()
    {
        for (int i = 0; i < rooms.Length; i++)
        {
            if (rooms[i].roomScene == roomManager.rooms && !rooms[i].hasBeenRevealed)
            {
                rooms[i].gameObject.SetActive(true);
                rooms[i].hasBeenRevealed = true;
                return;
            }
        }
    }
}
