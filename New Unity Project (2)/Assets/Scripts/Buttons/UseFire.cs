using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UseFire : MonoBehaviour {
    public Text TotalFirePowersText;
    private Button Button;
    public static int FireCrystalCounter = 0;
    public InitGame InitGame;
    public TransportLayer TransportLayer;
    public ParticleSystem IceSmash;
    public FireBase FireBase;
    public SoundManager SoundManager;

    void Start () {
        Button = GetComponent<Button>();
        InitGame = GameObject.Find("Main Camera").GetComponent<InitGame>();
    }
	
	void Update () {
        CheckButton();
        TotalFirePowersText.text = InitGame.TotalFirePowers.ToString();
    }

    public void OnButtonClick()
    {
        if (InitGame.TotalFirePowers >= 1 && SwitchStanceButton.DefensiveStanceEnabled == true)
        {
            InitGame.TotalFirePowers--;
            BurnFrozenCrystals();
        }else if (InitGame.TotalFirePowers >= 1 && SwitchStanceButton.OffensiveStanceEnabled == true)
        {
            InitGame.TotalFirePowers--;

            if (FireBase.OnlineGame && FireBase.isHost)
            {
                FireBase.HostPowersRef.Push().SetValueAsync("fire");
            }
            else if (FireBase.OnlineGame && !FireBase.isHost)
            {
                FireBase.ClientPowersRef.Push().SetValueAsync("fire");
            }
            else
            {
                TransportLayer.SendMyMessage("enemyusedfire");
            }
            
        }
        
    }


    public void BurnFrozenCrystals()
    {
        int counter = 0;
        CrystalHealth[] crystals = FindObjectsOfType<CrystalHealth>();

        foreach (CrystalHealth crystal in crystals)
        {
            if(crystal.name.Contains("IceCrystal") && crystal.tag!="FireCrystal")
            {
                SoundManager.PlaySmashSound();
                PlayParticles(counter,crystal.transform.position);
                Destroy(crystal.gameObject);
                counter++;
            }
        }
        if(counter == crystals.Length && InitGame.IcePowerEnabled == 1) // an oi crystals pou kahkan htan oloi icecrystals, simainei oti i start stin initgame de tha kalestei pote opote elegxoume
        {
            InitGame.Startz(); // pianei tin akraia periptwsi pou otan kaneis freeze olous tous icecrystals kai destroy me fire, stamatane na spawnaroun crystals
        }
       
    }

    public void PlayParticles(int counter,Vector3 position) // plays one different particle system per each ice crystal
    {
        GameObject.Find("IceCrystalParticle" + counter.ToString()).GetComponent<ParticleSystem>().transform.position = position;
        GameObject.Find("IceCrystalParticle" + counter.ToString()).GetComponent<ParticleSystem>().Play();     
    }





    public void EnableFirePower()
    {       

        CrystalHealth[] crystals = FindObjectsOfType<CrystalHealth>();

        foreach (CrystalHealth crystal in crystals)
        {
            FireCrystalCounter++;
            crystal.CrystalHP = 2;
            crystal.GetComponent<Animator>().runtimeAnimatorController = Resources.Load("FlameAnimator") as RuntimeAnimatorController; 
            crystal.tag = "FireCrystal";
        }      
    }


    public void CheckButton()
    {
        if (InitGame.TotalFirePowers == 0)
        {
            Button.interactable = false;
        }
        else
        {
            Button.interactable = true;
        }
    }
}
