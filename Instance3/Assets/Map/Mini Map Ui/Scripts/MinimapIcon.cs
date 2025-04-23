using UnityEngine;

public class MinimapPlayerIcon : MonoBehaviour
{
    public Transform icon;   // L’icône du joueur sur la minimap
    public Transform player; // Le joueur dans la scène

    void Update()
    {
        Vector2Int currentRoom = PlayerGlobalPosition.Instance.currentRoomCoords;
        MapContainerData roomData = MinimapData.Instance.GetRoom(currentRoom);

        if (roomData == null) return;

        Vector2 roomOrigin = roomData.roomOriginOnMap;
        Vector2 roomSize = roomData.roomSize;

        Vector2 localPlayerPos = player.position;
        Vector2 normalizedPos = new Vector2(
            localPlayerPos.x / roomSize.x,
            localPlayerPos.y / roomSize.y
        );

        Vector2 finalMinimapPos = roomOrigin + Vector2.Scale(normalizedPos, roomSize);

        // Appliquer à l’icône
        Vector3 iconPos = icon.localPosition;
        iconPos.x = finalMinimapPos.x;
        iconPos.y = finalMinimapPos.y;
        icon.localPosition = iconPos;
    }
}
