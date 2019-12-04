using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InitGame : MonoBehaviour {

    public static int TotalObjectsDestroyed = 0;
    public static int PlayerHP = 10;
    public static int EnemyHP = 10;
    public static int TimePowerEnabled = 0;
    public static int IcePowerEnabled = 0;
    public static int StarPowerEnabled = 0;
    public static int TotalIcePowers = 1;
    public static int TotalFirePowers = 1;
    public static int TotalStarPowers = 1;
    public static int TotalTimePowers = 1;
    public static bool CountDownEnabled = false;
    public static float timeLeft = 3.0f;
    public static int PreviousPosition = 999;
    public static int PreviousPosition2 = 999;
    public static float CrystalSpawnInterval = 0.35f;
    public static bool RunOnlyOnce = true; //handles the "sending infinite "enemylost" messages bug;
    public static bool GameIsOver = false; /*handles the ice power error. because ice power calls initgame.start() 
                                             after 3 sec, and if game is over in the middle of these 3 sec,it spawns again*/

    private int SpawnPositionX;
    private int HpCheck = 10;

    public UserInterface UserInterface;
    public TransitionsHandler TransitionsHandler;
    public FireBase FireBase;
    public TransportLayer TransportLayer;
    public PracticeMode PracticeMode;

    public GameObject BackButtonFirst;
    public GameObject BackButtonSecond;
    public GameObject BackButtonZero;
    public GameObject BackButtonOnline;
    public GameObject BackButtonCreateJoin;
    public GameObject MainGameObject;
    public GameObject MenuObject;
    public GameObject StartGameButton;
    public GameObject PlayAgainButton;
    public GameObject BackToMenuButton;
    public GameObject TimePowerButton;
    public GameObject FirePowerButton;
    public GameObject IcePowerButton;
    public GameObject StarPowerButton;
    public GameObject StanceButton;

    public void Startz() {
        if (!GameIsOver) // this if handles the ice power error explained above ^
        {
            InvokeRepeating("CreateCrystal", 0, CrystalSpawnInterval); //create crystal every 0.5 seconds
        }

    }

    private void Update()
    {
        CheckHP();
    }

    private void FixedUpdate()
    {
        StartAfterCountDown();
    }


    void CreateCrystal()
    {

        SpawnPositionX = Random.Range(-2, 3); //get random X position
        while (SpawnPositionX == PreviousPosition || SpawnPositionX == SpawnPower.PowerPosition)
        {
            SpawnPositionX = Random.Range(-2, 3);
        }
        //hold the last 2 crystal spawn locations so the powers wont spawn on crystals
        PreviousPosition2 = PreviousPosition;
        PreviousPosition = SpawnPositionX;

        if (SpawnPower.CrystalHP == 0) //if icepower disabled spawn normal crystal
        {
            Instantiate(Resources.Load("NormalCrystal", typeof(GameObject)), new Vector2(SpawnPositionX, 5.5f), Quaternion.Euler(0, 0, 0));
        }
        else if (SpawnPower.CrystalHP == 1) //if icepower enabled spawn icecrystal
        {
            Instantiate(Resources.Load("IceCrystal", typeof(GameObject)), new Vector2(SpawnPositionX, 5.5f), Quaternion.Euler(0, 0, 0));
        }
    }

    public void CheckHP()
    {
        if (PlayerHP != HpCheck && !PracticeMode.PracticeModeEnabled)// Checks if the playerhp has changed, to prevent sending infinite messages 
        {
            HpCheck = PlayerHP;
            if (FireBase.OnlineGame && FireBase.isHost)
            {
                FireBase.HostLivesRef.SetValueAsync(PlayerHP.ToString());
            } else if (FireBase.OnlineGame && !FireBase.isHost)
            {
                FireBase.ClientLivesRef.SetValueAsync(PlayerHP.ToString());
            }
            else
            {
                try
                {
                    TransportLayer.SendMyMessage(PlayerHP.ToString());
                }
                catch(System.Exception e)
                {
                    Debug.Log("Error caught! :"+e);
                }
                
            }

        }

        UserInterface.FillHealthBars();

        if (PlayerHP == 0 && RunOnlyOnce == true && !PracticeMode.PracticeModeEnabled)
        {
            RunOnlyOnce = false;

            if (FireBase.OnlineGame && FireBase.isHost)
            {
                FireBase.HostMessagesRef.SetValueAsync("hostloses");

            } else if (FireBase.OnlineGame && !FireBase.isHost)
            {
                FireBase.ClientMessagesRef.SetValueAsync("clientloses");
            }
            else
            {
                TransportLayer.SendMyMessage("enemylost"); //meaning that the one who sends it lost 
            }

            // YouLostText.enabled = true; //mono edw disable kai oxi stin endgame giati i endgame kaleitai kai otan kerdizeisS        
            EndGame(false, 2);
        }
        else if(PlayerHP == 0 && RunOnlyOnce == true && PracticeMode.PracticeModeEnabled)
        {
            RunOnlyOnce = false;
            PracticeEnd();
        }
    }

    public void PracticeEnd()
    {
        /*needs manually reset of powers
         * because circle animations are still being played
         * ^^^^^^^STILL BUGGED^^^^^^^^^
         */
        PracticeMode.SaveScore();
        PracticeMode.PracticeModeEnabled = false;
        UserInterface.DisableAllCircles();
        GameIsOver = true;       
        CancelInvoke();
        DestroyAllCrystals();      
        UserInterface.DisablePowerButtons();
        UserInterface.Timer.enabled = false;
        UserInterface.ScoreText.text = "Best Time<size=50></size>\n<size=150>" + PlayerPrefs.GetString("score");
        UserInterface.Timer.text = "";
        UserInterface.timer = 0f;
        UserInterface.MinuteCounter = 0;
        UserInterface.ScoreText.enabled = true;
        UserInterface.TryAgainButton.SetActive(true);
        UserInterface.PracticeBackToMenuButton.SetActive(true);
    }
    /*
     * win = 1 => WIN
     * win = 2 => LOSE
     * win = 3 or w/e => irrelevant
     */
    public void EndGame(bool PlayerDced, int win) // otan teleiwsoun oi zwes, destroy ola ta kuvakia
    {
        UserInterface.DisableAllCircles();
        GameIsOver = true;
        if (win == 1)
        {
            UserInterface.YouWonText.enabled = true;
        }
        else if (win == 2)
        {
            UserInterface.YouLostText.enabled = true;
        }

        if (!PlayerDced) //i endgame kaleitai kai otan faei dc kapoios. epomenos otan faei dc kapoios de prepei na uparxei to koubi Play Again
        {
            PlayAgainButton.SetActive(true);
            BackToMenuButton.SetActive(true);
            PlayAgainButton.GetComponent<Button>().interactable = false;
            BackToMenuButton.GetComponent<Button>().interactable = false;
            CancelInvoke();
            Invoke("InvokedEnablePlayAgainAndBackButtons", 3f);
        }
        else{
            BackToMenuButton.SetActive(true);
            CancelInvoke();
            BackToMenuButton.GetComponent<Button>().interactable = true;
        }

        DestroyAllCrystals();
        UserInterface.DisablePowerButtons();
    }

    public void InvokedEnablePlayAgainAndBackButtons()
    {
        PlayAgainButton.GetComponent<Button>().interactable = true;
        BackToMenuButton.GetComponent<Button>().interactable = true;
    }

    public void DestroyAllCrystals() {

        AddForce[] crystals = FindObjectsOfType<AddForce>();
        foreach (AddForce crystal in crystals)
        {
            Destroy(crystal.gameObject);
        }

    }

    public void ResetGame() { // otan patisw to koubi arxizei to game pali apo tin arxi
        ResetValues();
        TransportLayer.HostEnablesClientGame(true);
        InitGame.CountDownEnabled = true;
    }

    public void PlayAgainOnButtonClick()
    {
        PlayAgainButton.SetActive(false);

        if (FireBase.OnlineGame)
        {
            FireBase.PlayAgain = true;
            if (FireBase.isHost)
            {
                FireBase.HostPlayAgainRef.SetValueAsync("playagain");
            }
            else
            {
                FireBase.ClientPlayAgainRef.SetValueAsync("playagain");
            }

        }
        else /*if local game*/
        {
            if (TransportLayer.isClient)
            {
                TransportLayer.SendMyMessage("playagain");
            }
            else/*if host*/
            {
                TransportLayer.HostWantsToPlayAgain = true;
            }
        }
    }

    public void OnBackToMenuClick()
    {

        ResetValues();
        if (FireBase.OnlineGame)
        {

            FireBase.OnlineGame = false;
            if (FireBase.isHost)
            {
                FireBase.HostDisconnectedRef.OnDisconnect().Cancel();
                FireBase.HostDisconnectedRef.SetValueAsync("disconnected");
                FireBase.UnsubscribeFromClient();
                Invoke("DisconnectFromDatabase", 2f);
                //FireBase.DisconnectFromDatabase();
                
            }
            else
            {
                FireBase.ClientDisconnectedRef.OnDisconnect().Cancel();
                FireBase.UnsubscribeFromHost();
                if (!FireBase.ServerDeleted)
                {                   
                    FireBase.ClientDisconnectedRef.SetValueAsync("disconnected");                  
                    Invoke("InvokedDisconnectFromDatabase", 2f);
                    //FireBase.DisconnectFromDatabase();
                }

            }
            UserInterface.OpponentDisconnectedText.enabled = false;
            BackButtonCreateJoin.GetComponent<Button>().onClick.Invoke();
            BackButtonOnline.GetComponent<Button>().onClick.Invoke();
            BackButtonZero.GetComponent<Button>().onClick.Invoke();

        }
        else
        {
            BackButtonFirst.GetComponent<Button>().onClick.Invoke();
            BackButtonSecond.GetComponent<Button>().onClick.Invoke();
            BackButtonZero.GetComponent<Button>().onClick.Invoke();
        }
    }
    public void InvokedDisconnectFromDatabase(){
        FireBase.DisconnectFromDatabase();
    }

    public void ResetValues()
    {
        BackToMenuButton.SetActive(false);
        UserInterface.YouLostText.enabled = false;
        UserInterface.YouWonText.enabled = false;
        TotalObjectsDestroyed = 0;
        PlayerHP = 10;
        EnemyHP = 10;
        TimePowerEnabled = 0;
        IcePowerEnabled = 0;
        StarPowerEnabled = 0;
        TotalIcePowers = 1;
        TotalFirePowers = 1;
        TotalStarPowers = 1;
        TotalTimePowers = 1;
        RunOnlyOnce = true;
        GameIsOver = false;
        TransportLayer.RestartGame = true;
        timeLeft = 3.0f;
        UserInterface.CountDownText.enabled = true;
        UserInterface.CountDownText.fontSize = 95;

    }
    public void StartAfterCountDown()
    {
        if (CountDownEnabled)
        {
            timeLeft -= Time.deltaTime;
            if (timeLeft <= 1.5)
            {
                UserInterface.EnablePowerButtons();
            }
            UserInterface.CountDownText.fontSize += 2;
            UserInterface.CountDownText.text = (timeLeft).ToString("0");
            if (timeLeft <= 1)
            {

                UserInterface.CountDownText.enabled = false;

                if (TransportLayer.RestartGame == false )
                {
                    StartGameButton.GetComponent<Button>().onClick.Invoke();
                }
                Startz();
                CountDownEnabled = false;
            }
        }
    }

}
