using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Unity.Editor;
using Firebase.Database;
using System.Text;
using UnityEngine.UI;



public class FireBase : MonoBehaviour
{

    private string Message;

    public bool PlayAgain = false;
    public string ServerName;
    public bool isHost;

    public static bool OnlineGame = false; 
    public static bool ServerDeleted = false;  

    public InputField CreateServerNameInputField;
    public InputField JoinServerNameInputField;  
    public Text HostServerNameText;
    public Text ClientServerNameText;
  
    public DatabaseReference reference;
    public DatabaseReference StartGameRef;
    public DatabaseReference HostLivesRef;
    public DatabaseReference ClientLivesRef;
    public DatabaseReference HostPowersRef;
    public DatabaseReference ClientPowersRef;
    public DatabaseReference HostMessagesRef;
    public DatabaseReference ClientMessagesRef;
    public DatabaseReference ClientPlayAgainRef;
    public DatabaseReference HostPlayAgainRef;
    public DatabaseReference ClientDisconnectedRef;
    public DatabaseReference HostDisconnectedRef;

    public UserInterface UserInterface;
    public InitGame InitGame;
    public UseFire UseFire;
    public UseIce UseIce;
    public UseTime UseTime;
    public UseStar UseStar;

    private void Update()
    {
        if (OnlineGame)//this if is maybe not needed. needs testing
        {
            MessagesHandler(Message);
        }
         
    }

    public void StartDB(bool isHost)
    {    
        this.isHost = isHost;

        UserInterface.ShowHideLoadingTexts(isHost);

        PlayAgain = false;
        ServerDeleted = false;
        
        Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            var dependencyStatus = task.Result;
            if (dependencyStatus == Firebase.DependencyStatus.Available)
            {
                // Set a flag here to indicate whether Firebase is ready to use by your app.
                FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("");
                reference = FirebaseDatabase.DefaultInstance.RootReference;
                OnlineGame = true;

                CheckIfGameNameExists();
            }
            else
            {
                UnityEngine.Debug.LogError(System.String.Format(
                  "Could not resolve all Firebase dependencies: {0}", dependencyStatus));
                // Firebase Unity SDK is not safe to use here.
            }
        });
    }



   public void InitDatabase()
   {
        //string GameKey = reference.Push().Key;
        StartGameRef = FirebaseDatabase.DefaultInstance.GetReference("/Games/" + ServerName + "/StartGame");
        HostLivesRef = FirebaseDatabase.DefaultInstance.GetReference("/Games/" + ServerName + "/HostLives");
        ClientLivesRef = FirebaseDatabase.DefaultInstance.GetReference("/Games/" + ServerName + "/ClientLives");
        HostPowersRef = FirebaseDatabase.DefaultInstance.GetReference("/Games/" + ServerName + "/HostPowers");
        ClientPowersRef = FirebaseDatabase.DefaultInstance.GetReference("/Games/" + ServerName + "/ClientPowers");
        HostMessagesRef = FirebaseDatabase.DefaultInstance.GetReference("/Games/" + ServerName + "/HostMessages");
        ClientMessagesRef = FirebaseDatabase.DefaultInstance.GetReference("/Games/" + ServerName + "/ClientMessages");
        ClientPlayAgainRef = FirebaseDatabase.DefaultInstance.GetReference("/Games/" + ServerName + "/ClientPlayAgain");
        HostPlayAgainRef = FirebaseDatabase.DefaultInstance.GetReference("/Games/" + ServerName + "/HostPlayAgain");
        ClientDisconnectedRef = FirebaseDatabase.DefaultInstance.GetReference("/Games/" + ServerName + "/ClientDisconnected");
        HostDisconnectedRef = FirebaseDatabase.DefaultInstance.GetReference("/Games/" + ServerName + "/HostDisconnected");

        /* Host only subscribes to 
         * the changes of the client
         */
        if (isHost)
        {
            StartGameRef.SetValueAsync("0");
            ClientLivesRef.SetValueAsync("10");
            HostLivesRef.SetValueAsync("10");
            HostMessagesRef.SetValueAsync("null");
            ClientMessagesRef.SetValueAsync("null");
            ClientPlayAgainRef.SetValueAsync("null");
            HostPlayAgainRef.SetValueAsync("null");
            HostDisconnectedRef.SetValueAsync("null");
            ClientDisconnectedRef.SetValueAsync("null");
            HostDisconnectedRef.OnDisconnect().SetValue("disconnected2");
            //FirebaseDatabase.DefaultInstance.GetReference("/Games/" + ServerName).OnDisconnect().RemoveValue();

            ClientLivesRef.ValueChanged += HandleClientLivesValueChanged;
            ClientPowersRef.ValueChanged += HandleClientPowersValueChanged;
            ClientMessagesRef.ValueChanged += HandleClientMessagesValueChanged;
            ClientPlayAgainRef.ValueChanged += HandleClientPlayAgainValueChanged;
            StartGameRef.ValueChanged += HandleStartGameValueChanged;
            ClientDisconnectedRef.ValueChanged += HandleClientDisconnectedValueChanged;

        }
        else /*if isClient*/
        {
            HostLivesRef.ValueChanged += HandleHostLivesValueChanged;
            HostPowersRef.ValueChanged += HandleHostPowersValueChanged;
            HostMessagesRef.ValueChanged += HandleHostMessagesValueChanged;
            HostPlayAgainRef.ValueChanged += HandleHostPlayAgainValueChanged;
            HostDisconnectedRef.ValueChanged += HandleHostDisconnectedValueChanged;
            StartGameRef.ValueChanged += HandleStartGameValueChanged;
            ClientDisconnectedRef.OnDisconnect().SetValue("disconnected2");
           // FirebaseDatabase.DefaultInstance.GetReference("/Games/" + ServerName).OnDisconnect().RemoveValue();
            StartGameRef.SetValueAsync("1");
        
        }
       /* HostLivesRef.GetValueAsync().ContinueWith(task => {
            if (task.IsFaulted)
            {
                // Handle the error...
            }
            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                string json = snapshot.GetRawJsonValue();
                ParseJson(json);
             
            }
        });*/
    }
    //=============================HANDLERS====================================================================================================
    void HandleHostDisconnectedValueChanged(object sender, ValueChangedEventArgs args)
    {
        if (args.DatabaseError != null)
        {
            Debug.LogError(args.DatabaseError.Message);
            return;
        }
        Message = args.Snapshot.GetRawJsonValue().TrimStart('"').TrimEnd('"');
    }
    void HandleClientDisconnectedValueChanged(object sender, ValueChangedEventArgs args)
    {
        if (args.DatabaseError != null)
        {
            Debug.LogError(args.DatabaseError.Message);
            return;
        }
        
        Message = args.Snapshot.GetRawJsonValue().TrimStart('"').TrimEnd('"');
    }


    void HandleClientPlayAgainValueChanged(object sender, ValueChangedEventArgs args)
    {
        if (args.DatabaseError != null)
        {
            Debug.LogError(args.DatabaseError.Message);
            return;
        }
        if(args.Snapshot.GetRawJsonValue().TrimStart('"').TrimEnd('"') != "null")
        {
            Message = args.Snapshot.GetRawJsonValue().TrimStart('"').TrimEnd('"');
        }
        
    }

    void HandleHostPlayAgainValueChanged(object sender, ValueChangedEventArgs args)
    {
        if (args.DatabaseError != null)
        {
            Debug.LogError(args.DatabaseError.Message);
            return;
        }
        if (args.Snapshot.GetRawJsonValue().TrimStart('"').TrimEnd('"') != "null")
        {
            Message = args.Snapshot.GetRawJsonValue().TrimStart('"').TrimEnd('"');
        }
    }

    void HandleStartGameValueChanged(object sender, ValueChangedEventArgs args)
    {
        if (args.DatabaseError != null)
        {
            Debug.LogError(args.DatabaseError.Message);
            return;
        }       
        Message = args.Snapshot.GetRawJsonValue().TrimStart('"').TrimEnd('"');
    }

    void HandleHostMessagesValueChanged(object sender, ValueChangedEventArgs args)
    {
        if (args.DatabaseError != null)
        {
            Debug.LogError(args.DatabaseError.Message);
            return;
        }
        Message = args.Snapshot.GetRawJsonValue().TrimStart('"').TrimEnd('"');
        //Debug.Log(Message);

    }

    void HandleClientMessagesValueChanged(object sender, ValueChangedEventArgs args)
    {
        if (args.DatabaseError != null)
        {
            Debug.LogError(args.DatabaseError.Message);
            return;
        }
        Message = args.Snapshot.GetRawJsonValue().TrimStart('"').TrimEnd('"');
        //Debug.Log(Message);

    }

    void HandleHostLivesValueChanged(object sender, ValueChangedEventArgs args)
    {
        if (args.DatabaseError != null)
        {
            Debug.LogError(args.DatabaseError.Message);
            return;
        }
        string clientHP = args.Snapshot.GetRawJsonValue().TrimStart('"').TrimEnd('"');
        int.TryParse(clientHP, out InitGame.EnemyHP);
        //Debug.Log("HostLives changed to " + clientHP);

    }

    void HandleClientLivesValueChanged(object sender, ValueChangedEventArgs args)
    {
        if (args.DatabaseError != null)
        {
            Debug.LogError(args.DatabaseError.Message);
            return;
        }

        string clientHP = args.Snapshot.GetRawJsonValue().TrimStart('"').TrimEnd('"'); 
        int.TryParse(clientHP,out InitGame.EnemyHP);
       // Debug.Log("ClientLives changed to " + clientHP);
    }

    void HandleHostPowersValueChanged(object sender, ValueChangedEventArgs args)
    {
        if (args.DatabaseError != null)
        {
            Debug.LogError(args.DatabaseError.Message);
            return;
        }
        //Debug.Log(args.Snapshot.GetRawJsonValue());
        string[] msg = ParseJson(args.Snapshot.GetRawJsonValue());
       // Debug.Log("power: " + msg[0] + " " + msg.Length);
        Message = msg[msg.Length-1];
    }

    void HandleClientPowersValueChanged(object sender, ValueChangedEventArgs args)
    {
        if (args.DatabaseError != null)
        {
            Debug.LogError(args.DatabaseError.Message);
            return;
        }
        //Debug.Log(args.Snapshot.GetRawJsonValue());
        string[] msg = ParseJson(args.Snapshot.GetRawJsonValue());
       // Debug.Log("power: " + msg[0] + " " + msg.Length);
        Message = msg[msg.Length - 1]; // forcing the PowersHandler() function to run through the main thread...Cant call it from here.
    }
//=========================================================================================================================================
    /*
     * Called whenever you put a new Game Name in the inputfield. 
     * Checks if an active game already exists in firebase, 
     * if it exists, try again.
     */
    public void CheckIfGameNameExists()
    {
        if (isHost)
        {
            ServerName = HostServerNameText.text;
            //HostServerNameText.text = " "; //reset the inputfield 
        }
        else if (!isHost)
        {
            ServerName = ClientServerNameText.text;
            //ClientServerNameText.text = " ";//reset the inputfield 
            //Debug.Log(ServerName);
        }

        /*If zombie Node,
         * remove it and
         * create a new Node
         * with same name
         */
        FirebaseDatabase.DefaultInstance.GetReference("/Games").GetValueAsync().ContinueWith(task =>
        {
            if (task.IsFaulted)
            {
                Debug.Log("doesnt exist");
            }
            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                if (snapshot.Child(ServerName).Exists && snapshot.Child(ServerName).ChildrenCount<=2 && ServerName!="" )
                {
                    FirebaseDatabase.DefaultInstance.GetReference("/Games/" + ServerName).RemoveValueAsync();
                    Invoke("CreateServerNode", 2.5f);
                }
                else
                {
                    CreateServerNode();
                }
            }
        });
    
    }

    public void CreateServerNode()
    {
        FirebaseDatabase.DefaultInstance.GetReference("/Games").GetValueAsync().ContinueWith(task => {
            if (task.IsFaulted)
            {
                Debug.Log("doesnt exist");
            }
            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                //string[] check = ParseJson(snapshot.Child(ServerName).Child("StartGame").GetRawJsonValue());
                //Debug.Log("check= "+check);
                //Debug.Log(snapshot.Child(ServerName).Child("StartGame").GetRawJsonValue());
                //string StartGameValue = snapshot.Child(ServerName).Child("StartGame").GetRawJsonValue().TrimStart('"').TrimEnd('"');
               // string StartGameValue = "1";
                long ServerChildrenCount = snapshot.Child(ServerName).ChildrenCount;
                string StartGameValue;
                if (ServerChildrenCount == 9)
                {
                   StartGameValue = snapshot.Child(ServerName).Child("StartGame").GetRawJsonValue().TrimStart('"').TrimEnd('"');
                }
                else{
                    StartGameValue = "-1";
                }
                Debug.Log(ServerChildrenCount);

                if (snapshot.HasChild(ServerName) && isHost)
                {
                    UserInterface.WaitingForPlayersOnlineText.SetActive(false);
                    UserInterface.LoadingSprite.SetActive(false);
                    UserInterface.CreateServerNameInputFieldObject.SetActive(true);

                    Debug.Log("This Server Name already exists... Try again");
                    SSTools.ShowMessage("Server already exists!", SSTools.Position.bottom, SSTools.Time.twoSecond);
                }
                else if (!snapshot.HasChild(ServerName) && isHost)
                {
                    Debug.Log("Server Created!");
                    SSTools.ShowMessage("Server created!", SSTools.Position.bottom, SSTools.Time.oneSecond);
                    InitDatabase();

                }
                else if (snapshot.HasChild(ServerName) && !isHost && ServerName != "" && StartGameValue == "0")
                {
                    InitDatabase();
                    //SSTools.ShowMessage("Joining Game...", SSTools.Position.bottom, SSTools.Time.oneSecond);
                    Debug.Log("Server exists...Joining game");
                }
                else if ((!snapshot.HasChild(ServerName) && !isHost) || ServerName == "" || StartGameValue=="1" || StartGameValue=="-1")

                {
                    UserInterface.LoadingSprite.SetActive(false);
                    UserInterface.SearchingForGameOnlineText.SetActive(false);
                    UserInterface.JoinServerNameInputFieldObject.SetActive(true);
                    UserInterface.BackButtonOnlineCreateJoin.SetActive(true);

                    Debug.Log("Server doesnt exist...Try another one");
                    if (StartGameValue == "1")
                    {
                        SSTools.ShowMessage("Server is already commited!", SSTools.Position.bottom, SSTools.Time.twoSecond);
                    }
                    else
                    {
                        SSTools.ShowMessage("Server doesnt exist!", SSTools.Position.bottom, SSTools.Time.twoSecond);
                    }
                    
                }
                ResetInputFields();

            }
        });
    }

    public string[] ParseJson(string json)
    {
        string[] messages;
        int i=0;
        json = json.TrimStart('{').TrimEnd('}');
        if (json.Contains(","))
        {
            string[] values = json.Split(','); // split every key-values string to different strings (example: "89hh878H":"hello" is one keyvalue)
            messages = new string[values.Length];
            foreach (string val in values)
            {
                messages[i] = val.Split(':')[1].TrimStart('"').TrimEnd('"');
                Debug.Log(messages[i]);
                i++;
            }
        }
        else
        {
            messages = new string[1];
            messages[0] = json.Split(':')[1].TrimStart('"').TrimEnd('"');
            string key = json.Split(':')[0].TrimStart('"').TrimEnd('"');
            //Debug.Log(key);
            //Debug.Log(messages[0]);
        }
        return messages;
    }

    
    /* This function is planned to be called
     * whenever a game ends. It deletes the game "node" that 
     * was created in the firebase
     */
    public void DisconnectFromDatabase()
    {
        
        FirebaseDatabase.DefaultInstance.GetReference("/Games/" + ServerName).RemoveValueAsync();
        ServerDeleted = true;
        
    }

    /*
     * This function cant be called
     * through an async function like the "handlers"
     * above. So the main thread must be forced
     * to run this function through the Update
     * function
     */
    public void MessagesHandler(string msg)
    {
        switch (msg)
        {
            case "1":
                ClientServerNameText.text = "";
                Message = "";
                HostServerNameText.text = "";            
                UserInterface.LoadingSprite.SetActive(false);
                UserInterface.WaitingForPlayersOnlineText.SetActive(false);
                UserInterface.SearchingForGameOnlineText.SetActive(false);

                UserInterface.DisableMenuAndEnableGameInterface();
                InitGame.CountDownEnabled = true;
                break;
            case "star":
                Message = "";
                UseStar.EnableStarPower();
                break;
            case "fire":
                Message = "";
                UseFire.EnableFirePower();
                break;

            case "ice":
                Message = "";
                UseIce.SpawnIceCrystals();
                break;

            case "time":
                Message = "";
                UseTime.IncreaseFallSpeed();
                break;

            case "clientloses":            
            case "hostloses":
                Message = "";
                InitGame.EndGame(false,1);
                ResetHostDatabaseValues();
                ResetClientDatabaseValues();
                
                break;
            case "playagain":
               /* if (isHost)
                {
                    ClientPlayAgainRef.SetValueAsync("null");
                }
                else
                {
                    HostPlayAgainRef.SetValueAsync("null");
                }*/
                if (PlayAgain)
                {
                    PlayAgain = false;
                    Message = "";                
                    InitGame.ResetValues();
                    InitGame.CountDownEnabled = true;
                }
                break;
            case "disconnected":
                Message = "";
                /*if (!isHost) //if client gets message that the host was disconnected , that means that the server is deleted
                {
                    ServerDeleted = true;
                }*/
                if (!ServerDeleted)
                {
                    ServerDeleted = true;
                    Debug.Log("Case:disconnected");
                    //Message = "";
                    UserInterface.ShowDisconnectScreen();            
                }
                
                break;
            case "disconnected2":
                Debug.Log("Case:disconnected2");
                Message = "";
                UserInterface.ShowDisconnectScreen();
                InitGame.EndGame(true, 999);
                break;
        }
    } 


    public void UnsubscribeFromHost()
    {
        HostLivesRef.ValueChanged -= HandleHostLivesValueChanged;
        HostPowersRef.ValueChanged -= HandleHostPowersValueChanged;
        HostMessagesRef.ValueChanged -= HandleHostMessagesValueChanged;
        HostPlayAgainRef.ValueChanged -= HandleHostPlayAgainValueChanged;
        HostDisconnectedRef.ValueChanged -= HandleHostDisconnectedValueChanged;
        StartGameRef.ValueChanged -= HandleStartGameValueChanged;
    }

    public void UnsubscribeFromClient()
    {
        ClientLivesRef.ValueChanged -= HandleClientLivesValueChanged;
        ClientPowersRef.ValueChanged -= HandleClientPowersValueChanged;
        ClientMessagesRef.ValueChanged -= HandleClientMessagesValueChanged;
        ClientPlayAgainRef.ValueChanged -= HandleClientPlayAgainValueChanged;
        StartGameRef.ValueChanged -= HandleStartGameValueChanged;
        ClientDisconnectedRef.ValueChanged -= HandleClientDisconnectedValueChanged;
    }

    public void ResetInputFields()
    {
        CreateServerNameInputField.text = "";
        JoinServerNameInputField.text = "";
    }
    
    void ResetHostDatabaseValues()
    {
        StartGameRef.SetValueAsync("0"); 
        HostLivesRef.SetValueAsync("10");
        HostMessagesRef.SetValueAsync("null");
        HostPlayAgainRef.SetValueAsync("null");
        HostDisconnectedRef.SetValueAsync("null");       
        FirebaseDatabase.DefaultInstance.GetReference("/Games/" + ServerName + "/HostPowers").RemoveValueAsync();
        
    }

    void ResetClientDatabaseValues()
    {

        ClientLivesRef.SetValueAsync("10");
        ClientMessagesRef.SetValueAsync("null");
        ClientPlayAgainRef.SetValueAsync("null");
        ClientDisconnectedRef.SetValueAsync("null");        
        FirebaseDatabase.DefaultInstance.GetReference("/Games/" + ServerName + "/ClientPowers").RemoveValueAsync();
    }
}