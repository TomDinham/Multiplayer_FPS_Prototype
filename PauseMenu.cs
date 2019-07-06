using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;

public class PauseMenu : MonoBehaviour
{
    public static bool isOn = false;
    private NetworkManager NM;
    private void Start()
    {
        NM = NetworkManager.singleton;//knowing there can only be one network manager, we use can use the singleton field to assing our network manager instance
    }
    public void LeaveRoom()//removes the client from their current room and drops the host, so if the client is also the host of the game then the room will be closed too
    {
        MatchInfo matchinfo = NM.matchInfo;
        NM.matchMaker.DropConnection(matchinfo.networkId, matchinfo.nodeId, 0, NM.OnDropConnection);
        NM.StopHost();
    }
}
