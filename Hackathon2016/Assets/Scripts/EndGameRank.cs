using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class EndGameRank : MonoBehaviour
{
    public Text player1Name, player2Name, player3Name, player4Name;
    public Text player1Gold, player2Gold, player3Gold, player4Gold;
    public GameObject namepanel1, namepanel2, namepanel3, namepanel4;
    public GameObject goldpanel1, goldpanel2, goldpanel3, goldpanel4;

    public void ShowRank(Player[] rankedPlayers)
    {
        Debug.Log("Showing rank");
        switch (rankedPlayers.Length)
        {
            case 1:
                player1Name.text = rankedPlayers[0].name;
                player1Gold.text = rankedPlayers[0].Gold.ToString();
                namepanel2.SetActive(false);
                goldpanel2.SetActive(false);
                namepanel3.SetActive(false);
                goldpanel3.SetActive(false);
                namepanel4.SetActive(false);
                goldpanel4.SetActive(false);
                break;
            case 2:
                player1Name.text = rankedPlayers[0].name;
                player1Gold.text = rankedPlayers[0].Gold.ToString();
                player2Name.text = rankedPlayers[1].name;
                player2Gold.text = rankedPlayers[1].Gold.ToString();
                namepanel3.SetActive(false);
                goldpanel3.SetActive(false);
                namepanel4.SetActive(false);
                goldpanel4.SetActive(false);
                break;
            case 3:
                player1Name.text = rankedPlayers[0].name;
                player1Gold.text = rankedPlayers[0].Gold.ToString();
                player2Name.text = rankedPlayers[1].name;
                player2Gold.text = rankedPlayers[1].Gold.ToString();
                player3Name.text = rankedPlayers[2].name;
                player3Gold.text = rankedPlayers[2].Gold.ToString();
                namepanel4.SetActive(false);
                goldpanel4.SetActive(false);
                break;
            case 4:
                player1Name.text = rankedPlayers[0].name;
                player1Gold.text = rankedPlayers[0].Gold.ToString();
                player2Name.text = rankedPlayers[1].name;
                player2Gold.text = rankedPlayers[1].Gold.ToString();
                player3Name.text = rankedPlayers[2].name;
                player3Gold.text = rankedPlayers[2].Gold.ToString();
                player4Name.text = rankedPlayers[3].name;
                player4Gold.text = rankedPlayers[3].Gold.ToString();
                break;
        }

    }

    public void ShowDeadRank(List<Player> deadPlayers)
    {
        Player[] players = new Player[deadPlayers.Count];
        for (int i = 0; i < players.Length; i++)
        {
            players[i] = deadPlayers[i];
        }

        ShowDeadRank(players);
    }

    public void ShowDeadRank(Player[] deadPlayers)
    {
        Debug.Log("Showing rank");
        switch (deadPlayers.Length)
        {
            case 1:
                player1Name.text = deadPlayers[0].name + " died.";
                player1Gold.text = deadPlayers[0].Gold.ToString();
                namepanel2.SetActive(false);
                goldpanel2.SetActive(false);
                namepanel3.SetActive(false);
                goldpanel3.SetActive(false);
                namepanel4.SetActive(false);
                goldpanel4.SetActive(false);
                break;
            case 2:
                player1Name.text = deadPlayers[0].name + " died.";
                player1Gold.text = deadPlayers[0].Gold.ToString();
                player2Name.text = deadPlayers[1].name + " died.";
                player2Gold.text = deadPlayers[1].Gold.ToString();
                namepanel3.SetActive(false);
                goldpanel3.SetActive(false);
                namepanel4.SetActive(false);
                goldpanel4.SetActive(false);
                break;
            case 3:
                player1Name.text = deadPlayers[0].name + " died.";
                player1Gold.text = deadPlayers[0].Gold.ToString();
                player2Name.text = deadPlayers[1].name + " died.";
                player2Gold.text = deadPlayers[1].Gold.ToString();
                player3Name.text = deadPlayers[2].name + " died.";
                player3Gold.text = deadPlayers[2].Gold.ToString();
                namepanel4.SetActive(false);
                goldpanel4.SetActive(false);
                break;
            case 4:
                player1Name.text = deadPlayers[0].name + " died.";
                player1Gold.text = deadPlayers[0].Gold.ToString();
                player2Name.text = deadPlayers[1].name + " died.";
                player2Gold.text = deadPlayers[1].Gold.ToString();
                player3Name.text = deadPlayers[2].name + " died.";
                player3Gold.text = deadPlayers[2].Gold.ToString();
                player4Name.text = deadPlayers[3].name + " died.";
                player4Gold.text = deadPlayers[3].Gold.ToString();
                break;
        }
    }
}
