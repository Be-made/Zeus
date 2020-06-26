using TMPro;
using UnityEngine;

public class UIService : IService
{
    TextMeshProUGUI timer;
    TextMeshProUGUI score;
    GameObject ui;
    GameObject pauseMenuPanel;
    public void StartService(EngineScript instance)
    {
        timer = GameObject.Find("Timer").GetComponent<TextMeshProUGUI>();
        score = GameObject.Find("Score").GetComponent<TextMeshProUGUI>();
        ui    = GameObject.Find("UI");
        pauseMenuPanel = GameObject.Find("PauseMenu");
        pauseMenuPanel.SetActive(false);
    }

    int currentScore = 0;
    public void SetScore(int delteScore)
    {
        currentScore += delteScore;
        score.text    = $"Score: { currentScore }";
    }

    public void SetTimer(int seconds)
    {
        timer.text = $"Timer: { seconds }";
    }

    public void ActivatePauseMenu()
    {
        pauseMenuPanel.SetActive(true);
    }

    public void DeactivatePauseMenu()
    {
        pauseMenuPanel.SetActive(false);
    }
}
