using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialSFX : MonoBehaviour
{
    public void PlayButtonClick()
    {
        if (AudioManager.Instance != null)
            AudioManager.Instance.PlayButtonClick();
    }

    public void StartStretchSFX()
    {
        if (AudioManager.Instance != null)
            AudioManager.Instance.StartStretchSFX();
    }

    public void StopStretchSFX()
    {
        if (AudioManager.Instance != null)
            AudioManager.Instance.StopStretchSFX();
    }
}
