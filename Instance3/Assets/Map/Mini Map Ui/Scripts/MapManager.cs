using UnityEngine;
using UnityEngine.SceneManagement;

public class MapManager : MonoBehaviour
{
    public static MapManager instance;

    [SerializeField] private GameObject largeMap;
    [SerializeField] private GameObject miniMapCamera;
    [SerializeField] private GameObject mapIcon;

    bool test = false;

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
        largeMap.SetActive(true);
        isLargeMapActive = true;
        //if (SceneManager.GetSceneByName("Tutorial2Room").isLoaded)
        //{
        //    miniMapCamera.transform.position = new Vector3(0, miniMapCamera.transform.position.y, miniMapCamera.transform.position.z);
        //    if (!test)
        //    {
        //        initialCameraPosition = mapIcon.transform.position.x - 100;
        //        mapIcon.transform.position = new Vector3(initialCameraPosition, mapIcon.transform.position.y, mapIcon.transform.position.z);
        //        test = true;
        //    }
        //    initialCameraPosition = mapIcon.transform.position.x;
        //    mapIcon.transform.position = new Vector3(initialCameraPosition, mapIcon.transform.position.y, mapIcon.transform.position.z);
        //}
        //else
        //{
        //    miniMapCamera.transform.position = new Vector3(-10, miniMapCamera.transform.position.y, miniMapCamera.transform.position.z);
        //    initialCameraPosition = mapIcon.transform.position.x;
        //    mapIcon.transform.position = new Vector3(initialCameraPosition, mapIcon.transform.position.y, mapIcon.transform.position.z);
        //}
        
    }

    public void CloseLargeMap()
    {
        largeMap.SetActive(false);
        isLargeMapActive = false;
    }
}
