using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

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
    private int comboLevel = 0;

    public float comboSpeedIncrease = 100f;
    public float comboGreenShrink = 15f;
    public float minimumGreenWidth = 30f;

    private float originalSpeed;
    private float originalGreenWidth;

    void Start()
    {
        originalSpeed = speed;
        originalGreenWidth = normalZone.sizeDelta.x;
    }

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

        // PERFECT
        float greenHalf = normalZone.sizeDelta.x / 2f;

        if (pos >= -greenHalf && pos <= greenHalf)
        {
            damageMultiplier = 3f;

            Debug.Log("Perfect Hit!");

            comboLevel++;
            float newWidth =
    Mathf.Max(
        originalGreenWidth - comboGreenShrink * comboLevel,
        minimumGreenWidth
    );

            normalZone.sizeDelta = new Vector2(
                newWidth,
                normalZone.sizeDelta.y
            );
            attacker.animator.SetBool("Attack", true);
            StartCoroutine(NextPerfectAttack());
            return;

            speed = originalSpeed + comboSpeedIncrease * comboLevel;



            // Follow-up
            StartBar(attacker, target);

            return;
        }


        // NORMAL
        else if (
            (pos >= -84f && pos < -greenHalf) ||
            (pos > greenHalf && pos <= 84f)
        )
        {
            damageMultiplier = 1f;

            Debug.Log("Normal Hit!");

            target.TakeDamage(
                Mathf.RoundToInt(
                    attacker.damage * damageMultiplier
                )
            );

            attacker.attackFinished = true;
            EndCombo();

            return;
        }

        // MISS
        else
        {
            damageMultiplier = 0f;

            Debug.Log("Miss!");

            attacker.attackFinished = true;

            EndCombo();

            return;
        }
    }

    void EndCombo()
    {
        comboLevel = 0;
        attacker.animator.SetBool("run", true);
        attacker.animator.SetBool("returnready", false);
        speed = originalSpeed;

        normalZone.sizeDelta = new Vector2(
            originalGreenWidth,
            normalZone.sizeDelta.y
        );

        marker.anchoredPosition = new Vector2(-250, 0);

        gameObject.SetActive(false);
    }

    IEnumerator NextPerfectAttack()
    {
        attacker.animator.SetBool("returnready", false);
        attacker.SwingSFX.Play();

        yield return new WaitForSeconds(0.3f);

        target.TakeDamage(
            Mathf.RoundToInt(
                attacker.damage * 3f
            )
        );

        yield return new WaitForSeconds(0.4f);

        if (target == null || target.currentHP <= 0)
        {
            attacker.attackFinished = true;
            EndCombo();
            gameObject.SetActive(false);
            yield break;
        }
        attacker.animator.SetBool("Attack", false);
        attacker.animator.SetBool("returnready", true);

        StartBar(attacker, target);
    }

    void StopBarEnemy()
    {
        moving = false;
        float pos = marker.anchoredPosition.x;

        if (pos >= -30f && pos <= 30f)
        {
            levelattacktoPlayer = 0;
            Debug.Log("Perfect Hit!");
        }
        else if ((pos >= -84f && pos < -30f) ||
                 (pos > 30f && pos <= 84f))
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
        attacker.animator.SetBool("run", true);
        attacker.animator.SetBool("returnready", false);
        gameObject.SetActive(false);
    }
}