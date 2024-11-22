using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SensitivityController : MonoBehaviour
{
    public Models.PlayerSettingsModel playerSettings;
    
    private void Update()
    {
        // Mengurangi sensitivitas dengan tombol '-' atau '_'
        if (Input.GetKeyDown(KeyCode.Minus) || Input.GetKeyDown(KeyCode.Underscore))
        {
            AdjustSensitivity(-0.1f);
        }

        // Menambah sensitivitas dengan tombol '+' atau '='
        if (Input.GetKeyDown(KeyCode.Equals) || Input.GetKeyDown(KeyCode.Plus))
        {
            AdjustSensitivity(0.1f);
        }
    }


    private void AdjustSensitivity(float amount)
    {
        playerSettings.ViewXSensitivity += amount;
        playerSettings.ViewYSensitivity += amount;

        Debug.Log("New Sensitivity: " + playerSettings.ViewXSensitivity);
    }

}
