using UnityEngine;
using UnityEngine.SceneManagement;

public class pausesystem : MonoBehaviour
{
    public bool isPaused = false;

    [Header("Main")]
    public GameObject pauseMenuObj;

    [Header("Pages")]
    public GameObject itemPanel;
    public GameObject equipPanel;
    public GameObject optionsPanel;
    public GameObject exitPanel;

    [Header("Selector")]
    public RectTransform selector;

    public float[] selectorY = { 100, 40, -20 };

    private int menuChoice = 0;


    void Start()
    {
        pauseMenuObj.SetActive(false);

        ShowPage();
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
                Resume();
            else
                Pause();

            return;
        }

        if (!isPaused)
            return;


        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            menuChoice--;

            if (menuChoice < 0)
                menuChoice = 3;

            ShowPage();
        }


        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            menuChoice++;

            if (menuChoice > 3)
                menuChoice = 0;

            ShowPage();
        }
    }


    void ShowPage()
    {
        itemPanel.SetActive(menuChoice == 0);
        equipPanel.SetActive(menuChoice == 1);
        optionsPanel.SetActive(menuChoice == 2);
        exitPanel.SetActive(menuChoice == 3);


        Vector3 pos = selector.localPosition;
        pos.y = selectorY[menuChoice];
        selector.localPosition = pos;
    }


    public void Pause()
    {
        Time.timeScale = 0;
        isPaused = true;

        pauseMenuObj.SetActive(true);

        ShowPage();
    }


    public void Resume()
    {
        Time.timeScale = 1;
        isPaused = false;

        pauseMenuObj.SetActive(false);
    }


    public void QuitGame()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("mainmenu");
    }
}