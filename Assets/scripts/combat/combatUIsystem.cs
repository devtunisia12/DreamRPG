using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
using Unity.Cinemachine;
using TMPro;

public class CombatUISystem : MonoBehaviour
{
    public RectTransform imageChoice;
    public RectTransform imageChoiceItem;
    public GameObject selectPlayerObj, selectEnemyObj;
    public GameObject imageChoiceObj;

    private int indexPlayers = 0;
    private int indexChoice = 0; 
    private int indexEnemies = 0;
    private int indexItem = 0;

    public PlayerManager playerManager;
    bool isPlayerSelected = false;
    bool isEnemySelected = false;
    bool isStartSelected = false;
    bool isItemSelected = false;
    bool isSelected = false;
    public bool isDefenseSelected;

    public GameObject winTextObj;
    public TextMeshProUGUI winText;

    //animators
    public Animator animUICombat;
    public GameObject UICombatObj;
    private BattleUnit currentPlayer;
    private BattleUnit currentEnemy;

    [Header("Targets")]
    public Transform playerobjt;
    public GameObject Camcombat, combatUI;
    [Header("Camera")]
    public CinemachineCamera cam;
    public CinemachineFollow follow;
    public static bool canireturn = false;
    public GameObject listItem;
    public GameObject healthkit, defendkit, weaponkit, powerkit;
    private int itemTargetPlayer;

    private int selectedPlayerIndex = 0;
    private BattleCharacter currentTurnCharacter;
    private BattleUnit lastAttackerPlayer;

    private int activeTurnPlayerIndex = -1;
    private bool waitingForPlayerTurn = false;

    //sprites selection menu
    public GameObject listmenu;
    public Image attobj, defobj, talkobj, itemobj, mercyobj;
    public Sprite spriteatt, spritedef, spritetalk, spriteitem, spritemercy;
    public Sprite spriteattS, spritedefS, spritetalkS, spriteitemS, spritemercyS;
    public static CombatUISystem Instance;
    public GameObject attackbar;
    public AttackTimingBar attackTimingBar;

    public GameObject ItemUI;
    public RectTransform ItemUI1;
    public RectTransform ItemUI2;
    public RectTransform ItemUI3;
    public RectTransform ItemUI4;

    private Vector3 itemSmallScale = Vector3.one * 0.8f;
    private Vector3 itemBigScale = Vector3.one * 1.15f;

    private float itemScaleSpeed = 10f;

    public changingmusic musicchanger;
    public GameObject combatsystemobj;
    public AudioSource audioSelect;
    public AudioSource audioConfirm;
    public CinemachineConfiner2D confiner;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        ItemUI.SetActive(false);
        selectPlayerObj.SetActive(false);
        selectEnemyObj.SetActive(false);
        attackbar.SetActive(false);
        isSelected = false;
        attackTimingBar = attackbar.GetComponent<AttackTimingBar>();
        listmenu.SetActive(false);
        attobj.sprite = spriteatt;
        defobj.sprite = spritedef;
        talkobj.sprite = spritetalk;
        itemobj.sprite = spriteitem;
        mercyobj.sprite = spritemercy;

        animUICombat = UICombatObj.GetComponent<Animator>();
        animUICombat.SetBool("start", false);
        winTextObj.SetActive(false);
        listItem.SetActive(false);
        winText.text = "";

    }

    void UpdateItemSelection()
    {
        RectTransform[] items =
        {
        ItemUI1,
        ItemUI2,
        ItemUI3,
        ItemUI4
    };

        for (int i = 0; i < items.Length; i++)
        {
            if (items[i] == null)
                continue;

            Vector3 targetScale;

            if (i == indexItem)
                targetScale = itemBigScale;
            else
                targetScale = itemSmallScale;

            items[i].localScale = Vector3.Lerp(
                items[i].localScale,
                targetScale,
                Time.deltaTime * itemScaleSpeed
            );
        }
    }

    public void CheckWin()
    {
        int aliveEnemies = playerManager.enemies.Count;
        int alivesPlayers = playerManager.players.Count;

        if (aliveEnemies == 0)
        {
            StartCoroutine(WinBattle("Players win"));
            return;
        }

        if (alivesPlayers == 0)
        {
            StartCoroutine(WinBattle("Enemies win"));
            return;
        }
        isEnemySelected = false;
        isPlayerSelected = false;
        isSelected = false;
        isItemSelected = false;
        isStartSelected = false;

        activeTurnPlayerIndex = -1;
        waitingForPlayerTurn = false;

        selectPlayerObj.SetActive(false);
        selectEnemyObj.SetActive(false);
        listmenu.SetActive(false);
        imageChoiceObj.SetActive(false);
    }

    public void StartPlayerTurn(BattleCharacter character)
    {
        if (character.team != Team.Player)
            return;

        currentTurnCharacter = character;

        activeTurnPlayerIndex =
            playerManager.players.IndexOf(character.gameObject);


        selectedPlayerIndex = activeTurnPlayerIndex;
        indexPlayers = activeTurnPlayerIndex;

        RectTransform menuRect = listmenu.GetComponent<RectTransform>();

        Vector3 screenPos = Camera.main.WorldToScreenPoint(
            character.transform.position
        );

        menuRect.position = screenPos;

        indexEnemies = 0;
        CheckEnemyIndex();

        isPlayerSelected = false;
        isEnemySelected = false;
        isStartSelected = false;
        isItemSelected = false;
        isDefenseSelected = false;
        isSelected = false;

        BattleCharacter.pauseTimeline = true;

        listmenu.SetActive(true);
        imageChoiceObj.SetActive(true);
        selectEnemyObj.SetActive(false);
        selectPlayerObj.SetActive(false);
        BattleCharacter.pauseTimeline = true;


        listmenu.SetActive(true);
        imageChoiceObj.SetActive(true);


        character.state = BattleCharacter.BattleState.Ready;
    }

    void StartAttack()
    {
        CheckPlayerIndex();
        CheckEnemyIndex();
        currentPlayer =
        playerManager.players[activeTurnPlayerIndex]
        .GetComponent<BattleUnit>();

        currentEnemy =
        playerManager.enemies[indexEnemies]
        .GetComponent<BattleUnit>();
    }

    void CheckEnemyIndex()
    {
        if (playerManager.enemies.Count == 0)
            return;

        int aliveCount = 0;

        foreach (GameObject enemy in playerManager.enemies)
        {
            if (enemy.activeSelf)
                aliveCount++;
        }


        if (aliveCount == 0)
        {
            CheckWin();
            return;
        }


        if (!playerManager.enemies[indexEnemies].activeSelf)
        {
            for (int i = 0; i < playerManager.enemies.Count; i++)
            {
                if (playerManager.enemies[i].activeSelf)
                {
                    indexEnemies = i;
                    break;
                }
            }
        }
    }

    IEnumerator EnemyTurn()
    {
        foreach (GameObject enemyObj in playerManager.enemies)
        {
            if (!enemyObj.activeSelf)
                continue;


            BattleUnit enemy = enemyObj.GetComponent<BattleUnit>();


            if (lastAttackerPlayer == null)
                yield break;


            if (!lastAttackerPlayer.gameObject.activeSelf)
                yield break;


            yield return StartCoroutine(
                  enemy.AttackPlayer(lastAttackerPlayer, false)
            );


            yield return new WaitForSeconds(0.5f);
        }
    }

    void UseItem(int item)
    {
        switch (item)
        {
            case 0: // Health
                if (movement.healthmedkit > 0)
                {
                    movement.healthmedkit--;


                    BattleUnit player = currentTurnCharacter.GetComponent<BattleUnit>();

                    player.currentHP = player.maxHP;

                    if (player.currentHP > player.maxHP)
                        player.currentHP = player.maxHP;
                }
                break;

            case 1: // Power
                if (movement.powerItem > 0)
                {
                    movement.powerItem--;

                    BattleUnit player = currentTurnCharacter.GetComponent<BattleUnit>();

                    player.currentHP = player.maxHP;

                    if (player.currentHP > player.maxHP)
                        player.currentHP = player.maxHP;
                }
                break;

            case 2: // Defense
                if (movement.NewDefenseItem > 0)
                {
                    movement.NewDefenseItem--;

                    BattleUnit player = currentTurnCharacter.GetComponent<BattleUnit>();

                    player.currentHP = player.maxHP;

                    if (player.currentHP > player.maxHP)
                        player.currentHP = player.maxHP;
                }
                break;

            case 3: // Weapon
                if (movement.NewWeapon > 0)
                {
                    movement.NewWeapon--;

                    BattleUnit player = currentTurnCharacter.GetComponent<BattleUnit>();

                    player.currentHP = player.maxHP;

                    if (player.currentHP > player.maxHP)
                        player.currentHP = player.maxHP;
                }
                break;
        }

        ItemUI.SetActive(false);
        isSelected = false;
        imageChoiceObj.SetActive(true);
        isItemSelected = false;
        isPlayerSelected = false;
    }

    bool HasAnyItem()
    {
        return movement.healthmedkit > 0 ||
               movement.powerItem > 0 ||
               movement.NewDefenseItem > 0 ||
               movement.NewWeapon > 0;
    }

    void Update()
    {

        if (indexChoice == 0)
        {
            attobj.sprite = spriteattS;
            defobj.sprite = spritedef;
            talkobj.sprite = spritetalk;
            itemobj.sprite = spriteitem;
            mercyobj.sprite = spritemercy;
        }

        if (indexChoice == 2)
        {
            attobj.sprite = spriteatt;
            defobj.sprite = spritedefS;
            talkobj.sprite = spritetalk;
            itemobj.sprite = spriteitem;
            mercyobj.sprite = spritemercy;
        }

        if (indexChoice == 3)
        {
            attobj.sprite = spriteatt;
            defobj.sprite = spritedef;
            talkobj.sprite = spritetalk;
            itemobj.sprite = spriteitemS;
            mercyobj.sprite = spritemercy;
        }

        if (indexChoice == 4)
        {
            attobj.sprite = spriteatt;
            defobj.sprite = spritedef;
            talkobj.sprite = spritetalkS;
            itemobj.sprite = spriteitem;
            mercyobj.sprite = spritemercy;
        }

        if (indexChoice == 1)
        {
            attobj.sprite = spriteatt;
            defobj.sprite = spritedef;
            talkobj.sprite = spritetalk;
            itemobj.sprite = spriteitem;
            mercyobj.sprite = spritemercyS;
        }

        UpdateItemSelection();

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            CancelSelection();
        }

        if (movement.healthmedkit == 1)
        {
            healthkit.SetActive(true);
        }
        else
        {
            healthkit.SetActive(false);
        }
        if (movement.powerItem == 1)
        {
            powerkit.SetActive(true);
        }
        else
        {
            powerkit.SetActive(false);
        }
        if (movement.NewDefenseItem == 1)
        {
            defendkit.SetActive(true);
        }
        else
        {
            defendkit.SetActive(false);
        }
        if (movement.NewWeapon == 1)
        {
            weaponkit.SetActive(true);
        }
        else
        {
            weaponkit.SetActive(false);
        }

        if (isEnemySelected)
        {
            if (playerManager.enemies.Count == 0)
            {
                isEnemySelected = false;
                isSelected = false;
                selectEnemyObj.SetActive(false);

                return;
            }

            CheckEnemyIndex();

            if (indexEnemies >= 0 &&
                indexEnemies < playerManager.enemies.Count &&
                playerManager.enemies[indexEnemies] != null)
            {
                selectEnemyObj.transform.position =
                    playerManager.enemies[indexEnemies].transform.position;
            }
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            audioConfirm.Play();
            if (!isPlayerSelected && !isEnemySelected && !isItemSelected)
            {
                isSelected = true;
                if (indexChoice == 0) // Attack
                {
                    isPlayerSelected = true;
                    currentTurnCharacter.selectedAction = BattleCharacter.PlayerAction.Attack;
                    selectPlayerObj.SetActive(true);
                }
                else if (indexChoice == 2) // Defense
                {
                    isDefenseSelected = true;
                    currentTurnCharacter.selectedAction = BattleCharacter.PlayerAction.Defense;
                    isPlayerSelected = true;
                    selectPlayerObj.SetActive(true);
                }
                else if (indexChoice == 3) // Item
                {
                    if (HasAnyItem())
                    {
                        imageChoiceObj.SetActive(false);
                        isItemSelected = true;
                        ItemUI.SetActive(true);

                    }
                    else
                    {
                        indexChoice = 0;
                    }
                }
                else if (indexChoice == 1) // Mercy / Escape
                {
                    isStartSelected = true;

                    BattleCharacter player =
                        playerManager.players[activeTurnPlayerIndex]
                        .GetComponent<BattleCharacter>();

                    player.progress = 0f;
                    player.state = BattleCharacter.BattleState.Waiting;

                    listmenu.SetActive(false);
                    imageChoiceObj.SetActive(false);
                    selectPlayerObj.SetActive(false);
                    selectEnemyObj.SetActive(false);

                    BattleCharacter.pauseTimeline = false;

                    isPlayerSelected = false;
                    isEnemySelected = false;
                    isStartSelected = false;
                    isDefenseSelected = false;

                    activeTurnPlayerIndex = -1;
                    waitingForPlayerTurn = false;
                }
            }
            else if (isPlayerSelected && !isEnemySelected && !isItemSelected)
            {
                if (indexChoice == 0 || indexChoice == 2)
                {
                    selectedPlayerIndex = activeTurnPlayerIndex;

                    currentTurnCharacter.selectedEnemyIndex = indexEnemies;

                    lastAttackerPlayer =
                    playerManager.players[activeTurnPlayerIndex]
                    .GetComponent<BattleUnit>();
                    isPlayerSelected = false;
                    isEnemySelected = true;
                    selectPlayerObj.SetActive(false);
                    selectEnemyObj.SetActive(true);
                }

                if (indexChoice == 3)
                {
                    itemTargetPlayer = indexPlayers;

                    UseItem(indexItem);
                }
            }
            else if (isEnemySelected)
            {
                BattleCharacter player = currentTurnCharacter;

                if (player == null)
                    return;

                player.state = BattleCharacter.BattleState.Acting;

                BattleCharacter.pauseTimeline = false;

                listmenu.SetActive(false);
                selectEnemyObj.SetActive(false);

                isStartSelected = true;
            }
            else if (isItemSelected)
            {
                UseItem(indexItem);
            }
        }


        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (isEnemySelected)
            {
                indexEnemies--;
            }
            else if (isPlayerSelected && !isItemSelected)
            {
                if (activeTurnPlayerIndex == -1)
                    indexPlayers--;
            }
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (isEnemySelected)
            {
                indexEnemies++;
            }
            else if (isPlayerSelected && !isItemSelected)
            {
                if (activeTurnPlayerIndex == -1)
                    indexPlayers++;
            }
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow) && isItemSelected)
        {
            audioSelect.Play();
            indexItem--;
        }

        if (Input.GetKeyDown(KeyCode.RightArrow) && isItemSelected)
        {
            audioSelect.Play();
            indexItem++;
        }


        if (indexItem >= 4)
            indexItem = 0;

        if (indexItem < 0)
            indexItem = 3;

        if (Input.GetKeyDown(KeyCode.UpArrow) && isSelected == false)
        {
            audioSelect.Play();
            indexChoice--;
        }

        if (Input.GetKeyDown(KeyCode.DownArrow) && isSelected == false)
        {
            audioSelect.Play();
            indexChoice++;
        }

        if (indexPlayers >= playerManager.players.Count)
            indexPlayers = 0;

        if (indexPlayers < 0)
            indexPlayers = playerManager.players.Count - 1;

        if (indexEnemies >= playerManager.enemies.Count)
            indexEnemies = 0;

        if (indexEnemies < 0)
            indexEnemies = playerManager.enemies.Count - 1;

        if (playerManager.players.Count > 0)
        {
            if (isPlayerSelected)
            {
                CheckPlayerIndex();

                selectPlayerObj.transform.position =
                playerManager.players[indexPlayers].transform.position;
            }
        }

        if (playerManager.enemies.Count > 0)
        {
            if (indexEnemies >= 0 &&
                indexEnemies < playerManager.enemies.Count &&
                playerManager.enemies[indexEnemies] != null)
            {
                selectEnemyObj.transform.position =
                    playerManager.enemies[indexEnemies].transform.position;
            }
        }

        if (indexChoice > 5)
            indexChoice = 0;
        if (indexChoice < 0)
            indexChoice = 5;
    }
    public void ExecutePlayerAction(BattleCharacter character)
    {
        currentTurnCharacter = character;

        activeTurnPlayerIndex =
            playerManager.players.IndexOf(character.gameObject);

        if (character.selectedEnemyIndex >= 0)
        {
            indexEnemies = character.selectedEnemyIndex;
        }

        CheckPlayerIndex();
        CheckEnemyIndex();

        currentPlayer =
            playerManager.players[activeTurnPlayerIndex]
            .GetComponent<BattleUnit>();

        currentEnemy =
            playerManager.enemies[indexEnemies]
            .GetComponent<BattleUnit>();

        StartCoroutine(StartReaction());
    }

    public void ExecuteEnemyAction(BattleCharacter enemy)
    {
        StartCoroutine(EnemyAttack(enemy));
    }

    IEnumerator EnemyAttack(BattleCharacter enemy)
    {
        BattleUnit enemyUnit = enemy.GetComponent<BattleUnit>();

        if (playerManager.players.Count > 0)
        {
            BattleUnit target =
            playerManager.players[Random.Range(0, playerManager.players.Count)]
            .GetComponent<BattleUnit>();


            yield return StartCoroutine(
                  enemyUnit.AttackPlayer(target, false)
            );
        }


        enemy.FinishTurn();
    }

    void CheckPlayerIndex()
    {
        if (playerManager.players.Count == 0)
            return;


        if (!playerManager.players[indexPlayers].activeSelf)
        {
            for (int i = 0; i < playerManager.players.Count; i++)
            {
                if (playerManager.players[i].activeSelf)
                {
                    indexPlayers = i;
                    break;
                }
            }
        }
    }

    void CancelSelection()
    {
        isSelected = false;

        if (isEnemySelected)
        {
            isEnemySelected = false;
            isPlayerSelected = true;

            selectEnemyObj.SetActive(false);
            selectPlayerObj.SetActive(true);

            isStartSelected = false;
        }

        else if (isPlayerSelected)
        {
            isPlayerSelected = false;

            selectPlayerObj.SetActive(false);

            indexChoice = 0;
        }

        else if (isItemSelected)
        {
            isItemSelected = false;
            ItemUI.SetActive(false);
            indexChoice = 0;
        }
    }

    IEnumerator StartReaction()
    {
        animUICombat.SetBool("start", true);
        BattleCharacter.pauseTimeline = true;

        attackbar.SetActive(true);
        selectPlayerObj.SetActive(false);
        selectEnemyObj.SetActive(false);
        listmenu.SetActive(false);

        if (currentTurnCharacter.selectedAction == BattleCharacter.PlayerAction.Defense)
        {
            yield return StartCoroutine(
                currentEnemy.AttackPlayer(currentPlayer, true)
            );
        }
        else if (currentTurnCharacter.selectedAction == BattleCharacter.PlayerAction.Attack)
        {
            yield return StartCoroutine(
                currentPlayer.AttackEnemy(currentEnemy)
            );
        }

        animUICombat.SetBool("start", false);

        isPlayerSelected = false;
        isEnemySelected = false;
        isStartSelected = false;
        isSelected = false;

        BattleCharacter.pauseTimeline = false;

        currentTurnCharacter.selectedAction =
            BattleCharacter.PlayerAction.None;

        currentTurnCharacter.selectedEnemyIndex = -1;

        currentTurnCharacter.FinishTurn();

        currentTurnCharacter = null;
        activeTurnPlayerIndex = -1;
        waitingForPlayerTurn = false;
    }



    IEnumerator WinBattle(string WhoWinner)
    {
        isEnemySelected = false;
        isPlayerSelected = false;
        isSelected = false;
        isItemSelected = false;
        isStartSelected = false;

        activeTurnPlayerIndex = -1;
        waitingForPlayerTurn = false;

        selectPlayerObj.SetActive(false);
        selectEnemyObj.SetActive(false);
        listmenu.SetActive(false);
        imageChoiceObj.SetActive(false);

        yield return new WaitForSeconds(1f);

        winTextObj.SetActive(true);
        winText.text = WhoWinner;

        yield return new WaitForSeconds(1f);
        playerManager.ClearBattleObjects();

        musicchanger.ChangeBattleToMusic();
        confiner.enabled = true;
        movement.endedwar = false;
        movement.isCombat = false;
        winTextObj.SetActive(false);

        cam.Follow = playerobjt;

        movement.endedwar = true;
        movement.isCombat = false;

        Camcombat.SetActive(false);
        combatsystemobj.SetActive(false);

        
    }

}