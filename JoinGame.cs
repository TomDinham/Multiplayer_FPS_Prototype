using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.Networking.Match;


public class JoinGame : MonoBehaviour
{
    private NetworkManager networkManager;
    List<GameObject> RoomList = new List<GameObject>();
    [SerializeField]
    private GameObject RoomListItemPrefab;
    [SerializeField]
    private Transform RoomListParent;
    [SerializeField]
    private Text status;
    private void Start()
    {
        networkManager = NetworkManager.singleton;//knowing there can only be one network manager, we use can use the singleton field to assing our network manager instance
        if(networkManager.matchMaker == null)
        {
            networkManager.StartMatchMaker();//if the network is not within the matchmaker then start the matchmaker
        }
        RefreshRooms();
    }

    public void RefreshRooms()
    {
        ClearRoomList();

        if(networkManager.matchMaker == null)
        {
            networkManager.StartMatchMaker();//if the network is not within the matchmaker then start the matchmaker
        }
        networkManager.matchMaker.ListMatches(0, 20, "",false,0,0, OnMatchList);//list all current matches on the match maker using the on match list function
        status.text = "Loading...";//set text
    }
    public void OnMatchList(bool success, string extendedInfo, List<MatchInfoSnapshot> matches)
    {
        status.text = "";//set text
        if (matches == null)
        {
            status.text = "couldn't get room list."; // if there are no mathces set text and return
            return;
        }
        ClearRoomList();
        foreach (MatchInfoSnapshot match in matches) // for each match in matches list 
        {
            GameObject roomListItem = Instantiate(RoomListItemPrefab);//Instantiate the button for the room 
            roomListItem.transform.SetParent(RoomListParent); //set the parent for the instanciated object 
            RoomListItem _RoomList = roomListItem.GetComponent<RoomListItem>();//assing the script componect from the Instantiated object
            if (_RoomList != null)
            {
                _RoomList.SetUp(match,JoinRoom);//call the setup function from the room list script and send the currennt match and the join room function
            }
            RoomList.Add(roomListItem);//add the Instantiated object to a list of rooms
        }
        if(RoomList.Count == 0)
        {
            status.text = "No Rooms Available"; // if no rooms set text
        }
        
    }
    void ClearRoomList()
    {
        for (int i = 0; i < RoomList.Count; i++)
        {
            Destroy(RoomList[i]); // clear the list of rooms
        }
        RoomList.Clear();

    }
    public void JoinRoom(MatchInfoSnapshot _match)
    {
        networkManager.matchMaker.JoinMatch(_match.networkId, "","","",0,0, networkManager.OnMatchJoined); // connect the client to the selected room via the IP adress
        StartCoroutine(WaitForJoin());
    }
    IEnumerator WaitForJoin()
    {
        //creating a timer for joining the room so that the player does not get stuck in joining if an error occurs, refresh the rooms if the timer ends
        ClearRoomList();
        
        int countdown = 10;
        while(countdown > 0)
        {
            status.text = "Joining..." +"(" + countdown + ")";
            yield return new WaitForSeconds(1);
            countdown--;
        }

        status.text = "Failed to connect!";
        yield return new WaitForSeconds(1);

        MatchInfo matchinfo = networkManager.matchInfo;
        if(matchinfo != null)
        {
            networkManager.matchMaker.DropConnection(matchinfo.networkId, matchinfo.nodeId, 0, networkManager.OnDropConnection);
            networkManager.StopHost();
        }
        

        RefreshRooms();
    }
}
