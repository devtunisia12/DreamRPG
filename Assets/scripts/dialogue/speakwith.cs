using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class speakwith : MonoBehaviour
{
    public bool canChat = false;

    public GameObject canvas;
    public TextMeshProUGUI textComponent;
    public string[] lines;
    public float textSpeed;

    private bool chatStarted = false;
    private int index;
    public friendfollow friendFollowScript;
    public bool isFriendFollowing = false;
    public Animator characteranim;
    public GameObject characterobj;

    public Sprite[] spritesIcon;
    public Image icon;
    public GameObject warriorPrefab;
    public PlayerManager combatme;

    void Start()
    {
        textComponent.text = string.Empty;
        characteranim= characterobj.GetComponent<Animator>();
        characteranim.SetBool("talk", false);

    }

    void Update()
    {
        if (canChat)
        {
            if (Input.GetKeyDown(KeyCode.F) && !chatStarted)
            {
                canvas.SetActive(true);
                StartDialogue();
                GetComponent<BoxCollider2D>().enabled = false;
                chatStarted = true;
            }
        }

        if (Input.GetMouseButtonDown(0) && chatStarted)
        {
            if (textComponent.text == lines[index])
            {
                characteranim.SetBool("talk", true);
                NextLine();
            }
            else
            {
                characteranim.SetBool("talk", false);
                StopAllCoroutines();
                textComponent.text = lines[index];
            }
        }
    }


    // 2D Trigger
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            canChat = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            canChat = false;
        }
    }


    void StartDialogue()
    {
        characteranim.SetBool("talk", true);
        index = 0;
        textComponent.text = "";
        StartCoroutine(TypeLine());
    }


    IEnumerator TypeLine()
    {
        icon.sprite = spritesIcon[index];
        foreach (char c in lines[index].ToCharArray())
        {
            textComponent.text += c;
            yield return new WaitForSeconds(textSpeed);
        }
        characteranim.SetBool("talk", false);
    }


    void NextLine()
    {
        if (index < lines.Length - 1)
        {
            index++;
            textComponent.text = "";
            StartCoroutine(TypeLine());
        }
        else
        {
            canvas.SetActive(false);
            chatStarted = false;
            index = 0;
            textComponent.text = "";
            if (isFriendFollowing == true)
            {
                combatme.AddPlayerToParty(warriorPrefab);
                friendFollowScript.isFollowing = true;
            }
            characteranim.SetBool("talk", false);
            GetComponent<BoxCollider2D>().enabled = true;
        }
    }
}