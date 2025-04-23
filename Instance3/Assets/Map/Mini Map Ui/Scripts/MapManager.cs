using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class MapManager : MonoBehaviour
{
    public static MapManager instance;

    [SerializeField] private GameObject largeMap;
    [SerializeField] private GameObject miniMapCamera;
    [SerializeField] private GameObject mapIcon;
    public bool isLargeMapActive { get; private set; }

    private float initialCameraPosition;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        CloseLargeMap();
        initialCameraPosition = -200;
    }

    public void OpenMap()
    {
        if (!isLargeMapActive)
        {
            OpenLargeMap();
        }
        else
        {
            CloseLargeMap();
        }
    }

    public void OpenLargeMap()
    {
        largeMap.SetActive(true);
        isLargeMapActive = true;
    }

    public void CloseLargeMap()
    {
        largeMap.SetActive(false);
        isLargeMapActive = false;
    }
}
