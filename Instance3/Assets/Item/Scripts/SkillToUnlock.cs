using UnityEngine;

public class SkillToUnlock : MonoBehaviour
{
    [SerializeField] private SkillsName skillName;

    void Start()
    {
        // For debug
        PlayerPrefs.SetInt(skillName.ToString(), 0);
        PlayerPrefs.Save();

        if (PlayerPrefs.HasKey(skillName.ToString()) && PlayerPrefs.GetInt(skillName.ToString()) == 1) 
        gameObject.SetActive(false);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer(LayerMap.Player.ToString()))
        {
            PlayerSkillManager.onSetSkill?.Invoke(skillName, true);
            PlayerPrefs.SetInt(skillName.ToString(), 1);
            PlayerPrefs.Save();
            gameObject.SetActive(false);
        }
    }
}
