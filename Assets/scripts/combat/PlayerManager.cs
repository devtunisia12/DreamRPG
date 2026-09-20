using UnityEngine;
using System.Collections.Generic;

public class PlayerManager : MonoBehaviour
{
    public List<GameObject> players = new List<GameObject>();

    public List<Transform> playerPositions = new List<Transform>();

    public List<GameObject> playerPrefabs = new List<GameObject>();

    public List<GameObject> enemies = new List<GameObject>();

    public List<Transform> enemiesPositions = new List<Transform>();

    public List<GameObject> enemiesPrefabs = new List<GameObject>();

    public GameObject playerPrefab;

    private int doitoncePlayer = 0;

    void PlaceEnemies()
    {
        int amount = Mathf.Min(enemiesPrefabs.Count, enemiesPositions.Count);

        for (int i = 0; i < amount; i++)
        {
            GameObject enemy = Instantiate(
                enemiesPrefabs[i],
                enemiesPositions[i].position,
                Quaternion.identity
            );
            BattleCharacter battle = enemy.GetComponent<BattleCharacter>();

            if (battle != null)
            {
                BattleManagernew.Instance.RegisterEnemy(battle);
            }

            AddEnemy(enemy);
        }
    }
    void PlacePlayers()
    {
        int amount = Mathf.Min(playerPrefabs.Count, playerPositions.Count);

        for (int i = 0; i < amount; i++)
        {
            GameObject player = Instantiate(
                playerPrefabs[i],
                playerPositions[i].position,
                Quaternion.identity
            );
            BattleCharacter battle = player.GetComponent<BattleCharacter>();

            if (battle != null)
            {
                BattleManagernew.Instance.RegisterPlayer(battle);
            }

            AddPlayer(player);
        }
    }

    public void ClearBattleObjects()
    {
        Debug.Log("player removed");
        for (int i = players.Count - 1; i >= 0; i--)
        {
            if (players[i] != null)
                Destroy(players[i]);
        }

        for (int i = enemies.Count - 1; i >= 0; i--)
        {
            if (enemies[i] != null)
                Destroy(enemies[i]);
        }

        players.Clear();
        enemies.Clear();
    }

    public void StartBattle()
    {
        BattleManagernew.Instance.ClearBattle();

        foreach (GameObject player in players)
        {
            if (player != null)
            {
                Destroy(player);
            }
        }
        if (doitoncePlayer == 0)
        {
            playerPrefabs.Add(playerPrefab);
        }
        doitoncePlayer += 1;

        int amount = Mathf.Min(
            playerPrefabs.Count,
            playerPositions.Count
        );

        for (int i = 0; i < amount; i++)
        {
            GameObject player = Instantiate(
                playerPrefabs[i],
                playerPositions[i].position,
                Quaternion.identity
            );

            BattleCharacter battle = player.GetComponent<BattleCharacter>();

            if (battle != null)
            {
                BattleManagernew.Instance.RegisterPlayer(battle);
            }

            players.Add(player);
        }
    }

    public void AddPlayerToParty(GameObject playerPrefabnpc)
    {
        if (!playerPrefabs.Contains(playerPrefabnpc))
        {
            playerPrefabs.Add(playerPrefabnpc);

            Debug.Log(
                playerPrefabnpc.name + " joined the party!"
            );
        }
    }


    void AddPlayer(GameObject player)
    {
        if (!players.Contains(player))
        {
            players.Add(player);
        }
    }

    public void ClearBattle()
    {
        players.Clear();
        enemies.Clear();
    }

    public void AddEnemy(GameObject enemy)
    {
        if (!enemies.Contains(enemy))
        {
            enemies.Add(enemy);
        }
    }

    public void RespawnPlayers()
    {
        players.Clear();

        int amount = Mathf.Min(
            playerPrefabs.Count,
            playerPositions.Count
        );

        for (int i = 0; i < amount; i++)
        {
            GameObject player = Instantiate(
                playerPrefabs[i],
                playerPositions[i].position,
                Quaternion.identity
            );

            players.Add(player);
        }
    }


}