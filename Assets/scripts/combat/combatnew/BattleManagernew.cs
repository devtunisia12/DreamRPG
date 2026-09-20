using UnityEngine;
using System.Collections.Generic;

public class BattleManagernew : MonoBehaviour
{
    public static BattleManagernew Instance;
    public Transform playerSpawn;
    public Transform enemySpawn;

    public Transform iconParent;
    public Transform startPoint;
    public Transform endPoint;


    List<BattleCharacter> players = new();
    List<BattleCharacter> enemies = new();

    public void RegisterPlayer(BattleCharacter player)
    {
        if (!players.Contains(player))
        {
            players.Add(player);
            player.CreateIcon(iconParent);
        }
    }

    public void RegisterEnemy(BattleCharacter enemy)
    {
        if (!enemies.Contains(enemy))
        {
            enemies.Add(enemy);
            enemy.CreateIcon(iconParent);
        }
    }

    void Awake()
    {
        Instance = this;
    }

    public void ClearBattle()
    {
        players.Clear();
        enemies.Clear();
    }



    void Update()
    {
        foreach (var p in players)
        {
            p.UpdateIcon(
                startPoint.position,
                endPoint.position
            );
        }


        foreach (var e in enemies)
        {
            e.UpdateIcon(
                startPoint.position,
                endPoint.position
            );
        }
    }
}