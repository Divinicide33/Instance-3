using UnityEngine;

namespace Save
{
    public class ResetPlayerPrefs : MonoBehaviour
    {
        public void ResetAllPrefs()
        {
            PlayerPrefs.DeleteAll();
            PlayerPrefs.Save();
            Debug.Log("✅ Reset PlayerPrefs.");
        }
    }
}