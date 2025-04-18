using System.Collections;
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
    [SerializeField] private MiniMapRoomManager miniMapRoomManager;

    private RoomId currentRoom;
    private Dictionary<RoomId, Scene> loadedRooms = new();

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        GiveColliderForCinemachine.onColliderChange += UpdateConfinerWithRoom;
        StartCoroutine(LoadRoomCoroutine(startingRoom));
    }

    private void OnDestroy()
    {
        GiveColliderForCinemachine.onColliderChange -= UpdateConfinerWithRoom;
    }

    public void ChangeRoomWithFade(RoomId newRoom, Transform playerTransform, Vector3 newPosition)
    {
        StartCoroutine(ChangeRoomCoroutine(newRoom, playerTransform, newPosition));
    }

    private IEnumerator ChangeRoomCoroutine(RoomId newRoom, Transform playerTransform, Vector3 newPosition)
    {
        //Debug.Log("Changement de salle avec fondu : " + newRoom);
        
        bool fadeInComplete = false;
        FadeInOut.Instance.FadeIn(() => fadeInComplete = true);
        yield return new WaitUntil(() => fadeInComplete);
        
        yield return UnloadRoomCoroutine(currentRoom);
        
        playerTransform.position = newPosition;
        
        rooms = newRoom;
        currentRoom = newRoom;
        yield return LoadRoomCoroutine(newRoom);
        
        FadeInOut.Instance.FadeOut();

        miniMapRoomManager?.RevealRoom();
    }

    private IEnumerator LoadRoomCoroutine(RoomId newRoom)
    {
        if (loadedRooms.ContainsKey(newRoom))
            yield break;

        string sceneName = newRoom.ToString();
        AsyncOperation loadOp = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        while (!loadOp.isDone)
            yield return null;

        Scene loadedScene = SceneManager.GetSceneByName(sceneName);
        loadedRooms.Add(newRoom, loadedScene);
        currentRoom = newRoom;

        NotifyConfiner(loadedScene);
        NotifyEnemies(loadedScene);
    }

    private IEnumerator UnloadRoomCoroutine(RoomId room)
    {
        if (loadedRooms.TryGetValue(room, out Scene scene))
        {
            AsyncOperation unloadOp = SceneManager.UnloadSceneAsync(scene);
            while (!unloadOp.isDone)
                yield return null;

            loadedRooms.Remove(room);
        }
    }

    #region Notify
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
    #endregion
    
}
