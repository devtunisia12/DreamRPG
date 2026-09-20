using UnityEngine;
using UnityEngine.UI;

public enum Team
{
    Player,
    Enemy
}



public class BattleCharacter : MonoBehaviour
{
    public Team team;
    public static bool pauseTimeline;

    public enum BattleState
    {
        Waiting,
        Ready,
        Acting
    }

    public enum PlayerAction
    {
        None,
        Attack,
        Defense,
        Item,
        Mercy
    }

    public PlayerAction selectedAction = PlayerAction.None;

    public int selectedEnemyIndex = -1;

    public BattleState state = BattleState.Waiting;

    public float speed = 25f;
    public bool isPlayeror = false;
    private bool actionStarted = false;
    public GameObject healthbar;
    [HideInInspector]
    public float progress;

    public Image iconPrefab;
    public Sprite iconSprite;
    public Animator animator;
    private Image icon;

    public void Start()
    {
        animator = GetComponent<Animator>();
        animator.SetBool("transform", false);

        if (isPlayeror)
        {
            healthbar.SetActive(false);
            speed = Random.Range(5f, 10f);
        }
        else
        {
            speed = Random.Range(1f, 4f);
        }
    }
    public void CreateIcon(Transform parent)
    {
        icon = Instantiate(iconPrefab, parent);
        icon.sprite = iconSprite;
    }

    private void OnDestroy()
    {
        if (icon != null)
        {
            Destroy(icon.gameObject);
        }
    }

    void Update()
    {
        if (pauseTimeline)
            return;


        if (team == Team.Player && state == BattleState.Waiting)
        {
            progress += speed * Time.deltaTime;

            if (progress >= 80)
            {
                progress = 80;
                state = BattleState.Ready;
                healthbar.SetActive(true);
                animator.SetBool("transform", true);
                CombatUISystem.Instance.StartPlayerTurn(this);
            }
        }


        if (state == BattleState.Waiting || state == BattleState.Acting)
        {
            if (team == Team.Player)
            {
                healthbar.SetActive(false);
            }
            progress += speed * Time.deltaTime;


            if (progress >= 100 && !actionStarted)
            {
                progress = 100;
                actionStarted = true;


                if (team == Team.Player)
                {
                    healthbar.SetActive(false);
                    CombatUISystem.Instance.ExecutePlayerAction(this);
                }
                else
                {
                    CombatUISystem.Instance.ExecuteEnemyAction(this);
                }
            }
        }
    }



    public void UpdateIcon(Vector3 start, Vector3 end)
    {
        if (icon == null)
            return;

        icon.rectTransform.position =
            Vector3.Lerp(start, end, progress / 100f);
    }

    public void FinishTurn()
    {
        progress = 0;
        actionStarted = false;
        state = BattleState.Waiting;
    }
}