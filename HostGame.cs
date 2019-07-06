using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
public class HostGame : MonoBehaviour
{
    [SerializeField]
    private uint RoomSize = 5;
    private string RoomName;

    private NetworkManager networkManager;
    private void Start()
    {
        networkManager = NetworkManager.singleton;//knowing there can only be one network manager, we use can use the singleton field to assing our network manager instance
        if (networkManager.matchMaker == null)
        {
            networkManager.StartMatchMaker();//if the network is not within the matchmaker then start the matchmaker
        }
    }
    public void SetRoomName(string name)
    {
        RoomName = name;//set room name to the string passed in 
    }
    public void CreateRoom()
    {
        if(RoomName != "" && RoomName != null)
        {
            Debug.Log("Creating Room: " + RoomName + " With room for " + RoomSize + " Players.");
            networkManager.matchMaker.CreateMatch(RoomName, RoomSize, true,"","","",0,0, networkManager.OnMatchCreate);//create the room with the given name, size and given ip adresses
        }

        
    }
    public void QuitGame()
    {
        Application.Quit(); // quit the application 
    }
}
