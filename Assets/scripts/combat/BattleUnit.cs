using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.UI;

public class BattleUnit : MonoBehaviour
{
    public int maxHP = 100;
    public int currentHP;

    public int damage = 20;

    public Transform attackPoint;

    private Vector3 startPosition;

    public Animator animator;

    public bool attackFinished = false;

    public TextMeshProUGUI textHealthmis;
    public TextMeshProUGUI healthMaxtxt;

    public Slider HealthbarSlider;
    public Slider HealthbarSliderBig;
    public bool isPlayerorNot = false;
    public AudioSource HitedSFX;
    public AudioSource bodyHitedSFX;
    public AudioSource DeathSFX;
    public AudioSource SwingSFX;
    public AudioClip[] sounds;
    int lastSound = -1;



    void Start()
    {
        currentHP = maxHP;
        if (isPlayerorNot)
        {
            healthMaxtxt.text = maxHP.ToString();
        }
        startPosition = transform.position;
        textHealthmis.text = "";
        textHealthmis.gameObject.SetActive(false);
        animator = GetComponent<Animator>();
        if (animator != null)
        {
            animator.SetBool("run", false);
            animator.SetBool("die", false);
        }

    }

    void Update()
    {
        HealthbarSlider.value = currentHP;
        HealthbarSlider.maxValue = maxHP;
        if (isPlayerorNot)
        {
            HealthbarSliderBig.value = currentHP;
            HealthbarSliderBig.maxValue = maxHP;
            healthMaxtxt.text = currentHP.ToString();
        }


    }

    public IEnumerator AttackEnemy(BattleUnit enemy)
    {
        Vector3 original = transform.position;
        if (animator != null)
            animator.SetBool("run", true);
        while (Vector3.Distance(transform.position, enemy.transform.position) > 2.3f)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                enemy.transform.position,
                8f * Time.deltaTime
            );

            yield return null;
        }
        if (animator != null)
            animator.SetBool("run", false);




        yield return new WaitForSeconds(0.4f);
        FindObjectOfType<AttackTimingBar>(true)
        .StartBar(this, enemy);

        yield return new WaitUntil(() => attackFinished == true);

        if (animator != null)
        {
            animator.SetBool("returnready", false);
            animator.SetBool("Attack", true);
        }

        SwingSFX.Play();
        CombatUISystem.canireturn = true;
        yield return new WaitForSeconds(1f);
        animator.SetBool("Attack", false);
        if (animator != null)
            animator.SetBool("run", true);
        while (Vector3.Distance(transform.position, original) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                original,
                8f * Time.deltaTime
            );

            yield return null;
        }
        if (animator != null)
            animator.SetBool("run", false);
        textHealthmis.gameObject.SetActive(false);
        attackFinished = false;

    }

    public IEnumerator AttackPlayer(BattleUnit player, bool defended)
    {
        Vector3 original = transform.position;
        BattleCharacter.pauseTimeline = true;


        while (Vector3.Distance(transform.position, player.transform.position) > 1.2f)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                player.transform.position,
                8f * Time.deltaTime
            );

            yield return null;
        }

        CombatUISystem ui =
FindObjectOfType<CombatUISystem>();
        if (ui.isDefenseSelected)
        {
            if (player.animator != null)
                player.animator.SetBool("defbat", true);
            FindObjectOfType<AttackTimingBar>(true).StartBar(this, player);

            yield return new WaitUntil(() => attackFinished == true);
            CombatUISystem.canireturn = true;
            SwingSFX.Play();
            if (animator != null)
                animator.SetTrigger("Attack");


            yield return new WaitForSeconds(0.5f);
            if (AttackTimingBar.levelattacktoPlayer == 0)
            {
                player.TakeDamage(0);
            }
            else if (AttackTimingBar.levelattacktoPlayer == 1)
            {
                if (player.animator != null)
                    player.animator.SetBool("defbat", false);
                player.animator.SetTrigger("Hit");
                player.TakeDamage(20);
            }
            else
            {
                if (player.animator != null)
                    player.animator.SetBool("defbat", false);
                player.animator.SetTrigger("Hit");
                player.TakeDamage(40);
            }


            while (Vector3.Distance(transform.position, original) > 0.1f)
             {
                 transform.position = Vector3.MoveTowards(
                        transform.position,
                        original,
                        8f * Time.deltaTime
                 );

                 yield return null;
            }
            yield return new WaitForSeconds(1f);
            if (player.animator != null)
                player.animator.SetBool("defbat", false);
        }
        else
        {
            if (animator != null)
                animator.SetTrigger("Attack");

            yield return new WaitForSeconds(0.5f);


            player.TakeDamage(20);


            while (Vector3.Distance(transform.position, original) > 0.1f)
            {
                transform.position = Vector3.MoveTowards(
                    transform.position,
                    original,
                    8f * Time.deltaTime
                );

                yield return null;
            }
        }
        BattleCharacter.pauseTimeline = false;

        attackFinished = false;

    }


    public void PlaySoundHit()
    {
        int random = Random.Range(0, sounds.Length);

        while (random == lastSound && sounds.Length > 1)
            random = Random.Range(0, sounds.Length);

        lastSound = random;

        HitedSFX.PlayOneShot(sounds[random]);
    }



    public void TakeDamage(int amount)
    {
        PlaySoundHit();
        bodyHitedSFX.Play();
        currentHP -= amount;
        textHealthmis.text = "-" + amount.ToString();
        StartCoroutine(stopshowingHptext());
        CombatUISystem ui =
FindObjectOfType<CombatUISystem>();
        if (ui.isDefenseSelected == false)
        {
            animator.SetTrigger("Hit");
        }


        Debug.Log(name + " HP : " + currentHP);


        if (currentHP <= 0)
        {
            DeathSFX.Play();
            StartCoroutine(diesystem());
        }
    }
    
    IEnumerator stopshowingHptext()
    {
        textHealthmis.gameObject.SetActive(true);
        yield return new WaitForSeconds(1f);
        textHealthmis.gameObject.SetActive(false);

    }

    IEnumerator diesystem()
    {
        animator.SetBool("die", true);
        yield return new WaitForSeconds(2f);
        Die();
    }


    void Die()
    {
        CombatUISystem ui = FindObjectOfType<CombatUISystem>();

        if (ui != null && ui.playerManager != null)
        {
            if (isPlayerorNot)
            {
                ui.playerManager.players.Remove(gameObject);
            }
            else
            {
                ui.playerManager.enemies.Remove(gameObject);
            }
        }

        Destroy(gameObject);

        if (ui != null)
        {
            ui.CheckWin();
        }
    }

}