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
    public float fadetime = 1f;
    public float fadetimein = 1f;
    public float stoptime = 4f;
    public bool ChangeMusic = false;
    public int levelMusic=0;
    public static int globalMusic = 0;
    public changingmusic changeme;
    void Start()
    {
        BlackS.SetActive(false);
    }
    public void TeleportPlayer()
    {
        if (ChangeMusic == true)
        {
            if (levelMusic == 0)
            {
                globalMusic = levelMusic;
                changeme.ChangeBattleToMusic();
            }
            if (levelMusic == 1)
            {
                globalMusic = levelMusic;
                changeme.ChangeBattleToMusic2();
            }
            if (levelMusic == 2)
            {
                globalMusic = levelMusic;
                changeme.ChangeBattleToMusic3();
            }
        }
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
        yield return new WaitForSeconds(fadetime);
        TeleportPlayer();
        yield return new WaitForSeconds(fadetimein);
        BlackSanim.SetBool("fade", false);
        yield return new WaitForSeconds(stoptime);
        BlackS.SetActive(false);

    }
}
