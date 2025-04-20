using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ItemToUnlock : MonoBehaviour
{
    [SerializeField] private ItemsName itemName;

    void Start()
    {
        // For debug
        /*PlayerPrefs.SetInt(itemName.ToString(), 0);
        PlayerPrefs.Save();*/

        if (PlayerPrefs.HasKey(itemName.ToString() + SceneManager.GetSceneAt(1).name)) 
        {
            gameObject.SetActive(false);
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.layer == LayerMask.NameToLayer(LayerMap.Player.ToString()))
        {
            PlayerSkillManager.onSetItem?.Invoke(itemName, true);

            if (PlayerPrefs.HasKey(itemName.ToString()))
            {
                PlayerPrefs.SetInt(itemName.ToString(), PlayerPrefs.GetInt(itemName.ToString()) + 1);
            }
            else
            {
                PlayerPrefs.SetInt(itemName.ToString(), 1);
            }

            PlayerPrefs.SetString(itemName.ToString() + SceneManager.GetSceneAt(1).name, itemName.ToString() + SceneManager.GetSceneAt(1).name);

            PlayerPrefs.Save();

            DisplayPotions.onUpdate?.Invoke();

            gameObject.SetActive(false);
        }
    }
}
