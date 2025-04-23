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
        string roomName = PlayerPrefs.GetString("FountainRoom", "Room");

        if (System.Enum.TryParse(roomName, out RoomId savedRoomId))
        {
            foreach (var room in rooms)
            {
                if (room.roomScene == savedRoomId)
                {
                    PlayerGlobalPosition.Instance.currentRoomCoords = room.roomCoords;
                    break;
                }
            }
        }

        CheckRoomRevealed();
    }


    public void RevealRoom(RoomId nextRoom)
    {
        for (int i = 0; i < rooms.Length; i++)
        {
            if (rooms[i].roomScene == nextRoom)
            {
                rooms[i].gameObject.SetActive(true);
                rooms[i].hasBeenRevealed = true;

                PlayerGlobalPosition.Instance.currentRoomCoords = rooms[i].roomCoords;

                PlayerPrefs.SetInt($"Room Saved : {rooms[i].roomScene}", 1);
                PlayerPrefs.Save();

                return;
            }
        }
    }

    private void CheckRoomRevealed()
    {
        foreach (var room in rooms)
        {
            if (PlayerPrefs.HasKey($"Room Saved : {room.roomScene}") && PlayerPrefs.GetInt($"Room Saved : {room.roomScene}", 0) == 1)
            {
                room.gameObject.SetActive(true);
                room.hasBeenRevealed = true;
            }
        }
    }
}
