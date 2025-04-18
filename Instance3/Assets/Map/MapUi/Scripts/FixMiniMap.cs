using UnityEngine;

public class FixMinimap : MonoBehaviour
{
    public Transform icon; // l'ic�ne du joueur sur la minimap

    public PlayerController playerController;

    void Update()
    {
        Vector2 playerInRoom = playerController.transform.position;

        Vector2Int room = PlayerGlobalPosition.Instance.currentRoomCoords;
        float roomSize = 19.5f; // d�pend de ton �chelle minimap

        Vector2 minimapPos = playerInRoom + ((Vector2)room * roomSize);

        icon.localPosition = minimapPos;

    }
}
