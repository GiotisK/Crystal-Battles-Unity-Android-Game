using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Networking : NetworkDiscovery {
    public static bool isInitialized = false;
    public static string serverIP;
    public static bool foundClient = false;

    public GameObject otherObject; 

    public override void OnReceivedBroadcast(string fromAddress, string data) 
    {
        foundClient = true;
        Debug.Log("Received broadcast from: " + fromAddress + " with the data: " + data);
        //Instantiate(Resources.Load("FirePower", typeof(GameObject)), new Vector3(0f, 0f, -2), Quaternion.Euler(0, 0, 0));
        serverIP = fromAddress;
        GameObject.Find("Network").GetComponent<TransportLayer>().enabled = true;
        GameObject.Find("Network").GetComponent<TransportLayer>().InitTransport(true);

    }



    public void OnButtonClick(bool isServer) //isServer takes value from component
    {
        if (isServer) // host game (server)
        {
            GameObject.Find("Network").GetComponent<TransportLayer>().enabled = true;
            GameObject.Find("Network").GetComponent<TransportLayer>().InitTransport(false);
            isInitialized = Initialize();
            StartAsServer();

        }
        else // join game (client)
        {
            isInitialized = Initialize();
            StartAsClient();
        }

        
    }
    public void OnBackButtonClick()
    {   
        if (isInitialized)
        {
            isInitialized = false;          
            StopBroadcast();
            Debug.Log("BroadcastStopped");         
        }
        
    }
}
