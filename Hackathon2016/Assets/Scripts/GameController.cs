using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameController : MonoBehaviour
{
    GameController controller;

    public int numberOfPlayers;
    public List<Player> players;

    GameObject playerPrefab;

    public bool addPlayer;
    public int playerID = 1;
    public PlayerClass newPlayerClass;
    public Color newPlayerColor;

    public Transform startPlayerPosition, endPlayerPosition;

    public void Awake()
    {
        if (controller == null)
        {
            controller = this;
        }
        else if (controller != null)
        {
            Destroy(gameObject);
        }
        players = new List<Player>();
        playerPrefab = Resources.Load<GameObject>("Prefabs/Player");
    }

    void Update()
    {
        if (addPlayer)
        {
            addPlayer = false;
            OnPlayerConnect(playerID, newPlayerClass, newPlayerColor);
            playerID++;
        }
    }

    public void OnPlayerConnect(int newPlayerId, PlayerClass playerClass, Color color)
    {
        players.Add(Instantiate<GameObject>(playerPrefab).GetComponent<Player>());
        players[players.Count - 1].name = "Player" + playerID;
        players[players.Count - 1].PlayerID = newPlayerId;
        players[players.Count - 1].CurrentColor = color;
        players[players.Count - 1].Setup(Helper.GetInformation(playerClass));
    }

    public void RepositionPlayers()
    {
        //Position all the players currently connected
        //one player: centerpoint of start and end
        //two players: centerpoints between center and start/center and end
        //three players:
    }
}
