using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UseStar : MonoBehaviour {
    public Text TotalStarPowers;
    private Button Button;
    public TransportLayer TransportLayer;
    public FireBase FireBase;
    //public static bool StarPowerEnabled = false;

    void Start()
    {
        Button = GetComponent<Button>();
    }

    void Update()
    {
        CheckButton();
        TotalStarPowers.text = InitGame.TotalStarPowers.ToString();
    }


    public void OnButtonClick()
    {


        if (InitGame.TotalStarPowers >= 1 && SwitchStanceButton.DefensiveStanceEnabled == true)
        {
            InitGame.TotalStarPowers--;
            InitGame.StarPowerEnabled = 1;
            CancelInvoke();
            Invoke("ResetStarPower", 5);

        }
        else if (InitGame.TotalStarPowers >= 1 && SwitchStanceButton.OffensiveStanceEnabled == true)
        {
            InitGame.TotalStarPowers--;
            if (FireBase.OnlineGame && FireBase.isHost)
            {
                FireBase.HostPowersRef.Push().SetValueAsync("star");
            }
            else if (FireBase.OnlineGame && !FireBase.isHost)
            {
                FireBase.ClientPowersRef.Push().SetValueAsync("star");
            }
            else
            {
                TransportLayer.SendMyMessage("enemyusedstar");
            }
            
        }
    }
    public void EnableStarPower()
    {
        InitGame.StarPowerEnabled = 2;
        CancelInvoke();
        Invoke("ResetStarPower", 3.5f);
    }

    void ResetStarPower()
    {
       InitGame.StarPowerEnabled = 0;

    }


    void CheckButton()
    {
        if (InitGame.TotalStarPowers == 0 ) 
        {
            Button.interactable = false;
        }
        else 
        {
            Button.interactable = true;
        }

    }
}
