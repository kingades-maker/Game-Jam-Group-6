using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameOverUI : MonoBehaviour
{
    public GameObject gameOverPanel;
    public Button yesButton;
    public Button noButton;
    public ShapeSpawner spawner;

    void Start()
    {
        HideGameOver();
        yesButton.onClick.AddListener(OnYesClicked);
        noButton.onClick.AddListener(OnNoClicked);
    }

    public void ShowGameOver()
    {
        gameOverPanel.SetActive(true);
    }

    public void HideGameOver()
    {
        gameOverPanel.SetActive(false);
    }

    void OnYesClicked()
    {
        if (spawner != null)
        {
            spawner.RestartGame();
        }
    }

    void OnNoClicked()
    {
        if (spawner != null)
        {
            spawner.EndGame();
        }
    }
}