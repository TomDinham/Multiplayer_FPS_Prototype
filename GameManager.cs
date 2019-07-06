using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
   
    public static GameManager instance;
    public MatchSettings MatchSettings;
    [SerializeField]
    private GameObject sceneCam;
    private static Dictionary<string, PlayerManager> Players = new Dictionary<string, PlayerManager>();
    private const string PLAYER_ID_CONSTANT = "Player";

    private void Awake()
    {
        if(instance != null) //if the instance of the game manager is not null then there are more than one game manager components in the scene
        {
            Debug.LogError("More than one GameManager in scene");
                
        }
        else
        { instance = this; } // set instance to this instance of the game manager
        
    }

    public static void RegisterPlayer(string NetID, PlayerManager player)
    {
        string playerID = PLAYER_ID_CONSTANT + NetID;// setting the player id to constant string of player, and their network id value
        Players.Add(playerID, player); //adding the player to the list with their player id 
        player.transform.name = playerID;//set the players name to the players ID
    }

    public static void UnRegisterPlayer(string playerID)
    {
        Players.Remove(playerID);//remove the player with the corresponding id from the player list 
    }

    public static PlayerManager GetPlayer(string playerID)
    {
        return Players[playerID]; //  return the matching player with the corresponding id 
    }
    public void SetSceneCam(bool isActive)
    {
        if (sceneCam == null)
            return;

        sceneCam.SetActive(isActive);
    }
 
 
}
