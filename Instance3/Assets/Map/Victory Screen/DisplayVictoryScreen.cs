using System.Net.Sockets;
using UnityEngine;

public class DisplayVictoryScreen : MonoBehaviour
{
    [SerializeField] private GameObject finalBossPrefab;

    private void OnEnable()
    {
        DoorBoss.onRemoveEnemy += TriggerVictoryScreen;
    }

    private void OnDisable()
    {
        DoorBoss.onRemoveEnemy -= TriggerVictoryScreen;
    }

    private void TriggerVictoryScreen(Enemy enemy)
    {
        if (enemy.gameObject.name == finalBossPrefab.name)
        {
            WinManager.onVictory?.Invoke();
        }
    }
}
