using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using NDream.AirConsole;
using Newtonsoft.Json.Linq;

public class AirConsoleController : MonoBehaviour
{
    public static AirConsoleController instance;
    public int minPlayerCount = 1;

    void Awake()
    {
        if (instance != null) { Destroy(this); }
        else { instance = this; }

        AirConsole.instance.onMessage += OnMessage;
        AirConsole.instance.onConnect += OnConnect;
        AirConsole.instance.onDisconnect += OnDisconnect;
    }

    #region AirConsole
    // Start the game if 2 players are connected, game not running (activePlayers == null)
    void OnConnect(int device_id)
    {
        Debug.Log("Connect " + device_id);
        // If no one is set up yet
        if (AirConsole.instance.GetActivePlayerDeviceIds.Count == 0)
        {
            // If we have the minimum # of connected devices
            if (AirConsole.instance.GetControllerDeviceIds().Count >= minPlayerCount)
            {
                // Ready to play
            }
        }
    }

    void OnDisconnect(int device_id)
    {
        int activePlayer = AirConsole.instance.ConvertDeviceIdToPlayerNumber(device_id);
        Debug.Log("Disconnect " + activePlayer);
        if (AirConsole.instance.GetControllerDeviceIds().Count < minPlayerCount)
        {
            // NOT ENOGUH PLAYERS
        }
    }

    void OnMessage(int device_id, JToken data)
    {
        int activePlayer = AirConsole.instance.ConvertDeviceIdToPlayerNumber(device_id);
        Debug.Log("Active player " + activePlayer);
        string action = (string)data["a"];
        switch (action)
        {
            case "j":
                Debug.Log("Join");
                break;
            case "s":
                Debug.Log("Start");
                StartGame();
                break;
            case "a":
                // ATTACK BUTTON
                Debug.Log("attack");
                break;
            case "b":
                int itemABid = (int)data["i"]["a"];
                int itemBBid = (int)data["i"]["b"];
                int itemCBid = (int)data["i"]["c"];
                // HANDLE BID
                Debug.Log("Bid");
                break;
            case "d":
                // DEFEND BUTTON
                Debug.Log("Defend");
                break;
            case "cl":
                int playerClass = (int)data["i"];
                Debug.Log("Player class");
                break;
            case "co":
                int playerColor = (int)data["i"];
                Debug.Log("Player color");
                break;
            default:
                Debug.Log("Unknown input");
                break;
        }
    }
    #endregion

    void StartGame()
    {
        int playerCount = AirConsole.instance.GetControllerDeviceIds().Count;
        Debug.Log("Player count: " + playerCount);
        AirConsole.instance.SetActivePlayers(playerCount);
    }
}