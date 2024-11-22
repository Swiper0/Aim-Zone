using UnityEngine;
using TMPro;

public class ModeController : MonoBehaviour
{
    public TargetSpawner targetSpawner;
    public TextMeshPro modeText;
    private string currentMode = "";

    void Update()
    {

        // Jika game sudah dimulai, mode tidak bisa diubah
        if (targetSpawner.isGameStarted)
        {
            return; // Menghentikan eksekusi jika game sudah dimulai
        }

        // Mode pemilihan jika game belum dimulai
        if (Input.GetKeyDown(KeyCode.Alpha1)) // Tombol 1 untuk Easy
        {
            currentMode = "Easy"; // Set mode ke Easy
            targetSpawner.SetMode("Easy");
            UpdateModeText("Easy");
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2)) // Tombol 2 untuk Normal
        {
            currentMode = "Normal"; // Set mode ke Normal
            targetSpawner.SetMode("Normal");
            UpdateModeText("Normal");
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3)) // Tombol 3 untuk Hard
        {
            currentMode = "Hard"; // Set mode ke Hard
            targetSpawner.SetMode("Hard");
            UpdateModeText("Hard");
        }

        Debug.Log("Current mode: " + currentMode);
    }

    // Fungsi untuk memperbarui teks UI dengan mode yang dipilih
    void UpdateModeText(string mode)
    {
        if (modeText != null)
        {
            modeText.text = "" + mode;
        }
    }

    // Fungsi untuk memulai permainan dan menandai bahwa game sudah dimulai
    public void StartGame()
    {
        targetSpawner.StartSpawning();

    }
}
