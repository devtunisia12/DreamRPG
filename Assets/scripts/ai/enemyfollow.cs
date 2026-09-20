using UnityEngine;
using System.Collections.Generic;

public class enemyfollow : MonoBehaviour
{
    public Transform player;
    public GameObject playerObject;

    public Transform[] paths;
    public float speed = 3f;

    private SpriteRenderer spriteRenderer;
    public bool isFollowing = false;
    public Animator animatorpla;
    public int EnemiesNumbers = 0;
    private int currentPath = 0;
    public PlayerManager playermanager;
    public List<GameObject> enemiesPrefabs = new List<GameObject>();

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animatorpla = GetComponent<Animator>();

        player = playerObject.transform;
    }

    void Update()
    {
        if (isFollowing)
        {
            Vector2 direction =
                (player.position - transform.position).normalized;

            transform.position +=
                (Vector3)direction * speed * Time.deltaTime;

            Flip(direction);
        }
        else
        {
            if (paths.Length == 0)
                return;

            Transform targetPath = paths[currentPath];

            float distance = Vector2.Distance(
                transform.position,
                targetPath.position
            );

            if (distance > 0.1f)
            {
                Vector2 direction =
                    (targetPath.position - transform.position).normalized;

                transform.position +=
                    (Vector3)direction * speed * Time.deltaTime;

                Flip(direction);
            }
            else
            {
                currentPath++;

                if (currentPath >= paths.Length)
                {
                    currentPath = 0;
                }
            }
        }
    }

    void Flip(Vector2 direction)
    {
        if (direction.x > 0)
            spriteRenderer.flipX = true;
        else if (direction.x < 0)
            spriteRenderer.flipX = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject == playerObject)
        {
            isFollowing = true;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject != playerObject)
            return;
        playermanager.StartBattle();

        int amount = Mathf.Min(
            EnemiesNumbers,
            playermanager.enemiesPositions.Count
        );

        for (int i = 0; i < amount; i++)
        {
            GameObject enemyPrefab =
                enemiesPrefabs[Random.Range(0, enemiesPrefabs.Count)];

            Transform spawnPoint = playermanager.enemiesPositions[i];

            GameObject enemy = Instantiate(
                enemyPrefab,
                spawnPoint.position,
                Quaternion.identity
            );

            BattleCharacter battle = enemy.GetComponent<BattleCharacter>();

            BattleManagernew.Instance.RegisterEnemy(battle);

            playermanager.AddEnemy(enemy);
        }
        gameObject.SetActive(false);

    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject == playerObject)
        {
            isFollowing = false;
        }
    }
}