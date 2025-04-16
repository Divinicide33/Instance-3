using UnityEngine;

public class ItemToUnlock : MonoBehaviour
{
    [SerializeField] private ItemsName itemName;

    void Start()
    {
        // For debug
        /*PlayerPrefs.SetInt(itemName.ToString(), 0);
        PlayerPrefs.Save();*/

        if (PlayerPrefs.HasKey(itemName.ToString()) && PlayerPrefs.GetInt(itemName.ToString()) == 1) 
        gameObject.SetActive(false);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.layer == LayerMask.NameToLayer(LayerMap.Player.ToString()))
        {
            PlayerSkillManager.onSetItem?.Invoke(itemName, true);
            PlayerPrefs.SetInt(itemName.ToString(), 1);
            PlayerPrefs.Save();
            gameObject.SetActive(false);
        }
    }
}
