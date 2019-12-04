using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UserInterface : MonoBehaviour {
    public float timer = 0f;
    public int MinuteCounter = 0;


    public Text LivesText;
    public Text EnemyLivesText;

    public TransitionsHandler TransitionsHandler;

    public TextMeshProUGUI OpponentDisconnectedText;
    public TextMeshProUGUI YouLostText;
    public TextMeshProUGUI YouWonText;
    public TextMeshProUGUI CountDownText;
    public TextMeshProUGUI Timer;
    public TextMeshProUGUI ScoreText;

    public GameObject TimeRedCircle;
    public GameObject IceRedCircle;
    public GameObject FireRedCircle;
    public GameObject StarRedCircle;
    public GameObject TimeGreenCircle;
    public GameObject IceGreenCircle;
    public GameObject FireGreenCircle;
    public GameObject StarGreenCircle;



    public GameObject LoadingSprite;
    public GameObject CreateServerNameInputFieldObject;
    public GameObject JoinServerNameInputFieldObject;   
    public GameObject WaitingForPlayersOnlineText;
    public GameObject SearchingForGameOnlineText;
    public GameObject WaitingForPlayersLANText;
    public GameObject SearchingForGameLANText;
    public GameObject BackButtonOnlineCreateJoin;
    public GameObject PlayAgainButton;
    public GameObject MainGameObject;
    public GameObject MenuObject;
    public GameObject MyHealthBarIcon;
    public GameObject EnemyHealthBarIcon;
    public GameObject TryAgainButton;
    public GameObject PracticeBackToMenuButton;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (!InitGame.GameIsOver)
        {
            CirclesHandler();
        }
        
        ShowLives();
    }

    void ShowLives()
    {
        LivesText.text = InitGame.PlayerHP.ToString();
        EnemyLivesText.text = InitGame.EnemyHP.ToString();
    }

    public void DisableOtherTexts()
    {
        WaitingForPlayersLANText.SetActive(false);
        SearchingForGameLANText.SetActive(false);
    }

    public void ShowDisconnectScreen()
    {
        OpponentDisconnectedText.enabled = true;
        YouLostText.enabled = false;
        YouWonText.enabled = false;
        PlayAgainButton.SetActive(false);
    }

    public void EnablePowerButtons()
    {
        TransitionsHandler.PlayTransitions();
    }

    public void DisablePowerButtons()
    {
        TransitionsHandler.ResetTransitions();
    }

    public void DisableMenuAndEnableGameInterface()
    {
        MainGameObject.SetActive(true);
        MenuObject.SetActive(false);
    }

    public void DisableAllCircles()
    {
        TimeRedCircle.SetActive(false);
        IceRedCircle.SetActive(false);
        FireRedCircle.SetActive(false); 
        StarRedCircle.SetActive(false);
        TimeGreenCircle.SetActive(false);
        IceGreenCircle.SetActive(false);
        FireGreenCircle.SetActive(false);
        StarGreenCircle.SetActive(false);
    }
    public void CirclesHandler()
    {
        if (InitGame.TimePowerEnabled == 2)
        {
            TimeRedCircle.SetActive(false);
            TimeGreenCircle.SetActive(true);
        }else if (InitGame.TimePowerEnabled == 1)
        {
            TimeRedCircle.SetActive(true);
            TimeGreenCircle.SetActive(false);
        }else if (InitGame.TimePowerEnabled == 0)
        {
            TimeRedCircle.SetActive(false);
            TimeGreenCircle.SetActive(false);
        }

        if(InitGame.StarPowerEnabled == 2)
        {
            StarRedCircle.SetActive(true);
            StarGreenCircle.SetActive(false);
        }else if (InitGame.StarPowerEnabled == 1)
        {
            StarRedCircle.SetActive(false);
            StarGreenCircle.SetActive(true);

        }else if (InitGame.StarPowerEnabled == 0)
        {
            StarRedCircle.SetActive(false);
            StarGreenCircle.SetActive(false);
        }

        if (InitGame.IcePowerEnabled == 1){
            IceRedCircle.SetActive(false);
            IceGreenCircle.SetActive(true);
        }else if(InitGame.IcePowerEnabled == 0 && SpawnPower.CrystalHP == 1)
        {
            IceRedCircle.SetActive(true);
            IceGreenCircle.SetActive(false);
        }
        else
        {
            IceRedCircle.SetActive(false);
            IceGreenCircle.SetActive(false);
        }

    }

    public void ShowHideLoadingTexts(bool isHost)
    {
        if (isHost)
        {
            CreateServerNameInputFieldObject.SetActive(false);
            WaitingForPlayersOnlineText.SetActive(true);
        }
        else
        {
            JoinServerNameInputFieldObject.SetActive(false);
            SearchingForGameOnlineText.SetActive(true);
        }
        LoadingSprite.SetActive(true);
        if (!isHost)
        {
            BackButtonOnlineCreateJoin.SetActive(false);
        }
        
    }

    public void FillHealthBars()
    {
        EnemyHealthBarIcon.transform.GetChild(0).GetComponent<Image>().fillAmount = (float)InitGame.EnemyHP / 10;
        MyHealthBarIcon.transform.GetChild(0).GetComponent<Image>().fillAmount = (float)InitGame.PlayerHP / 10;
    }

    public void TimerUI()
    {
        if (PracticeMode.PracticeModeEnabled) {
            timer += Time.deltaTime;
            if (timer < 10)
            {
                Timer.text = (MinuteCounter + ":0" + ((int)timer).ToString());
            }
            else
            {
                Timer.text = (MinuteCounter + ":" + ((int)timer).ToString());
            }
            
            if (timer >= 60)
            {
                timer = 0;
                MinuteCounter++;
            }

        }
       
    }


}
