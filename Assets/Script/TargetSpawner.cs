using UnityEngine;

public class TargetSpawner : MonoBehaviour
{
    public GameObject targetPrefab; // Prefab target
    public Vector3 spawnAreaSize = new Vector3(10f, 5f, 10f);
    public int maxSpawns = 30;
    private int currentSpawnCount = 0;
    private GameObject currentTarget; 
    private bool isSpawning = false;
    public bool isGameStarted = false;

    private float currentLifetime = 5f;
    public void StartSpawning()
    {
        if (!isGameStarted)
        {
            isSpawning = true;
            currentSpawnCount = 0;
            isGameStarted = true;
            SpawnTarget();
        }
    }

    // Method untuk berhenti spawn target
    public void StopSpawning()
    {
        isSpawning = false;
        currentSpawnCount = 0;
        isGameStarted = false; 


        if (currentTarget != null)
        {
            Destroy(currentTarget);
            currentTarget = null;
        }
    }

    // Method untuk mengubah mode
    public void SetMode(string mode)
    {
        if (isGameStarted) // Jangan ubah mode jika game sudah dimulai
        {
            return;
        }

        switch (mode.ToLower())
        {
            case "easy":
                currentLifetime = 5f;
                Debug.Log("Mode set to EASY. Lifetime: 5 seconds.");
                break;
            case "normal":
                currentLifetime = 3f;
                Debug.Log("Mode set to NORMAL. Lifetime: 3 seconds.");
                break;
            case "hard":
                currentLifetime = 1.2f;
                Debug.Log("Mode set to HARD. Lifetime: 1.2 seconds.");
                break;
            default:
                Debug.LogWarning("Invalid mode: " + mode);
                break;
        }
    }

    private void Update()
    {
        // Spawn target baru jika spawn diaktifkan dan target sudah habis
        if (isSpawning && currentSpawnCount < maxSpawns && currentTarget == null)
        {
            SpawnTarget();
        }

        // Jika mencapai batas spawn, hentikan permainan
        if (isSpawning && currentSpawnCount >= maxSpawns)
        {
            StopSpawning();
            Debug.Log("Game finished. All targets have been spawned.");
        }
    }

private void SpawnTarget()
{
    if (currentSpawnCount >= maxSpawns)
    {
        return;
    }

    // Jika ini adalah target pertama, reset skor
    if (currentSpawnCount == 0)
    {
        if (ScoreManager.instance != null)
        {
            ScoreManager.instance.ResetScore();
        }
        else
        {
            Debug.LogWarning("ScoreManager instance not found.");
        }
    }

    // Tentukan posisi spawn acak
    Vector3 spawnPosition = new Vector3(
        Random.Range(-spawnAreaSize.x / 2, spawnAreaSize.x / 2),
        Random.Range(-spawnAreaSize.y / 2, spawnAreaSize.y / 2),
        Random.Range(-spawnAreaSize.z / 2, spawnAreaSize.z / 2)
    );

    // Spawn target baru
    currentTarget = Instantiate(targetPrefab, transform.position + spawnPosition, Quaternion.identity);
    currentSpawnCount++;
    Debug.Log($"Spawned target #{currentSpawnCount} at position: {spawnPosition}");

    // Atur lifetime target jika memiliki komponen Enemy
    Enemy enemyScript = currentTarget.GetComponent<Enemy>();
    if (enemyScript != null)
    {
        enemyScript.SetLifetime(currentLifetime);
    }
    else
    {
        Debug.LogWarning("Spawned target does not have an Enemy script.");
    }
}
}
