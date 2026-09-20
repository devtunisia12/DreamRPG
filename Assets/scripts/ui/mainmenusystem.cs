using UnityEngine;
using UnityEngine.SceneManagement;

public class mainmenusystem : MonoBehaviour
{
    public string levelName;
    public float[] listPositionY = { 100, 40, -20, -80,-140 };
    public float[] listPositionX= { 100, 40, -20, -80, -140 };
    public AudioSource audioSelect;
    public AudioSource audioConfirm;

    public int indexmenu = 0;
    public RectTransform imageChoice;

    void Start()
    {
        indexmenu = 0;
        audioConfirm = GetComponent<AudioSource>();
        audioSelect = GetComponent<AudioSource>();
    }
    public void LoadLevel()
    {
        SceneManager.LoadScene(levelName);
    }

    public void Newlevel()
    {
        SceneManager.LoadScene("SampleScene");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    void Update()
    {
       
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            audioSelect.Play();
            indexmenu--;
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            audioSelect.Play();
            indexmenu++;
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (indexmenu == 1)
            {
                audioConfirm.Play();
                Newlevel();
            }

            if (indexmenu == 4)
            {
                audioConfirm.Play();
                QuitGame();
            }
        }


            if (indexmenu >= listPositionY.Length)
            indexmenu = 0;
        if (indexmenu < 0)
            indexmenu = listPositionY.Length - 1;
        imageChoice.anchoredPosition = new Vector2(listPositionX[indexmenu], listPositionY[indexmenu]);
    }
}
