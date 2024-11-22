using UnityEngine;

public class StartStopController : MonoBehaviour
{
    public TargetSpawner targetSpawner;

    public void StartGame()
    {
        Debug.Log("Game started.");
        targetSpawner.StartSpawning();
    }

    public void StopGame()
    {
        Debug.Log("Game stopped.");
        targetSpawner.StopSpawning();
    }
}
