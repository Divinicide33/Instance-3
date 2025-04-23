using UnityEngine;

public class PlayerGlobalPosition : MonoBehaviour
{
    public static PlayerGlobalPosition Instance;

    public Vector2Int currentRoomCoords;

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
