using UnityEngine;

public class zonespeak : MonoBehaviour
{
    public GameObject speakchatbubble;
    public speakwith npc; // Reference to this NPC

    private bool canspeak = false;

    void Start()
    {
        speakchatbubble.SetActive(false);
    }

    void Update()
    {
        if (canspeak && Input.GetMouseButtonDown(0))
        {
            canspeak = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            speakchatbubble.SetActive(true);
            canspeak = true;

            npc.canChat = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            speakchatbubble.SetActive(false);
            canspeak = false;

            npc.canChat = false;
        }
    }
}