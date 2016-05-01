using UnityEngine;
using UnityEngine.UI;
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
    public GameObject titleScreen, storeScreen, endScreen;
    Text timerText;

    public Dictionary<int, int> playerDeviceIDs; //key=playerID, value=deviceID
    public List<int> availableIDS = new List<int> { 1, 2, 3, 4 };

    public bool lootOut;
    public GameObject chest;

    Dictionary<int, BidInformation> bids;
    public bool doneBidding;

    public Sprite chestClosed, chestOpen;

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
        timerText = storeScreen.GetComponentsInChildren<Text>()[0];
        storeScreen.SetActive(false);
        endScreen.SetActive(false);

        bids = new Dictionary<int, BidInformation>();
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
        yield return StartCoroutine(EnterDungeon(3));
        AudioController.controller.PlayBackgroundSong(SongType.End);
        yield return StartCoroutine(StartStoreEncounter());

        //Get ranking ready
        int goldRank1 = 0, goldRank2 = 0, goldRank3 = 0, goldRank4 = 0;
        Player rank1 = players[0], rank2 = players[0], rank3 = players[0], rank4 = players[0];
        foreach (Player p in players)
        {
            if (p.Gold > goldRank1)
            {
                goldRank1 = p.Gold;
                rank1 = p;
            }
        }
        foreach (Player p in players)
        {
            if (p.Gold > goldRank2 && p != rank1)
            {
                goldRank2 = p.Gold;
                rank2 = p;
            }
        }
        foreach (Player p in players)
        {
            if (p.Gold > goldRank3 && p != rank1 && p != rank2)
            {
                goldRank3 = p.Gold;
                rank3 = p;
            }
        }
        foreach (Player p in players)
        {
            if (p != rank1 && p != rank2 && p != rank3)
            {
                goldRank4 = p.Gold;
                rank4 = p;
            }
        }
        switch (players.Count)
        {
            case 1: endScreen.GetComponent<EndGameRank>().ShowRank(new Player[] { rank1 }); break;
            case 2: endScreen.GetComponent<EndGameRank>().ShowRank(new Player[] { rank1, rank2 }); break;
            case 3: endScreen.GetComponent<EndGameRank>().ShowRank(new Player[] { rank1, rank2, rank3 }); break;
            case 4: endScreen.GetComponent<EndGameRank>().ShowRank(new Player[] { rank1, rank2, rank3, rank4 }); break;
        }

        endScreen.SetActive(true);
        if (!testing)
        {
            var message2 = new
            {
                z = 0
            };
            AirConsoleController.instance.UpdateState(message2);
        }
        adventuring = false;

        yield return null;
    }



    IEnumerator EnterDungeon(int numberOfEnemies = 3)
    {
        Information[] enemies = Helper.GetProgression(numberOfEnemies);

        Debug.Log("Entering dungeon");
        for (int i = 0; i < numberOfEnemies; i++)
        {
            //Set up currentEnemy to the next enemy
            currentEnemy.gameObject.SetActive(true);
            currentEnemy.Setup(enemies[i]);
            currentEnemy.enemyClass = enemies[i].eClass;
            Debug.Log("Enemy " + currentEnemy.isActiveAndEnabled + currentEnemy.name);
            yield return StartCoroutine(StartEnemyEncounter());

        }
        Debug.Log("Killed all the enemies!");

        //Revive the players after the dungeon and before the store
        foreach (Player p in deadPlayers)
        {
            players.Add(p);
        }
        int j = deadPlayers.Count;
        for (int k = 0; k < j; k++)
        {
            deadPlayers.Remove(deadPlayers[0]);
        }
        foreach (Player p in players)
        {
            p.Health = p.MaxHealth;
            p.Dead = false;
            p.gameObject.SetActive(true);
        }
        RepositionPlayers();
        //Inside a dungeon - there will be several enemies/boss and then at the end of the dungeon there will be a shop encounter
        //A dungeon is a series of encounters with enemies - no breaks in between
        yield return null;
    }


    IEnumerator StartEnemyEncounter()
    {
        if (!testing)
        {
            var message = new
            {
                e = 0
            };
            AirConsoleController.instance.UpdateState(message);
        }
        Debug.Log("A wild pikachu has appeared! " + currentEnemy.Health);
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
        chest.GetComponent<SpriteRenderer>().sprite = chestClosed;
        chest.transform.GetChild(0).gameObject.SetActive(false);

        if (!testing)
        {
            var message = new
            {
                l = 0
            };
            AirConsoleController.instance.UpdateState(message);
        }
        lootOut = true;
        yield return new WaitForSeconds(Random.Range(treasureRange.x, treasureRange.y));
        if (!testing)
        {
            var message2 = new
            {
                l = 1
            };
            AirConsoleController.instance.UpdateState(message2);
        }
        while (lootOut)
        {
            chest.SetActive(true);
            //do a countdown and then randomly show the loot after 1-3 seconds (first to claim it can distribute as desired)
            Debug.Log("Waiting for loot!");
            yield return null;
        }

        chest.transform.GetChild(0).gameObject.SetActive(true);
        chest.GetComponent<SpriteRenderer>().sprite = chestOpen;
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

    float storeTime = 15f;

    IEnumerator StartStoreEncounter()
    {
        storeScreen.SetActive(true);
        Debug.Log("You've reached the store!");
        if (!testing)
        {
            var message = new
            {
                p = 0
            };
            AirConsoleController.instance.UpdateState(message);
        }
        float currentTime = storeTime;

        while (currentTime > 0)
        {
            timerText.text = currentTime.ToString();
            currentTime -= 1;
            yield return new WaitForSeconds(1f);
        }
        timerText.text = "Processing bids...";

        //Request bids from the phones and then wait for all the responses
        if (!testing)
        {
            var message = new
            {
                b = 0
            };
            AirConsoleController.instance.UpdateState(message);
        }

        while (!doneBidding)
        {
            yield return null;
        }

        //check th bids - highest for each item wins - nobody gets it if there is a tie
        int highBid1 = 0, highBid2 = 0, highBid3 = 0;
        Player highBidder1 = null, highBidder2 = null, highBidder3 = null;
        foreach (Player p in players)
        {
            if (bids[p.DeviceID].bid1 > highBid1)
            {
                highBid1 = bids[p.DeviceID].bid1;
                highBidder1 = p;
            }
            if (bids[p.DeviceID].bid2 > highBid2)
            {
                highBid2 = bids[p.DeviceID].bid2;
                highBidder2 = p;
            }
            if (bids[p.DeviceID].bid3 > highBid3)
            {
                highBid3 = bids[p.DeviceID].bid3;
                highBidder3 = p;
            }
        }
        //Now we have the highest bid, so check if there are multiple of that bid and if so, nobody gets that item, if not the highest bidder gets the item, if the bid is 0, nobody gets it
        int nBid1 = 0, nBid2 = 0, nBid3 = 0;

        foreach (Player p in players)
        {
            if (bids[p.DeviceID].bid1 == highBid1) nBid1++;
            if (bids[p.DeviceID].bid2 == highBid2) nBid2++;
            if (bids[p.DeviceID].bid3 == highBid3) nBid3++;
        }

        //Check item 1:
        if (nBid1 != 1 || highBid1 == 0)
        {
            Debug.Log("Nobody gets anything!");
        }
        else
        {
            //Only items with a single high bid != 0
            highBidder1.DefensePower += 5;
            highBidder1.Gold -= highBid1;
            UpdateStats(highBidder1);
        }
        //Check item 2:
        if (nBid2 != 1 || highBid2 == 0)
        {
            Debug.Log("Nobody gets anything!");
        }
        else
        {
            //Only items with a single high bid != 0
            highBidder2.MaxHealth += 20;
            highBidder2.Gold -= highBid2;
            UpdateStats(highBidder2);
        }
        //Check item 3:
        if (nBid3 != 1 || highBid3 == 0)
        {
            Debug.Log("Nobody gets anything!");
        }
        else
        {
            //Only items with a single high bid != 0
            highBidder3.AttackPower += 5;
            highBidder3.Gold -= highBid3;
            UpdateStats(highBidder3);
        }
        timerText.text = "Bids processed!";

        yield return new WaitForSeconds(5f);
        if (!testing)
        {
            var message2 = new
            {
                r = 0
            };
            AirConsoleController.instance.UpdateState(message2);
        }
        //send each player a screen for bidding on store items - only the highest bid for each item will be accepted
        storeScreen.SetActive(false);
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

    public bool testing = true;
    public void UpdateStats(Player p)
    {
        if (testing)
        {
            Debug.Log("Testing, won't update phone stats.");
        }
        else
        {
            if (!testing)
            {

                var message = new
                {
                    a = new { d = p.DefensePower, h = (int)((p.Health * 1f / p.MaxHealth * 1f) * 100), g = p.Gold, a = p.AttackPower }
                };
                AirConsoleController.instance.UpdateState(p.DeviceID, message);
            }
        }
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
        playerToKill.Dead = true;
        AudioController.controller.PlaySound(SoundType.Death);
        StartCoroutine(TurnOffPlayer(playerToKill));

        if (players.Count == 0)
        {
            AudioController.controller.PlayBackgroundSong(SongType.Game);
            endScreen.GetComponent<EndGameRank>().ShowDeadRank(deadPlayers);
            endScreen.SetActive(true);
            if (!testing)
            {
                var message = new
                {
                    x = 0
                };
                AirConsoleController.instance.UpdateState(message);
            }
        }
        else
        {
            if (!testing)
            {
                var message = new
                {
                    d = 0
                };
                AirConsoleController.instance.UpdateState(playerToKill.DeviceID, message);
            }
        }
    }
    IEnumerator TurnOffPlayer(Player playerToKill)
    {
        yield return new WaitForSeconds(0.75f);
        playerToKill.gameObject.SetActive(false);
        RepositionPlayers();
        yield return null;
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
            if (!testing)
            {
                var message = new
                {
                    e = 1
                };

                AirConsoleController.instance.UpdateState(message);
            }
            waitingForPlayers = false;
        }
    }

    public void Attack(int deviceID)
    {
        Debug.Log("Attacking");
        Player p = GetPlayer(deviceID);
        if (p != null)
        {
            p.attack = true;
        }
        //GetPlayer(deviceID).attack = true;
    }

    public void Defend(int deviceID)
    {
        Debug.Log("Defending");
        Player p = GetPlayer(deviceID);
        if (p != null)
        {
            p.defend = true;
        }
        //GetPlayer(deviceID).defend = true;
    }

    public int lootLow = 50, lootHigh = 150;

    public void Loot(int deviceID)
    {
        lootOut = false;
        if (!testing)
        {
            var message = new
            {
                l = 2
            };
            AirConsoleController.instance.UpdateState(deviceID, message);
        }
        foreach (Player p in players)
        {
            if (p.DeviceID != deviceID && !p.Dead)
            {
                Debug.Log(p.Dead);
                if (!testing)
                {
                    var message3 = new
                    {
                        l = 3
                    };
                    AirConsoleController.instance.UpdateState(p.DeviceID, message3);
                }
            }
        }

    }

    public void KeepLoot(int deviceID)
    {
        GetPlayer(deviceID).Gold += Random.Range(lootLow, lootHigh);
        UpdateStats(GetPlayer(deviceID));
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
        UpdateAllStats();
    }

    public void Bid(int deviceID, int item1Bid, int item2Bid, int item3Bid)
    {
        //add to the bid list, if it's not the length of # players wait
        if (!bids.ContainsKey(deviceID))
        {
            //new bid, add it to the list and check if it's the last bid or not
            bids[deviceID] = new BidInformation(deviceID, item1Bid, item2Bid, item3Bid);
        }

        if (bids.Count == players.Count)
        {
            doneBidding = true;
        }
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
