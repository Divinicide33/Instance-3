using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CheatCodeManager : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1)) UnlockAll();

        if (Input.GetKeyDown(KeyCode.F2)) TpToZeus();

        if (Input.GetKeyDown(KeyCode.F3)) TpToHermes();

        if (Input.GetKeyDown(KeyCode.F4)) UnlockMap();

        if (Input.GetKeyDown(KeyCode.F5)) Invincible();

    }

    private void UnlockAll()
    {
        Debug.Log("UnlockAll");

        PlayerPrefs.SetInt(ItemsName.HpMax.ToString(), 10);
        PlayerPrefs.Save();

        PlayerController.onUpdatePlayerMaxHealth?.Invoke();

        PlayerSkillManager.onUnlockAll?.Invoke();
    }

    private void TpToZeus()
    {
        Debug.Log("TpToZeus");
        // use room manager
    }

    private void TpToHermes()
    {
        Debug.Log("TpToHermes");
        // use room manager
    }

    private void UnlockMap()
    {
        Debug.Log("UnlockMap");
        
        foreach (RoomId room in Enum.GetValues(typeof(RoomId)))
        {
            PlayerPrefs.SetInt($"Room Saved : {room}", 1);
        }

        PlayerPrefs.Save();
    }

    private void Invincible()
    {
        Debug.Log("Invincible");
        PlayerController.onSetTotallyInvincible?.Invoke();
        // make the player invincible iin PlayerController
    }
}
