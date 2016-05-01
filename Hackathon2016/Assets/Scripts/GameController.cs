using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameController : MonoBehaviour
{
    public static GameController controller;

    public int numberOfPlayers;
    public List<Player> players, deadPlayers;

    GameObject playerPrefab;

    public bool addPlayer;
    public int playerID = 0;
    public int[] playerColors = { 0, 1, 2, 3, 4, 5, 6, 7 };
    public List<string> randomNames;

    public Transform startPlayerPosition, endPlayerPosition;

    public bool adventuring = false, waitingForPlayers = true;

    public Enemy currentEnemy; //there will only ever be one enemy at a time
    public GameObject titleScreen;

    public Dictionary<int, int> playerDeviceIDs; //key=playerID, value=deviceID
    public List<int> availableIDS = new List<int> { 1, 2, 3, 4 };

    public bool lootOut;
    public GameObject chest;

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
        playerDeviceIDs = new Dictionary<int, int>();
        playerPrefab = Resources.Load<GameObject>("Prefabs/Player");
    }

    void Start()
    {
        StartCoroutine(WaitForPlayers());
        AudioController.controller.PlayBackgroundSong(SongType.TitleScreen);
    }

    void Update()
    {
        if (addPlayer && players.Count < 4)
        {
            addPlayer = false;
            OnPlayerConnect(playerID, (PlayerClass)Random.Range(0, 2), playerColors[playerID], GetName());
            playerID++;
        }

        //foreach (Player p in players)
        //{
        //    int id = p.PlayerID + 1;
        //    if (Input.GetButtonDown("Player" + id + "Attack"))
        //    {
        //        p.attack = true;
        //    }
        //    else if (Input.GetButtonDown("Player" + id + "Defend"))
        //    {
        //        p.defend = true;
        //    }
        //}
        //Input will be coming from somewhere else (the phones)
    }

    IEnumerator WaitForPlayers()
    {
        titleScreen.SetActive(true);
        while (waitingForPlayers)
        {
            //Debug.Log("Waiting for more players to join!");
            //wait for new players to join (either 4, or all players have indicated they are ready to go)
            yield return null;
        }
        waitingForPlayers = false;
        StartCoroutine(StartAdventure()); //This will start the actual game
        yield return null;
    }

    IEnumerator StartAdventure()
    {
        AudioController.controller.PlayBackgroundSong(SongType.Game);

        adventuring = true;
        Debug.Log("Starting adventure!"); //This is the actual game part

        //For now there will be a single dungeon and then a single store encounter

        //Wait for all players to indicate they are ready to move forward? - send event to all players and wait for response
        //When all players are ready to move forward, the dungeon starts.
        yield return StartCoroutine(EnterDungeon());
        yield return StartCoroutine(StartStoreEncounter());
        AudioController.controller.PlayBackgroundSong(SongType.End);

        adventuring = false;
        yield return null;
    }

    IEnumerator EnterDungeon(int numberOfEnemies = 3)
    {
        Information[] enemies = Helper.GetRandomEnemies(numberOfEnemies);

        Debug.Log("Entering dungeon");
        for (int i = 0; i < numberOfEnemies; i++)
        {
            //Set up currentEnemy to the next enemy
            currentEnemy.gameObject.SetActive(true);
            currentEnemy.Setup(enemies[i]);
            Debug.Log("Enemy " + currentEnemy.isActiveAndEnabled + currentEnemy.name);
            yield return StartCoroutine(StartEnemyEncounter());

        }
        Debug.Log("Killed all the enemies!");
        //Inside a dungeon - there will be several enemies/boss and then at the end of the dungeon there will be a shop encounter
        //A dungeon is a series of encounters with enemies - no breaks in between
        yield return null;
    }


    IEnumerator StartEnemyEncounter()
    {
        Debug.Log("A wild pikachu has appeared! " + currentEnemy.Health);
        var message = new
        {
            e = 0
        };
        AirConsoleController.instance.UpdateState(message);
        // The enemy will appear and be setup
        // Players can attack and defend (based on their cooldowns)
        // The enemy will attack according to it's schedule
        // If a player runs out of health, they die for that battle
        // If the enemy runs out of health, the players win

        while (currentEnemy.Health > 0)
        {
            //currentEnemy.Health -= 100;
            // the enemy is still alive - wait for the players to get working
            yield return null;
        }
        currentEnemy.gameObject.SetActive(false);
        yield return StartCoroutine(BattleEnd());
        yield return null;
    }

    public bool waitingForLootDecision;

    Vector2 treasureRange = new Vector2(1f, 3f);
    IEnumerator BattleEnd()
    {
        Debug.Log("Battle is over!");

        var message = new
        {
            l = 0
        };
        AirConsoleController.instance.UpdateState(message);

        lootOut = true;
        yield return new WaitForSeconds(Random.Range(treasureRange.x, treasureRange.y));
            var message2 = new
            {
                l = 1
            };
            AirConsoleController.instance.UpdateState(message2);
        while (lootOut)
        {
            chest.SetActive(true);
            //do a countdown and then randomly show the loot after 1-3 seconds (first to claim it can distribute as desired)
            Debug.Log("Waiting for loot!");
            yield return null;
        }
        //Someone has the loot
        waitingForLootDecision = true;
        while (waitingForLootDecision)
        {
            //
            yield return null;
        }

        chest.SetActive(false);

        //show the loot and let the players fight for it!
        yield return null;
    }

    IEnumerator StartStoreEncounter()
    {
        Debug.Log("You've reached the store!");
        //send each player a screen for bidding on store items - only the highest bid for each item will be accepted
        yield return null;
    }

    public void AttackEnemy(int amount)
    {
        currentEnemy.TakeDamage(amount);
    }
    public void OnPlayerConnect(int newPlayerId, int playerClass, int color, string playerName)
    {
        OnPlayerConnect(newPlayerId, GetClass(playerClass), color, playerName);
    }

    public void OnPlayerConnect(int deviceID, PlayerClass playerClass, int color, string playerName)
    {
        // If the title screen is alive and well, drop it 
        if (titleScreen.activeInHierarchy && !titleScreen.GetComponent<TitleScreenDrop>().dropping)
        {
            titleScreen.GetComponent<TitleScreenDrop>().drop = true;
        }
        players.Add(Instantiate<GameObject>(playerPrefab).GetComponent<Player>());
        //players[players.Count - 1].name = "Player" + playerID;
        players[players.Count - 1].NameText = playerName;
        players[players.Count - 1].DeviceID = deviceID;
        players[players.Count - 1].CurrentColor = color;
        players[players.Count - 1].PClass = playerClass;
        players[players.Count - 1].Setup(Helper.GetInformation(playerClass));

        RepositionPlayers();
        UpdateStats(players[players.Count - 1]);
    }

    public void UpdateAllStats()
    {
        foreach (Player p in players)
        {
            UpdateStats(p);
        }
    }

    public void UpdateStats(Player p)
    {
        var message = new
        {
            a = new { d = p.DefensePower, h = (p.Health / p.MaxHealth) * 100, g = p.Gold, a = p.AttackPower }
        };
        AirConsoleController.instance.UpdateState(p.DeviceID, message);
    }

    Vector3 centerPoint, delta;
    public void RepositionPlayers()
    {
        delta = ((endPlayerPosition.position - startPlayerPosition.position) / 10f); //logic tells me it should be 8, but it doesn't look right, so I'm going with 10
        centerPoint = startPlayerPosition.position + ((endPlayerPosition.position - startPlayerPosition.position) / 2f);
        switch (players.Count)
        {
            case 1:
                players[0].transform.position = centerPoint; break;
            case 2:
                players[0].transform.position = centerPoint + ((startPlayerPosition.position - centerPoint) / 2f) + delta;
                players[1].transform.position = centerPoint - ((centerPoint - endPlayerPosition.position) / 2f) - delta; break;
            case 3:
                players[0].transform.position = startPlayerPosition.position;
                players[1].transform.position = centerPoint;
                players[2].transform.position = endPlayerPosition.position; break;
            case 4:
                players[0].transform.position = startPlayerPosition.position;
                players[1].transform.position = centerPoint + ((startPlayerPosition.position - centerPoint) / 2f) + delta;
                players[2].transform.position = centerPoint - ((centerPoint - endPlayerPosition.position) / 2f) - delta;
                players[3].transform.position = endPlayerPosition.position; break;
        }
        //Position all the players currently connected
        //one player: centerpoint of start and end
        //two players: centerpoints between center and start/center and end
        //three players: centerpoint and start and end
        //four players: one at start and end, and positions of only two players
    }

    //public Color GetColor(int colorIndex)
    //{
    //    return playerColors[colorIndex];
    //}

    public PlayerClass GetClass(int c)
    {
        return (PlayerClass)c;
    }

    string GetName()
    {
        string name = randomNames[Random.Range(0, randomNames.Count - 1)];
        randomNames.Remove(name);
        return name;
    }

    public void KillPlayer(Player playerToKill)
    {
        players.Remove(playerToKill);
        playerToKill.explosion.SetActive(true);
        playerToKill.gameObject.SetActive(false);
        AudioController.controller.PlaySound(SoundType.Death);
        RepositionPlayers();

        var message = new
        {
            d = 0
        };
        AirConsoleController.instance.UpdateState(playerToKill.DeviceID, message);
    }

    public void Join(int deviceID)
    {
        if (players.Count < 4)
        {
            //connect the player
            var message = new
            {
                s = 1
            };
            AirConsoleController.instance.UpdateState(deviceID, message);
        }
        else
        {
            //tell them to go home
            var message = new
            {
                f = 1
            };
            AirConsoleController.instance.UpdateState(deviceID, message);
        }
    }

    public void Setup(string playerName, PlayerClass playerClass, int color, int deviceID)
    {
        Debug.Log("Setup was called");
        //int playerID = availableIDS[0];
        //availableIDS.Remove(playerID);
        //playerDeviceIDs[playerID] = deviceID;
        //this will call on player connect
        OnPlayerConnect(deviceID, playerClass, color, playerName);
    }

    public void Ready(int deviceID)
    {
        // if all connected players are ready, then start the game and update the game state on each phone
        // otherwise, wait for another player to connect
        GetPlayer(deviceID).readyToAdventure = true;
        bool allPlayersReady = true;
        foreach (Player p in players)
        {
            if (!p.readyToAdventure) allPlayersReady = false;
        }
        if (allPlayersReady)
        {
            var message = new
            {
                e = 1
            };

            AirConsoleController.instance.UpdateState(message);
            waitingForPlayers = false;
        }
    }

    public void Attack(int deviceID)
    {
        Debug.Log("Attacking");
        GetPlayer(deviceID).attack = true;
    }

    public void Defend(int deviceID)
    {
        Debug.Log("Defending");
        GetPlayer(deviceID).defend = true;
    }

    public int lootLow = 50, lootHigh = 150;

    public void Loot(int deviceID)
    {
        lootOut = false;
        var message = new
        {
            l = 2
        };
        AirConsoleController.instance.UpdateState(deviceID, message);

        foreach (Player p in players)
        {
            if (p.DeviceID != deviceID)
            {
                var message3 = new
                {
                    l = 3
                };
                AirConsoleController.instance.UpdateState(p.DeviceID, message3);
            }
        }

    }

    public void KeepLoot(int deviceID)
    {
        GetPlayer(deviceID).Gold += Random.Range(lootLow, lootHigh);
        waitingForLootDecision = false;
    }

    public void ShareLoot(int deviceID)
    {
        int shareAmount = Random.Range(lootLow, lootHigh) / players.Count;
        foreach (Player p in players)
        {
            p.Gold += shareAmount;
        }
        waitingForLootDecision = false;
    }

    public void Bid(int deviceID, int item1Bid, int item2Bid, int item3Bid)
    {
        //add to the bid list, if it's not the length of # players wait
    }

    Player GetPlayer(int id)
    {
        foreach (Player p in players)
        {
            if (p.DeviceID == id) return p;
        }
        return null;
    }
}
