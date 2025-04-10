using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RoomManager : MonoBehaviour
{
    public static RoomManager Instance { get; private set; }

    public static Action<RoomId, Transform , Transform> onDoorUsed;

    [SerializeField] private RoomId currentRoom;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
        LoadScene(currentRoom);
    }

    private void OnEnable()
    {
        onDoorUsed += OnPlayerEnterRoom;
    }

    private void OnDisable()
    {
        onDoorUsed -= OnPlayerEnterRoom;
    }

    public void OnPlayerEnterRoom(RoomId newRoom, Transform playerPosition, Transform newPosition)
    {
        ChangeRoom(newRoom);
        ChangePlayerPosition(playerPosition, newPosition);
    }

    private void ChangePlayerPosition(Transform playerPosition, Transform newPosition)
    {
        playerPosition.position = newPosition.position;
    }

    public void ChangeRoom(RoomId newRoom)
    {
        if (currentRoom == newRoom)
            return;

        LoadScene(newRoom);
        //UnloadScene(currentRoom);

        currentRoom = newRoom;

    }

    private static string GetSceneName(RoomId room)
    {
        return room.ToString();
    }

    private void LoadScene(RoomId room)
    {
        string sceneName = GetSceneName(room);
        SceneManager.LoadScene(sceneName);
        
        Debug.Log("Scène initiale chargée : " + sceneName);
    }

/*    private async void UnloadScene(RoomId room)
    {
        string sceneName = GetSceneName(room);
        AsyncOperation unloadOp = SceneManager.UnloadSceneAsync(sceneName);
        while (!unloadOp.isDone)
        {
            await Task.Yield();
        }
        loadedRooms.Remove(room);
        Debug.Log("Déchargé : " + sceneName);
    }*/

}