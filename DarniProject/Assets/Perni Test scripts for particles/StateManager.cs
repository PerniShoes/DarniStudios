using UnityEngine;

public class StateManager : MonoBehaviour
{
    public static StateManager Instance { get; private set; }
    private GameObject gameStateCanvas;
    private GameObject pauseUi;

    private bool isPaused = false;
    public bool IsPaused => isPaused;

    void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            // In case there is already an Instance of StateManager
            Destroy(gameObject);
        }
    }
    void Start()
    {
        gameStateCanvas = GameObject.FindWithTag("GameStateCanvas");
        pauseUi = gameStateCanvas.transform.Find("PauseUI").gameObject;
    }

    void Update()
    {
        HandleInput();


    }
    private void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            PauseOrUnpauseGame();
        }


    }

    private void PauseOrUnpauseGame()
    {
        if (isPaused)
        {
            UnpauseGame();
        }
        else
        {
            PauseGame();
        }
    }

    private void PauseGame()
    {

        pauseUi.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
    }
    private void UnpauseGame()
    {

        pauseUi.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }


}
