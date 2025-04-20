using UnityEngine;

public class MapManager : MonoBehaviour
{
    public static MapManager instance;

    [SerializeField] private GameObject miniMap;
    [SerializeField] private GameObject largeMap;
    [SerializeField] private GameObject miniMapCamera;

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
        miniMapCamera.transform.position = new Vector3(miniMapCamera.transform.position.x, miniMapCamera.transform.position.y, -200f);
    }

    public void CloseLargeMap()
    {
        miniMap.SetActive(true);
        largeMap.SetActive(false);
        isLargeMapActive = false;
        miniMapCamera.transform.position = new Vector3(miniMapCamera.transform.position.x, miniMapCamera.transform.position.y, -25f);
    }
}
