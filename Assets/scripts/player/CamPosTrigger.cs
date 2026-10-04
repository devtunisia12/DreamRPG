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

    [Header("Party")]
    public PlayerManager playerManager;
    public movement moveact;
    public SaveSystem saveme;
    void Start()
    {
        BlackS.SetActive(false);
    }
    public void TeleportPlayer()
    {
        Vector3 oldPlayerPosition = Player.transform.position;

        foreach (GameObject player in moveact.playersPos)
        {
            if (player != null)
            {
                Vector3 offset = player.transform.position - oldPlayerPosition;

                player.transform.position =
                    teleportPosition.position + offset;
            }
        }

        Player.transform.position = teleportPosition.position;
        saveme.newBoundsd = newBounds;
        confiner.BoundingShape2D = newBounds;
        confiner.InvalidateBoundingShapeCache();
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
