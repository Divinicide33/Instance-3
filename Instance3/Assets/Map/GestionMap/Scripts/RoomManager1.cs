using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Cinemachine;

public class RoomManager1 : MonoBehaviour
{
    public static RoomManager1 Instance { get; private set; }

    [SerializeField] private RoomId startingRoom;
    [SerializeField] private CinemachineConfiner2D confiner;

    private RoomId currentRoom;
    private Dictionary<RoomId, Scene> loadedRooms = new();

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private async void Start()
    {
        GiveColliderForCinemachine.onColliderChange += UpdateConfinerWithRoom;
        await LoadRoom(startingRoom);
    }

    private void OnDestroy()
    {
        GiveColliderForCinemachine.onColliderChange -= UpdateConfinerWithRoom;
    }

    public async Task LoadRoom(RoomId newRoom)
    {
        if (loadedRooms.ContainsKey(newRoom))
        {
            Debug.Log("Room déjà chargée : " + newRoom);
            return;
        }

        string sceneName = newRoom.ToString();
        AsyncOperation loadOp = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        while (!loadOp.isDone)
            await Task.Yield();

        Scene loadedScene = SceneManager.GetSceneByName(sceneName);
        loadedRooms.Add(newRoom, loadedScene);
        currentRoom = newRoom;

        NotifyConfiner(loadedScene);
    }

    public async Task ChangeRoom(RoomId newRoom, Transform playerTransform, Vector3 newPosition)
    {
        await LoadRoom(newRoom);
        UnloadRoom(currentRoom);
        currentRoom = newRoom;

        playerTransform.position = newPosition;
    }

    private void UnloadRoom(RoomId room)
    {
        if (loadedRooms.TryGetValue(room, out Scene scene))
        {
            SceneManager.UnloadSceneAsync(scene);
            loadedRooms.Remove(room);
        }
    }

    private void UpdateConfinerWithRoom(PolygonCollider2D poly)
    {
        confiner.BoundingShape2D = poly;
        confiner.InvalidateBoundingShapeCache();
    }

    private void NotifyConfiner(Scene roomScene)
    {
        foreach (GameObject root in roomScene.GetRootGameObjects())
        {
            GiveColliderForCinemachine giver = root.GetComponentInChildren<GiveColliderForCinemachine>();
            if (giver != null)
            {
                // Déplace dans la scène active si nécessaire
                SceneManager.MoveGameObjectToScene(giver.gameObject, SceneManager.GetActiveScene());
                giver.SendCollider();
                return;
            }
        }

        Debug.LogWarning("Aucun GiveColliderForCinemachine trouvé dans : " + roomScene.name);
    }
}
