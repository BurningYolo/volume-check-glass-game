using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField]
    private GameObject pauseUI;

    public bool isGamePaused = false;
    private float previousTimeScale;

    private void Awake()
    {
        // Deactivate the pause UI at the start of the game
        pauseUI.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            TogglePause();
        }
    }

    private void TogglePause()
    {
        isGamePaused = !isGamePaused;

        if (isGamePaused)
        {
            PauseGame();
        }
        else
        {
            ResumeGame();
        }
    }

    private void PauseGame()
    {
        // Store the previous time scale
        previousTimeScale = Time.timeScale;

        // Set the time scale to 0 to pause the game
        Time.timeScale = 0f;

        // Activate the pause UI
        pauseUI.SetActive(true);
    }

    private void ResumeGame()
    {
        // Set the time scale back to the previous value to resume the game
        Time.timeScale = previousTimeScale;

        // Deactivate the pause UI
        pauseUI.SetActive(false);
    }
}
