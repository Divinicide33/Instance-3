using UnityEngine;

public class MapContainerData : MonoBehaviour
{
    public RoomId roomScene; // ton enum de salles
    public Vector2Int roomCoords; // coordonnées de la salle dans la map
    public Vector2 roomOriginOnMap; // coin haut gauche sur la minimap
    public Vector2 roomSize; // taille visuelle de la salle sur la minimap

    public bool hasBeenRevealed { get; set; }

    private void Awake()
    {
        if (MinimapData.Instance != null)
        {
            MinimapData.Instance.RegisterRoom(this);
        }
    }
}
