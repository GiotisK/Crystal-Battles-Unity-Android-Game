using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwitchStanceButton : MonoBehaviour {

    private Sprite AttackSprite;
    public Sprite DefenseSprite;
    private Sprite CurrentSprite;
    public Image ButtonImage;

    public static bool DefensiveStanceEnabled = false;
    public static bool OffensiveStanceEnabled = true;

    private void Start()
    {
        AttackSprite = Resources.Load("Swords", typeof(Sprite)) as Sprite;
        DefenseSprite = Resources.Load("Shield", typeof(Sprite)) as Sprite;
        ButtonImage = GetComponent<Image>();
    }
    public void OnButtonClick()
    {
        if (PracticeMode.PracticeModeEnabled)
        {
            ButtonImage.sprite = DefenseSprite;
            DefensiveStanceEnabled = true;
            OffensiveStanceEnabled = false;
        }
        else/*If multiplayer*/
        {
            CurrentSprite = ButtonImage.sprite;
            if (CurrentSprite == AttackSprite)
            {

                ButtonImage.sprite = DefenseSprite;
                DefensiveStanceEnabled = true;
                OffensiveStanceEnabled = false;

            }
            else if (CurrentSprite == DefenseSprite)
            {
                ButtonImage.sprite = AttackSprite;
                OffensiveStanceEnabled = true;
                DefensiveStanceEnabled = false;
            }
        }

    }
}
