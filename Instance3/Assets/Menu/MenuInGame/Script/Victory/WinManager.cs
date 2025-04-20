using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class WinManager : MonoBehaviour
{
    [Header("Input")]
    [SerializeField] private PlayerInput playerInput;

    [Header("Victory")]
    [SerializeField] private string sceneToLoadOnVictory = "VictoryScene";



    public static Action onVictory;

    private bool hasWon = false;

    private void Start()
    {
        onVictory += TriggerVictory;
    }

    private void OnDestroy()
    {
        onVictory -= TriggerVictory;
    }

    public static void WinGame() => onVictory?.Invoke();


    private void TriggerVictory()
    {
        if (hasWon) return;

        hasWon = true;
        Time.timeScale = 1f;
        LoadVictoryScene();
    }

    private void LoadVictoryScene()
    {
        if (!string.IsNullOrEmpty(sceneToLoadOnVictory))
        {
            SceneManager.LoadScene(sceneToLoadOnVictory);
        }
        else
        {
            Debug.LogError("No scene name assigned in 'sceneToLoadOnVictory'.");
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            WinGame();
        }
    }
}
