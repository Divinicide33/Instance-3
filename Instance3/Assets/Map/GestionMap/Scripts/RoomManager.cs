using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Cinemachine;

public class RoomManager : MonoBehaviour
{
    public static RoomManager Instance { get; private set; }

    public RoomId rooms;

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
            Debug.Log("Room d�j� charg�e : " + newRoom);
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
        NotifyEnemies(loadedScene);
    }

    public async Task ChangeRoom(RoomId newRoom, Transform playerTransform, Vector3 newPosition)
    {
        Debug.Log("Changement de salle : " + newRoom);
        
        PlayerInputScript.onDisableInput?.Invoke();
        await UnloadRoom(currentRoom);
        
        playerTransform.position = newPosition;
        rooms = newRoom;
        currentRoom = newRoom;
        
        await LoadRoom(newRoom);
        PlayerInputScript.onEnableInput?.Invoke();
    }

    private async Task UnloadRoom(RoomId room)
    {
        if (loadedRooms.TryGetValue(room, out Scene scene))
        {
            await SceneManager.UnloadSceneAsync(scene);
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
                // D�place dans la sc�ne active si n�cessaire
                //SceneManager.MoveGameObjectToScene(giver.gameObject, SceneManager.GetActiveScene());
                giver.SendCollider(confiner.gameObject);
                StartCoroutine(giver.ReloadCollider(confiner.gameObject));
                return;
            }
        }

        Debug.LogWarning("Aucun GiveColliderForCinemachine trouv� dans : " + roomScene.name);
    }
    
    private void NotifyEnemies(Scene roomScene)
    {
        foreach (GameObject root in roomScene.GetRootGameObjects())
        {
            GivePlayerForEnnemy giver = root.GetComponentInChildren<GivePlayerForEnnemy>();
            if (giver != null)
            {
                giver.TryFindAndSendPlayer();
                return;
            }
        }

        Debug.LogWarning("Aucun GivePlayerForEnnemy trouvé dans : " + roomScene.name);
    }

    
}
