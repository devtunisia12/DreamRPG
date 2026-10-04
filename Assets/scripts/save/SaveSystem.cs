using UnityEngine;
using Unity.Cinemachine;
using System.Collections;
using System.Collections.Generic;

public class SaveSystem : MonoBehaviour
{
    public Transform player;
    public CinemachineConfiner2D confiner;
    public Collider2D newBoundsd;
    public movement move;
    public List<GameObject> playersPos2 = new List<GameObject>();
    public List<GameObject> playerPrefabs2 = new List<GameObject>();
    public List<GameObject> allFriendPrefabs = new List<GameObject>();
    public PlayerManager playerManager;

    void Start()
    {
        if (mainmenusystem.isLoaded == true)
        {
            LoadGame();
            mainmenusystem.isLoaded = false;
        }
    }

    public void AddPlayerToParty2(GameObject playerPrefabnpc)
    {
        if (!playerPrefabs2.Contains(playerPrefabnpc))
        {
            playerPrefabs2.Add(playerPrefabnpc);

            Debug.Log(
                playerPrefabnpc.name + "  Saved joined the party!"
            );
        }
    }

    public void SaveGame()
    {
        // Player position
        PlayerPrefs.SetFloat("PlayerX", player.position.x);
        PlayerPrefs.SetFloat("PlayerY", player.position.y);

        // Movement items
        PlayerPrefs.SetInt("healthmedkit", movement.healthmedkit);
        PlayerPrefs.SetInt("powerItem", movement.powerItem);
        PlayerPrefs.SetInt("NewDefenseItem", movement.NewDefenseItem);
        PlayerPrefs.SetInt("NewWeapon", movement.NewWeapon);

        // Menu items
        PlayerPrefs.SetInt("potionC", itemmenusystem.potionC);
        PlayerPrefs.SetInt("reviveC", itemmenusystem.reviveC);
        PlayerPrefs.SetInt("ManapotC", itemmenusystem.ManapotC);
        PlayerPrefs.SetInt("BjinC", itemmenusystem.BjinC);
        PlayerPrefs.SetInt("keyC", itemmenusystem.keyC);
        PlayerPrefs.SetInt("OrbC", itemmenusystem.OrbC);
        PlayerPrefs.SetInt("BookC", itemmenusystem.BookC);
        PlayerPrefs.SetInt("InkC", itemmenusystem.InkC);

        SaveFriends();

        //Cinemachine Stuff
        PlayerPrefs.SetString("Bounds", newBoundsd.gameObject.name);
        SaveBattleParty();
        //Party Friend Stuff
        PlayerPrefs.Save();

        Debug.Log("Game Saved!");
    }

    void SaveBattleParty()
    {
        Debug.Log("BEFORE SAVE - playerPrefabs2 Count = " + playerPrefabs2.Count);

        PlayerPrefs.SetInt("BattlePartyCount", playerPrefabs2.Count);

        for (int i = 0; i < playerPrefabs2.Count; i++)
        {
            if (playerPrefabs2[i] == null)
                continue;

            PlayerPrefs.SetString(
                "BattleParty_" + i,
                playerPrefabs2[i].name
            );

            Debug.Log(
                "Saved BattleParty_" + i +
                " = " + playerPrefabs2[i].name
            );
        }
    }


    void SaveFriends()
    {
        PlayerPrefs.SetInt("FriendCount", playersPos2.Count);

        for (int i = 0; i < playersPos2.Count; i++)
        {
            GameObject friend = playersPos2[i];

            if (friend == null)
                continue;

            friendfollow follow = friend.GetComponent<friendfollow>();

            // Save Friend name
            PlayerPrefs.SetString(
                "Friend_" + i + "_Name",
                friend.name
            );

            // Save position
            PlayerPrefs.SetFloat(
                "Friend_" + i + "_X",
                friend.transform.position.x
            );

            PlayerPrefs.SetFloat(
                "Friend_" + i + "_Y",
                friend.transform.position.y
            );

            // Save following state
            if (follow != null)
            {
                PlayerPrefs.SetInt(
                    "Friend_" + i + "_Following",
                    follow.isFollowing ? 1 : 0
                );
            }
        }
    }




    void LoadFriends()
    {
        int friendCount = PlayerPrefs.GetInt("FriendCount", 0);

        for (int i = 0; i < friendCount; i++)
        {
            // Get saved friend name
            string friendName = PlayerPrefs.GetString(
                "Friend_" + i + "_Name",
                ""
            );

            if (string.IsNullOrEmpty(friendName))
                continue;

            // Find friend in scene
            GameObject friend = GameObject.Find(friendName);

            if (friend == null)
            {
                Debug.LogWarning(
                    "Friend not found in scene: " + friendName
                );

                continue;
            }

            // Add to lists
            if (!playersPos2.Contains(friend))
            {
                playersPos2.Add(friend);
            }

            if (!move.playersPos.Contains(friend))
            {
                move.playersPos.Add(friend);
            }

            // Load position
            float x = PlayerPrefs.GetFloat(
                "Friend_" + i + "_X"
            );

            float y = PlayerPrefs.GetFloat(
                "Friend_" + i + "_Y"
            );

            friend.transform.position = new Vector3(
                x,
                y,
                friend.transform.position.z
            );

            // Load following state
            friendfollow follow = friend.GetComponent<friendfollow>();

            if (follow != null)
            {
                int following = PlayerPrefs.GetInt(
                    "Friend_" + i + "_Following",
                    0
                );

                follow.isFollowing = following == 1;

                Debug.Log(
                    friend.name +
                    " loaded. Following = " +
                    follow.isFollowing
                );
            }
        }
    }


    void LoadBattleParty()
    {
        int partyCount = PlayerPrefs.GetInt("BattlePartyCount", 0);

        Debug.Log("AFTER LOAD - BattlePartyCount = " + partyCount);

        playerPrefabs2.Clear();

        for (int i = 0; i < partyCount; i++)
        {
            string prefabName = PlayerPrefs.GetString(
                "BattleParty_" + i,
                ""
            );

            Debug.Log(
                "Loading BattleParty_" + i +
                " = " + prefabName
            );

            if (string.IsNullOrEmpty(prefabName))
                continue;

            GameObject prefab = FindPrefabByName(prefabName);

            if (prefab != null)
            {
                playerPrefabs2.Add(prefab);

                if (!playerManager.playerPrefabs.Contains(prefab))
                {
                    playerManager.playerPrefabs.Add(prefab);
                }

                Debug.Log(
                    "RESTORED: " + prefab.name
                );
            }
        }

        Debug.Log(
            "FINAL playerPrefabs2 Count = " +
            playerPrefabs2.Count
        );
    }
    GameObject FindPrefabByName(string prefabName)
    {
        foreach (GameObject prefab in allFriendPrefabs)
        {
            if (prefab != null && prefab.name == prefabName)
            {
                return prefab;
            }
        }

        Debug.LogWarning("Battle prefab not found: " + prefabName);
        return null;
    }

    public void LoadGame()
    {
        if (!PlayerPrefs.HasKey("PlayerX"))
        {
            Debug.Log("No Save Found!");
            return;
        }

        // Player position
        float x = PlayerPrefs.GetFloat("PlayerX");
        float y = PlayerPrefs.GetFloat("PlayerY");

        player.position = new Vector3(x, y, player.position.z);

        // Movement items
        movement.healthmedkit = PlayerPrefs.GetInt("healthmedkit", 0);
        movement.powerItem = PlayerPrefs.GetInt("powerItem", 0);
        movement.NewDefenseItem = PlayerPrefs.GetInt("NewDefenseItem", 0);
        movement.NewWeapon = PlayerPrefs.GetInt("NewWeapon", 0);

        // Menu items
        itemmenusystem.potionC = PlayerPrefs.GetInt("potionC", 0);
        itemmenusystem.reviveC = PlayerPrefs.GetInt("reviveC", 0);
        itemmenusystem.ManapotC = PlayerPrefs.GetInt("ManapotC", 0);
        itemmenusystem.BjinC = PlayerPrefs.GetInt("BjinC", 0);
        itemmenusystem.keyC = PlayerPrefs.GetInt("keyC", 0);
        itemmenusystem.OrbC = PlayerPrefs.GetInt("OrbC", 0);
        itemmenusystem.BookC = PlayerPrefs.GetInt("BookC", 0);
        itemmenusystem.InkC = PlayerPrefs.GetInt("InkC", 0);

        //Cinemachine Stuff
        string savedBounds = PlayerPrefs.GetString("Bounds", "");

        GameObject boundsObject = GameObject.Find(savedBounds);

        if (boundsObject != null)
        {
            newBoundsd = boundsObject.GetComponent<Collider2D>();
            confiner.BoundingShape2D = newBoundsd;
        }

        //Party Friend Stuff
        LoadFriends();
        LoadBattleParty();

        Debug.Log("Game Loaded!");
    }
}