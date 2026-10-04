using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class introsystem : MonoBehaviour
{
    public Image img;
    public Animator fademe;
    bool finished = false;
    public float timeofvideo = 6f;
    void Start()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
        Debug.Log("Save deleted!");
        img.fillAmount = 0f;
        fademe.SetBool("fade", false);
        finished = false;
        StartCoroutine(Startvideo());
    }
    void Update()
    {
        if (Input.GetKey(KeyCode.E))
        {
            img.fillAmount += 0.5f *Time.deltaTime;
            if (img.fillAmount >=1f)
            {
                img.fillAmount = 1f;
                StartCoroutine(GotoLevel());
            }
        }
        else
        {
            if (img.fillAmount > 0f && finished==false)
            {
                img.fillAmount -=0.5f * Time.deltaTime;
            }
        }
    }

    public void Newlevel()
    {
        SceneManager.LoadScene("SampleScene");
    }

    IEnumerator GotoLevel()
    {
        finished = true;
        fademe.SetBool("fade", true);
        yield return new WaitForSeconds(1f);
        Newlevel();
    }


    IEnumerator Startvideo()
    {
        yield return new WaitForSeconds(timeofvideo);
        if (finished == false)
        {
            StartCoroutine(GotoLevel());
        }
    }
}
