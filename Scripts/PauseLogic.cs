using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseLogic : MonoBehaviour
{
    public Canvas pauseCanvas;

    private void Start()
    {
        pauseCanvas.gameObject.SetActive(false);
        Time.timeScale = 1f;
    }

    public void Play()
    {
        pauseCanvas.gameObject.SetActive(false);
        Time.timeScale = 1f;
    }

    public void Pause()
    {
        Time.timeScale = 0f;
        pauseCanvas.gameObject.SetActive(true);
    }

    public void Menu()
    {
        DataContainer.ClearAllData();
        DataContainer.LoadPlayerData();
        DataContainer.checkpointIndex = 0;
        SceneManager.LoadScene("MainMenu");
    }

    public void Retry()
    {
        DataContainer.ClearPendings();
        DataContainer.checkpointIndex = 0;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
