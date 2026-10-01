using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class itemmenusystem : MonoBehaviour
{
    [Header("Selector")]
    public RectTransform selector;
    public float[] selectorY = { 100, 40, -20,-60 };
    public float[] selectorX = { 100, 40, -20, -60 };

    private int menuChoice = 0;

    public static int potionC, reviveC, ManapotC, BjinC, keyC, OrbC, BookC, InkC;
    public AudioSource audioSelect;

    public TextMeshProUGUI PotionCTXT,revTXT,ManapotTXT,bjinTXT,KeyTXT,OrbTXT,BookTXT,InkTXT;


    void Update()
    {
        PotionCTXT.text = "*"+potionC.ToString();
        revTXT.text = "*" + reviveC.ToString();
        ManapotTXT.text = "*" + ManapotC.ToString();
        bjinTXT.text = "*" + BjinC.ToString();
        KeyTXT.text = "*" + keyC.ToString();
        OrbTXT.text = "*" + OrbC.ToString();
        BookTXT.text = "*" + BookC.ToString();
        InkTXT.text = "*" + InkC.ToString();

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            menuChoice--;

            if (menuChoice < 0)
                menuChoice = 7;

            ShowPage();
        }


        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            menuChoice++;

            if (menuChoice > 7)
                menuChoice = 0;

            ShowPage();
        }
    }

    void ShowPage()
    {
        audioSelect.Play();
        Vector3 pos = selector.localPosition;
        pos.y = selectorY[menuChoice];
        pos.x = selectorX[menuChoice];
        selector.localPosition = pos;
    }
}
