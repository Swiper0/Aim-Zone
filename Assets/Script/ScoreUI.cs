using UnityEngine;
using TMPro;

public class ScoreUI : MonoBehaviour
{
    public TextMeshPro scoreText;

    private void Update()
    {
        if (ScoreManager.instance != null)
        {
            scoreText.text = "" + ScoreManager.instance.score;
        }
    }
}
