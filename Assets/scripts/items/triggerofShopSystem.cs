using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class triggerofShopSystem : MonoBehaviour
{
    public GameObject UIComment, UIShop;

    public GameObject triggerTrigger;
    void Start()
    {
        UIShop.SetActive(false);
        UIComment.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            UIComment.SetActive(true);
            if (Input.GetKey(KeyCode.F))
            {
                openShopUI();
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
                openShopUI();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            UIComment.SetActive(false);
        }
    }

    public void openShopUI()
    {
        UIShop.SetActive(true);
    }
    public void gotoShop()
    {
        UIShop.SetActive(false);
        triggerTrigger.SetActive(true);
        StartCoroutine(doit());
    }
    IEnumerator doit()
    {
        yield return new WaitForSeconds(4f);
        triggerTrigger.SetActive(false);
    }
}
