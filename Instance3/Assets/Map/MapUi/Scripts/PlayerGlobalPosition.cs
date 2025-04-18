using UnityEngine;

public class PlayerGlobalPosition : MonoBehaviour
{
    public static PlayerGlobalPosition Instance;

    public Vector2Int currentRoomCoords; // Ex: (0,0), (1,0), (-1,1) etc.

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
