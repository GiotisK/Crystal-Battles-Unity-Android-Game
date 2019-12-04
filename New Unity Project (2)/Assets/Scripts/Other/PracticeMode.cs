using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PracticeMode : MonoBehaviour {
    public static bool PracticeModeEnabled = false;
    private float PowerSpawnInterval;
    public UserInterface UserInterface;
    public InitGame InitGame;
    public SwitchStanceButton SwitchStanceButton;
    public TransitionsHandler TransitionsHandler;
    public UseFire UseFire;
    public UseIce UseIce;
    public UseStar UseStar;
    public UseTime UseTime;

	// Use this for initialization
	void Start () {
        PracticeModeEnabled = false;
    }
	
	// Update is called once per frame
	void Update () {
        if (PracticeModeEnabled)
        {
            SpawnRandomPowers();
        }
    }

    private void FixedUpdate()
    {
        if (PracticeModeEnabled && !InitGame.CountDownEnabled) //start after countdown is finished
        {
            UserInterface.TimerUI();
        }

    }


    public void OnButtonClick() {       
        StartPracticeMode();            
    }


    public void StartPracticeMode()/*On Practice button click*/
    {
        UserInterface.ScoreText.enabled = false;
        UserInterface.Timer.enabled = true;
        PowerSpawnInterval = 8.0f;
        
        UserInterface.PracticeBackToMenuButton.SetActive(false);
        UserInterface.TryAgainButton.SetActive(false);
        PracticeModeEnabled = true;
        InitGame.ResetValues();
        UserInterface.DisableMenuAndEnableGameInterface();
        InitGame.CountDownEnabled = true;
        InitGame.StartGameButton.GetComponent<Button>().onClick.Invoke();
        TransitionsHandler.EnemyHp.SetActive(false);
        TransitionsHandler.EnemyHpBar.SetActive(false);
        Invoke("SetDefensiveStanceOnly", 2f);
    }

    public void SetDefensiveStanceOnly()
    {
        SwitchStanceButton.ButtonImage.sprite = SwitchStanceButton.DefenseSprite;
        SwitchStanceButton.DefensiveStanceEnabled = true;
        SwitchStanceButton.OffensiveStanceEnabled = false;
    }

    public void ExitPracticeMode()
    {       
        InitGame.ResetValues();
        TransitionsHandler.EnemyHp.SetActive(true);
        TransitionsHandler.EnemyHpBar.SetActive(true);
        UserInterface.ScoreText.enabled = false;
        UserInterface.TryAgainButton.SetActive(false);
        UserInterface.PracticeBackToMenuButton.SetActive(false);
        UserInterface.MainGameObject.SetActive(false);
        UserInterface.MenuObject.SetActive(true);
    }

    public void SpawnRandomPowers()
    {      
        PowerSpawnInterval -= Time.deltaTime;
        if (PowerSpawnInterval <= 0)
        {
            PowerSpawnInterval = 5.0f;
            int RandomNum = Random.Range(0, 4);
            switch (RandomNum)
            {
                case (0):
                    UseFire.EnableFirePower();
                    break;
                case (1):
                    UseIce.SpawnIceCrystals();
                    break;
                case (2):
                    UseStar.EnableStarPower();
                    break;
                case (3):
                    UseTime.IncreaseFallSpeed();
                    break;
                case (4):
                    break;
                case (5):
                    break;
                case (6):
                    break;
            }
        }
    }

    public void SaveScore()
    {
        
        string[] currentscore = (UserInterface.Timer.text).Split(':');
        if (PlayerPrefs.GetString("score") == "")
        {
            PlayerPrefs.SetString("score", "0:0");
        }
        string[] savedscore = PlayerPrefs.GetString("score").Split(':');
        if (int.Parse(currentscore[0]) == int.Parse(savedscore[0])){
            if (int.Parse(currentscore[1]) >  int.Parse(savedscore[1]))
            {
                PlayerPrefs.SetString("score", UserInterface.Timer.text);
            }
        }else if (int.Parse(currentscore[0]) > int.Parse(savedscore[0])){
            PlayerPrefs.SetString("score", UserInterface.Timer.text);
        }

    }
}
