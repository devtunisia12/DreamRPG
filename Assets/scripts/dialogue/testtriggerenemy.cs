using UnityEngine;
using Unity.Cinemachine;

public class testtriggerenemy : MonoBehaviour
{
    public GameObject Camcombat, camgameplay, playerobj, lookcombatobj,combatUI;
    [Header("Camera")]
    public CinemachineCamera cam;
    public CinemachineFollow follow;
    public CinemachineConfiner2D confiner;

    [Header("Targets")]
    public Transform playerobjt;
    public Transform lookcombatobjt;

    public changingmusic musicchanger;
    public PlayerManager playermana;

    void Start()
    {
        camgameplay.SetActive(true);
        Camcombat.SetActive(false);
        combatUI.SetActive(false);
        cam.Follow = playerobjt;

    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            movement.CombatMode = true;
            BattleCharacter.pauseTimeline = false;
            cam.Follow = lookcombatobjt;
            Camcombat.SetActive(true);
            combatUI.SetActive(true);
            movement.isCombat = true;
            musicchanger.ChangeMusicToBattle();
            confiner.enabled = false;

        }
    }


}
