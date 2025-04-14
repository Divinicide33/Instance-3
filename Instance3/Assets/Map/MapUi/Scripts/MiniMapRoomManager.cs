using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MiniMapRoomManager : MonoBehaviour
{
    public static MiniMapRoomManager instance;

    private MapContainerData[] rooms;

    [SerializeField] private GameObject dontDestroy; //remove this later

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        rooms = GetComponentsInChildren<MapContainerData>(true);
        DontDestroyOnLoad(dontDestroy);
    }

    private void Start()
    {
        RevealRoom();
    }

    public void RevealRoom()
    {
        string newLoadedScene = SceneManager.GetActiveScene().name;
        Debug.Log("New loaded scene: " + newLoadedScene);
        for (int i = 0; i < rooms.Length; i++)
        {
            if (rooms[i].roomScene.ToString() == newLoadedScene && !rooms[i].hasBeenRevealed)
            {
                rooms[i].gameObject.SetActive(true);
                rooms[i].hasBeenRevealed = true;
                return;
            }
        }
    }
}
