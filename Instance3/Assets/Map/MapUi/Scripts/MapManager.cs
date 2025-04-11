using UnityEngine;

public class MapManager : MonoBehaviour
{
    public static MapManager instance;

    [SerializeField] private GameObject miniMap;
    [SerializeField] private GameObject largeMap;

    public bool isLargeMapActive { get; private set; }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        CloseLargeMap();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
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
    }

    public void OpenLargeMap()
    {
        miniMap.SetActive(false);
        largeMap.SetActive(true);
        isLargeMapActive = true;
    }

    public void CloseLargeMap()
    {
        miniMap.SetActive(true);
        largeMap.SetActive(false);
        isLargeMapActive = false;
    }
}
