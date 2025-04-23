using UnityEngine;

public class MapContainerData : MonoBehaviour
{
    public RoomId roomScene;
    public Vector2Int roomCoords;
    public Vector2 roomOriginOnMap;
    public Vector2 roomSize;

    public bool hasBeenRevealed { get; set; }

    private void Awake()
    {
        if (MinimapData.Instance != null)
        {
            MinimapData.Instance.RegisterRoom(this);
        }
    }
}
