using UnityEngine;
using Unity.Cinemachine;
using System.Collections;
using System.Collections.Generic;

public class CamPosTrigger : MonoBehaviour
{
    [Header("Teleport")]
    public Transform teleportPosition;
    public Transform Player;

    [Header("Camera")]
    public CinemachineConfiner2D confiner;
    public Collider2D newBounds;
    public GameObject BlackS;
    public Animator BlackSanim;

    void Start()
    {
        BlackS.SetActive(false);
    }
    public void TeleportPlayer()
    {
        Player.transform.position = teleportPosition.position;
        confiner.BoundingShape2D = newBounds;
        confiner.InvalidateBoundingShapeCache();
        BlackSanim = BlackS.GetComponent<Animator>();

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(TeleWait());
        }
    }

    private IEnumerator TeleWait()
    {
        BlackS.SetActive(true);
        BlackSanim.SetBool("fade", true);
        yield return new WaitForSeconds(1f);
        TeleportPlayer();
        yield return new WaitForSeconds(2f);
        BlackSanim.SetBool("fade", false);
        yield return new WaitForSeconds(4f);
        BlackS.SetActive(false);

    }
}
