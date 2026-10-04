using UnityEngine;

public class triggerOfSaveSystem : MonoBehaviour
{
    public GameObject UIComment, UISave;

    public SaveSystem saveme;
    void Start()
    {
        UISave.SetActive(false);
        UIComment.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            UIComment.SetActive(true);
            if (Input.GetKey(KeyCode.F))
            {
                OpenSaveUi();
            }
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            UIComment.SetActive(true);
            if (Input.GetKey(KeyCode.F))
            {
                OpenSaveUi();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            UIComment.SetActive(false);
            UISave.SetActive(false);
        }
    }

    public void OpenSaveUi()
    {
        UISave.SetActive(true);
    }

    public void ClickSaveUI()
    {
        UISave.SetActive(false);
        saveme.SaveGame();
    }
}
