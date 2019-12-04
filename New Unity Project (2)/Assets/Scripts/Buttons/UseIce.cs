using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UseIce : MonoBehaviour {
    public Text TotalIcePowersText;
    private Button Button;
    public InitGame InitGame;
    public float timeLeft = 3.0f; //ice power duration
    public TransportLayer TransportLayer;
    public FireBase FireBase;

    void Start()
    {
        Button = GetComponent<Button>();
        InitGame = GameObject.Find("Main Camera").GetComponent<InitGame>();
    }

    void Update()
    {
        CheckButton();
        TotalIcePowersText.text = InitGame.TotalIcePowers.ToString();

        //reset ice power
        if (InitGame.IcePowerEnabled == 1)
        {
            InitGame.CancelInvoke();
            timeLeft -= Time.deltaTime;
            if (timeLeft <= 0)
            {
               
                InitGame.IcePowerEnabled = 0;
                timeLeft = 3.0f; // set timeLeft again to 3 so it can be ready for the next time


                CrystalHealth[] crystals = FindObjectsOfType<CrystalHealth>();
                foreach (CrystalHealth crystal in crystals)
                {
                    if (crystal.tag != "FireCrystal")
                    {
                        crystal.tag = "Crystal";
                    }
                        
                }

                InitGame.Startz();

            }
            
        }
    }


    public void OnButtonClick()
    {

        if (InitGame.TotalIcePowers >= 1 && SwitchStanceButton.DefensiveStanceEnabled == true)
        {
            timeLeft = 3.0f; // Fixes the bug: if u press ice wait 1 second, then press ice again, it goes again from 3seconds and not waste one
            InitGame.TotalIcePowers--;

            CrystalHealth[] crystals = FindObjectsOfType<CrystalHealth>();

            foreach (CrystalHealth crystal in crystals)
            {
                if (crystal.tag != "FireCrystal") // fire crystals must remain firecrystals
                {
                    crystal.tag = "FrozenCrystal";
                }
               
            }
            InitGame.IcePowerEnabled = 1;
        }
        else if(InitGame.TotalIcePowers >= 1 && SwitchStanceButton.OffensiveStanceEnabled == true)
        {
            InitGame.TotalIcePowers--;
            if (FireBase.OnlineGame && FireBase.isHost)
            {
                FireBase.HostPowersRef.Push().SetValueAsync("ice");
            }
            else if (FireBase.OnlineGame && !FireBase.isHost)
            {
                FireBase.ClientPowersRef.Push().SetValueAsync("ice");
            }
            else
            {
                TransportLayer.SendMyMessage("enemyusedice");
            }

        }       
    }

    public void SpawnIceCrystals()
    {
        SpawnPower.CrystalHP = 1;
        Invoke("ResetCrystalHP", 5);       
    }

    public void ResetCrystalHP()
    {
        SpawnPower.CrystalHP = 0;
    }

    public void CheckButton()
    {
        if (InitGame.TotalIcePowers == 0 )
        {
            Button.interactable = false;
        }
        else
        {
            Button.interactable = true;
        }
    }
}
