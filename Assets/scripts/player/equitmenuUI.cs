using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class equitmenuUI : MonoBehaviour
{
    public PlayerManager playermang;
    public AudioSource audioSelect;
    public TextMeshProUGUI namechar, nameweapon, namearmor, nameaccessory, atktxt, deftxt, Manatxt, stafftxt, bigstafftxt, scarftxt;
    public Image Icon;
    private int menuChoice = 0;

    void Start()
    {
        ShowPage();
    }

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            menuChoice--;

            if (menuChoice < 0)
                menuChoice = playermang.playerPrefabs.Count - 1;

            ShowPage();
        }


        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            menuChoice++;

            if (menuChoice > playermang.playerPrefabs.Count)
                menuChoice = 0;

            ShowPage();
        }
    }

    void ShowPage()
    {
        GameObject player;

        if (menuChoice == 0)
        {
            player = playermang.playerPrefab;
        }
        else
        {
            player = playermang.playerPrefabs[menuChoice - 1];
        }

        BattleUnit battleUnit = player.GetComponent<BattleUnit>();

        namechar.text = battleUnit.nametx;
        nameweapon.text = battleUnit.Stafftx;
        namearmor.text = battleUnit.armortx;
        nameaccessory.text = battleUnit.accesstx;

        atktxt.text = "Atk: " + battleUnit.attacktx.ToString() + " ->";
        deftxt.text = "Def: " + battleUnit.deftx.ToString() + " ->";
        Manatxt.text = "Mana: " + battleUnit.manatx.ToString() + " ->";
        stafftxt.text = "Staff: " + battleUnit.bigstafftx.ToString();
        bigstafftxt.text = "BigStaff: " + battleUnit.bigstafftx.ToString();
        scarftxt.text = "Scarf: " + battleUnit.scarftx.ToString();
        Icon.sprite = battleUnit.iconplayer;
    }
}
