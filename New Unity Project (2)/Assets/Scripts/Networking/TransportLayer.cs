using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine.UI;
using TMPro;

public class TransportLayer : MonoBehaviour {
    private int myReliableChannelId;
    private int socketID;
    private int connectionID=-1;

    public bool ClientWantsToPlayAgain = false;
    public bool HostWantsToPlayAgain = false;
    public bool isClient;
    public bool RestartGame;

    public Networking net;
    public GameObject StartGameButton;
    public GameObject PlayAgainButton;
    public UserInterface UserInterface;
    public UseTime UseTime;
    public UseIce UseIce;
    public UseFire UseFire;
    public UseStar UseStar;
    public InitGame InitGame;

    void Update () {

        CheckForNewGame();

        int recHostId;
        int recConnectionId;
        int recChannelId;
        byte[] recBuffer = new byte[1024];
        int bufferSize = 1024;
        int dataSize;
        byte error;
        NetworkEventType recData = NetworkTransport.Receive(out recHostId, out recConnectionId, out recChannelId, recBuffer, bufferSize, out dataSize, out error);

        //Debug.Log(recConnectionId);
        switch (recData)
        {
            case NetworkEventType.Nothing:

                break;

            case NetworkEventType.ConnectEvent:
                UserInterface.DisableOtherTexts();
                Debug.Log("Connection was successful!!");
                connectionID = recConnectionId;
                Networking.isInitialized = false;
                net.StopBroadcast();

                if (!isClient)
                {
                    UserInterface.DisableMenuAndEnableGameInterface();
                    HostEnablesClientGame(false);
                    InitGame.CountDownEnabled = true;
                }
                break;

            case NetworkEventType.DataEvent:
                //Instantiate(Resources.Load("TimePower", typeof(GameObject)), new Vector3(0f, 0f, -2), Quaternion.Euler(0, 0, 0));
                Stream stream = new MemoryStream(recBuffer);
                BinaryFormatter formatter = new BinaryFormatter();
                string message = formatter.Deserialize(stream) as string;
                //Debug.Log("incoming message event received: " + message);
                ParseMessage(message);
                break;

            case NetworkEventType.DisconnectEvent:
                //Debug.Log("Disconnect Event Received");
           /////////////////////////////////////////////////////////////////
                ClientWantsToPlayAgain = false;
                HostWantsToPlayAgain = false;
           ////////////////////////////////////////////////////////////////////
                InitGame = GameObject.Find("Main Camera").GetComponent<InitGame>();
                UserInterface.ShowDisconnectScreen();
                InitGame.EndGame(true,999);
                UserInterface.DisableOtherTexts();
               
                break;

            case NetworkEventType.BroadcastEvent:
                //Instantiate(Resources.Load("IcePower", typeof(GameObject)), new Vector3(0f, 0f, -2), Quaternion.Euler(0, 0, 0));
                break;
        }
    }
    

    public void InitTransport(bool isClient)
    {
        this.isClient = isClient;
        NetworkTransport.Init();
        ConnectionConfig config = new ConnectionConfig();
        myReliableChannelId = config.AddChannel(QosType.Reliable);
        HostTopology topology = new HostTopology(config, 1);
        socketID = NetworkTransport.AddHost(topology, 8888);

        if (isClient)
        {
            byte error;
            connectionID = NetworkTransport.Connect(socketID, Networking.serverIP, 8888, 0, out error);
            //Debug.Log("i just made connection: " + connectionID);
        }
    }

   public void SendMyMessage(string textInput)
    {
        byte error;
        byte[] buffer = new byte[1024];
        Stream message = new MemoryStream(buffer);
        BinaryFormatter formatter = new BinaryFormatter();
        formatter.Serialize(message, textInput);
        NetworkTransport.Send(socketID, connectionID, myReliableChannelId, buffer, (int)message.Position, out error);

        //If there is an error, output message error to the console
        if ((NetworkError)error != NetworkError.Ok)
        {
            Debug.Log("Message send error: " + (NetworkError)error);
        }
    }

    void ParseMessage(string message)
    {
        switch (message)
        {
            case "enablegame":
                if (isClient)
                {
                    UserInterface.DisableMenuAndEnableGameInterface();
                    InitGame.CountDownEnabled = true;
                }
                break;
            case "restartgame":
                if (isClient)
                {
                    InitGame = GameObject.Find("Main Camera").GetComponent<InitGame>();
                    InitGame.ResetValues();
                    InitGame.CountDownEnabled = true;
                }
                break;
            case "enemyusedtime":
                UseTime.IncreaseFallSpeed();

                break;
            case "enemyusedice":
                UseIce.SpawnIceCrystals();
                break;
            case "enemyusedfire":
                UseFire.EnableFirePower();
                break;
            case "enemyusedstar":
                UseStar.EnableStarPower();
                break;
            case "enemylost":
                InitGame.EndGame(false,1);
                break;
            case "playagain":
                if (!isClient) 
                {
                    ClientWantsToPlayAgain = true;
                }
                break;
            default: /* Get enemy HP */              
                int.TryParse(message,out InitGame.EnemyHP);
                break;
        }
    }

    public void CheckForNewGame() //HOST checks if client wants to play and also IF you(HOST) want to play again ---> start new game
    {
        if (ClientWantsToPlayAgain && HostWantsToPlayAgain && !isClient) // code must only run on host. only hosts starts a new game
        {
            InitGame = GameObject.Find("Main Camera").GetComponent<InitGame>(); //its needed again here. causes error
            InitGame.ResetGame();
            ClientWantsToPlayAgain = false;
            HostWantsToPlayAgain = false;
        }
    }

    public void OnBackButtonClick()
    {
        byte error;
        //Debug.Log("conn id "+connectionID);
        if (!FireBase.OnlineGame) //Disconnect from lan connection only if the game is local
        {
            if (socketID >= 0 && connectionID != -1)
            {
                NetworkTransport.Disconnect(socketID, connectionID, out error);
                NetworkServer.Reset();
                Debug.Log("Disconnected from transport. socketid: " + socketID);
            }
            else
            {
                NetworkServer.Reset();
            }
        }

    }

    public void HostEnablesClientGame(bool RestartGame)
    {
        this.RestartGame = RestartGame;  
        if (RestartGame)
        {
            SendMyMessage("restartgame");
        }
        else
        {
            SendMyMessage("enablegame");
        }       
    }  
}
