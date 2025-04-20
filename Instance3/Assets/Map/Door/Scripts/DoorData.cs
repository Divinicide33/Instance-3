using UnityEngine;
[System.Serializable]
public class DoorData
{
    public RoomId room;
    public Vector3 position;

    public DoorData(RoomId room, Vector3 position)
    {
        this.room = room;
        this.position = position;
    }
}