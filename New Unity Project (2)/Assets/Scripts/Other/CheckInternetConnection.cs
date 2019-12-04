using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CheckInternetConnection : MonoBehaviour {
    public TextMeshProUGUI InternetConnectionCheckText;
    //public GameObject MultiPlayerButton;
    public Button MultiPlayerButton;

    private void Start()
    {
        MultiPlayerButton = GameObject.Find("MultiPlayer").GetComponent<Button>();
    }
    void Update () {
        if (Application.internetReachability == NetworkReachability.NotReachable && InternetConnectionCheckText.enabled == false)
        {
            //Debug.Log("Error. Check internet connection!");
            InternetConnectionCheckText.enabled = true;
            MultiPlayerButton.interactable = false;
        }
        else if(Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork
             || Application.internetReachability == NetworkReachability.ReachableViaCarrierDataNetwork 
             && InternetConnectionCheckText.enabled == true)
        {
           // Debug.Log("Connection is ok");
            InternetConnectionCheckText.enabled = false;
            MultiPlayerButton.interactable = true;
        }

    }
}
