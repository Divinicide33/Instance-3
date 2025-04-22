using System.Collections.Generic;
using UnityEngine;

public class MinimapData : MonoBehaviour
{
    public static MinimapData Instance;

    private Dictionary<Vector2Int, MapContainerData> rooms = new();

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    public void RegisterRoom(MapContainerData room)
    {
        if (!rooms.ContainsKey(room.roomCoords))
        {
            rooms[room.roomCoords] = room;
        }
        else
        {
            Debug.LogWarning($"Une salle est déjà enregistrée à la position {room.roomCoords}");
        }
    }

    public MapContainerData GetRoom(Vector2Int coords)
    {
        if (rooms.TryGetValue(coords, out var room))
            return room;

        Debug.LogWarning($"Aucune salle trouvée à {coords}");
        return null;
    }
}
