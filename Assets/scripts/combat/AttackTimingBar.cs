using UnityEngine;
using UnityEngine.UI;

public class AttackTimingBar : MonoBehaviour
{
    public RectTransform marker;
    public RectTransform redZone;
    public RectTransform normalZone;

    public float speed = 300f;

    private bool moving = false;
    private float direction = 1;

    private float damageMultiplier = 0;
    private BattleUnit attacker;
    private BattleUnit target;
    public static int levelattacktoPlayer = 3;


    void Update()
    {

        if (!moving)
            return;


        marker.anchoredPosition += Vector2.right * speed * direction * Time.deltaTime;


        if (marker.anchoredPosition.x > 250)
        {
            direction = -1;
        }

        if (marker.anchoredPosition.x < -250)
        {
            direction = 1;
        }


        if (Input.GetKeyDown(KeyCode.Space))
        {

            CombatUISystem ui =
    FindObjectOfType<CombatUISystem>();
            if (ui.isDefenseSelected)
            {
                StopBarEnemy();
            }
            else
            {
                StopBar();
            }
        }
    }



    public void StartBar(BattleUnit player, BattleUnit enemy)
    {
        attacker = player;
        target = enemy;
        direction = 1;
        marker.anchoredPosition = new Vector2(-250, 0);

        moving = true;

        gameObject.SetActive(true);
    }


    
    void StopBar()
    {
        moving = false;
        float pos = marker.anchoredPosition.x;


        if (pos >= -79f && pos <= 79f)
        {
            damageMultiplier = 3f;
            Debug.Log("Perfect Hit!");
        }
        else if ((pos >= -145f && pos < -80f) ||
                 (pos > 80f && pos <= 145f))
        {
            damageMultiplier = 1f;
            Debug.Log("Normal Hit!");
        }
        else
        {
            damageMultiplier = 0;
            Debug.Log("Miss!");
        }


        target.TakeDamage(
        Mathf.RoundToInt(attacker.damage * damageMultiplier)
        );
        attacker.attackFinished = true;


        gameObject.SetActive(false);
        marker.anchoredPosition = new Vector2(-250, 0);

    }

    void StopBarEnemy()
    {
        moving = false;
        float pos = marker.anchoredPosition.x;

        if (pos >= -79f && pos <= 79f)
        {
            levelattacktoPlayer = 0;
            Debug.Log("Perfect Hit!");
        }
        else if ((pos >= -145f && pos < -80f) ||
                 (pos > 80f && pos <= 145f))
        {
            levelattacktoPlayer = 1;
            Debug.Log("Normal Hit!");
        }
        else
        {
            levelattacktoPlayer = 2;
            Debug.Log("Miss!");
        }


        attacker.attackFinished = true;


        gameObject.SetActive(false);
    }
}