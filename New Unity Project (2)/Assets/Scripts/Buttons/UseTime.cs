using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UseTime : MonoBehaviour {
    public Text TotalTimePowersText;
    private Button Button;
    public TransportLayer TransportLayer;
    public FireBase FireBase;

    void Start () {
        Button = GetComponent<Button>();
	}
	
	void Update () {
        CheckButton();
        TotalTimePowersText.text = InitGame.TotalTimePowers.ToString();
	}

    public void OnButtonClick()
    {
        if (InitGame.TotalTimePowers >= 1 && SwitchStanceButton.OffensiveStanceEnabled == true)
        {
            InitGame.TotalTimePowers--;
            
            if (FireBase.OnlineGame && FireBase.isHost)
            {
                FireBase.HostPowersRef.Push().SetValueAsync("time");
            }
            else if (FireBase.OnlineGame && !FireBase.isHost)
            {
                FireBase.ClientPowersRef.Push().SetValueAsync("time");
            }
            else
            {
                TransportLayer.SendMyMessage("enemyusedtime");
            }
        }
        else if (InitGame.TotalTimePowers >= 1 && SwitchStanceButton.DefensiveStanceEnabled == true)
        {
            InitGame.TotalTimePowers--;
            DecreaseFallSpeed();

        }
    }

    public void DecreaseFallSpeed()
    {
        InitGame.TimePowerEnabled = 2;
        CancelInvoke();
        Invoke("ResetFallSpeed2", 5);
    }

    public void IncreaseFallSpeed()
    {
        InitGame.TimePowerEnabled = 1;
        CancelInvoke();
        Invoke("ResetFallSpeed", 5);
        
    }
    public void ResetFallSpeed2() //reset called from defensive stance
    {
        InitGame.TimePowerEnabled = 0;
        InitGame.CrystalSpawnInterval = 0.35f; // only used for decreasefallspeed()
    }


    public void ResetFallSpeed()//reset called from offensive stance
    {
        InitGame.TimePowerEnabled = 0;
        InitGame.CrystalSpawnInterval = 0.35f; // only used for decreasefallspeed()
    }

    public void CheckButton()
    {
        if (InitGame.TotalTimePowers == 0)
        {
            Button.interactable = false;
        }
        else
        {
            Button.interactable = true;
        }

    }
}
