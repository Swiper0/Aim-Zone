using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float health = 100f;
    private float lifetime = 5f;
    public event System.Action OnDeath;

    private Renderer enemyRenderer;
    private Material enemyMaterial;
    private Color originalColor;
    private Color originalEmissionColor;

    private void Start()
    {
        enemyRenderer = GetComponent<Renderer>();
        if (enemyRenderer != null)
        {
            enemyMaterial = enemyRenderer.material;
            originalColor = enemyMaterial.color;
            originalEmissionColor = enemyMaterial.GetColor("_EmissionColor");
        }

        // Hancurkan target setelah lifetime berakhir
        Destroy(gameObject, lifetime);
    }

    public void SetLifetime(float newLifetime)
    {
        lifetime = newLifetime;

        // Update juga timer untuk menghancurkan musuh jika lifetime berubah
        Destroy(gameObject, lifetime);
    }

    // Fungsi untuk menerima damage
    public void TakeDamage(float amount)
    {
        health -= amount;

        if (health <= 0f)
        {

            Die();
        }
        else
        {
            // Tampilkan efek berkedip merah saat tertembak
            FlashRed();
        }
    }

    private void Die()
    {
        // Panggil event OnDeath untuk memberi tahu spawner
        OnDeath?.Invoke();

        // Tambahkan skor saat musuh dihancurkan
        if (ScoreManager.instance != null)
        {
            ScoreManager.instance.AddScore(1);
        }

        // Hancurkan musuh
        Destroy(gameObject);
    }

    // Fungsi untuk membuat musuh berkedip merah
    private void FlashRed()
    {
        if (enemyMaterial != null)
        {
            // Set warna material menjadi merah
            enemyMaterial.color = Color.red;

            // Matikan emission
            enemyMaterial.SetColor("_EmissionColor", Color.black);

            Invoke("ResetColor", 0.1f);
        }
    }

    // Fungsi untuk mengembalikan warna ke warna semula
    private void ResetColor()
    {
        if (enemyMaterial != null)
        {
            enemyMaterial.color = originalColor;

            // Aktifkan kembali emission ke warna awal
            enemyMaterial.SetColor("_EmissionColor", originalEmissionColor);
        }
    }
}
