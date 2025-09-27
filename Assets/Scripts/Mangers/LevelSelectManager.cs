using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class LevelSelectManager : MonoBehaviour
{
    public GameObject levelButtonsParent;
    public Sprite filledStar;
    public Text TotalStars; // Assign in inspector

    private Button[] levelButtons;
    private int unlockedLevel;

    void Awake()
    {
        unlockedLevel = PlayerPrefs.GetInt("CurrentLevelNO", 0);
        levelButtons = levelButtonsParent.GetComponentsInChildren<Button>();

        int totalStarsCount = 0;

        for (int i = 0; i < levelButtons.Length; i++)
        {
            levelButtons[i].interactable = i <= unlockedLevel;

            int stars = PlayerPrefs.GetInt($"Level_{i}_Stars", 0);
            totalStarsCount += stars;

            Transform buttonTransform = levelButtons[i].transform;
            // Reverse order: 3, 2, 1 (skip 0, which is text)
            for (int s = 3; s >= 1; s--)
            {
                Transform starChild = buttonTransform.GetChild(s);
                Image starImage = starChild.GetComponent<Image>();
                if (starImage != null)
                {
                    // Fill stars according to score
                    if ((3 - s) < stars && filledStar != null)
                        starImage.sprite = filledStar;
                    // else leave as is
                }
            }

            int levelIndex = i;
            levelButtons[i].onClick.AddListener(() => OnLevelButtonClicked(levelIndex));
        }

        // Set total stars text in "x/30" format
        if (TotalStars != null)
            TotalStars.text = $"{totalStarsCount}/30";
    }

    void OnLevelButtonClicked(int levelIndex)
    {
        PlayerPrefs.SetInt("SelectedLevelNO", levelIndex);
        PlayerPrefs.Save();

        if(AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayButtonClick();
        }
        StartCoroutine(TransitionToGameMusic());
    }
    
    private IEnumerator TransitionToGameMusic()
    {
        if(AudioManager.Instance != null)
        {
            yield return StartCoroutine(AudioManager.Instance.FadeMusic(0f, 1f));
        }
        SceneManager.LoadScene(1);
    }
}