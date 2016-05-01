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

        AirConsole.instance.onMessage += OnMessageReceived;
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

    void OnMessageReceived(int device_id, JToken data)
    {
        int activePlayerID = AirConsole.instance.ConvertDeviceIdToPlayerNumber(device_id);
        Debug.Log("Message recieved from device " + device_id + "  their player id is " + activePlayerID);
        int action = (int)data["a"];

        switch (action)
        {
            case 0:
                Debug.Log("Join.");
                GameController.controller.Join(device_id);
                break;
            case 1:
                Debug.Log("Setup");
                string name = (string)data["b"]["n"];
                PlayerClass playerClass = (PlayerClass)((int)data["b"]["c"]);
                int playerColor = (int)data["b"]["r"];
                GameController.controller.Setup(name, playerClass, playerColor, device_id);
                break;
            case 2:
                Debug.Log("Ready");
                GameController.controller.Ready(device_id);
                break;
            case 3:
                Debug.Log("Attack");
                GameController.controller.Attack(device_id);
                break;
            case 4:
                Debug.Log("Defend");
                GameController.controller.Defend(device_id);
                break;
            case 5:
                Debug.Log("Loot");
                GameController.controller.Loot(device_id);
                break;
            case 6:
                Debug.Log("KeepLoot");
                GameController.controller.KeepLoot(device_id);
                break;
            case 7:
                Debug.Log("ShareLoot");
                GameController.controller.ShareLoot(device_id);
                break;
            case 8:
                Debug.Log("Bid");
                int itemABid = (int)data["b"]["a"];
                int itemBBid = (int)data["b"]["b"];
                int itemCBid = (int)data["b"]["c"];
                GameController.controller.Bid(device_id, itemABid, itemBBid, itemCBid);
                break;
        }

        //switch (action)
        //{
        //    case "j":
        //        Debug.Log("Join");
        //        break;
        //    case "s":
        //        Debug.Log("Start");
        //        StartGame();
        //        break;
        //    case "a":
        //        // ATTACK BUTTON
        //        Debug.Log("attack");
        //        break;
        //    case "b":
        //        int itemABid = (int)data["b"]["a"];
        //        int itemBBid = (int)data["b"]["b"];
        //        int itemCBid = (int)data["b"]["c"];
        //        // HANDLE BID
        //        Debug.Log("Bid");
        //        break;
        //    case "d":
        //        // DEFEND BUTTON
        //        Debug.Log("Defend");
        //        break;
        //    case "cl":
        //        int playerClass = (int)data["i"];
        //        Debug.Log("Player class");
        //        break;
        //    case "co":
        //        int playerColor = (int)data["i"];
        //        Debug.Log("Player color");
        //        break;
        //    default:
        //        Debug.Log("Unknown input");
        //        break;
        //}
    }
    #endregion

    public void UpdateState(object data)
    {
        AirConsole.instance.Broadcast(data);
    }

    public void UpdateState(int deviceId, object data)
    {
        AirConsole.instance.Message(deviceId, data);
    }

    void StartGame()
    {
        int playerCount = AirConsole.instance.GetControllerDeviceIds().Count;
        Debug.Log("Player count: " + playerCount);
        AirConsole.instance.SetActivePlayers(playerCount);
    }
}