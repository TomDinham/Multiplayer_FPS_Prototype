using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking.Match;
using UnityEngine.UI;
public class RoomListItem : MonoBehaviour
{
    public delegate void JoinGameDelegate(MatchInfoSnapshot match);
    public JoinGameDelegate joinGameCallBack;
    
    [SerializeField]
    private Text RoomItemText;
    private MatchInfoSnapshot Match;

    //set the room list item text and function for joining the mathch
    public void SetUp(MatchInfoSnapshot match,JoinGameDelegate _joinGameCallBack)
    {
        Match = match;
        joinGameCallBack = _joinGameCallBack;

        RoomItemText.text = Match.name + "(" + Match.currentSize + "/" + Match.maxSize + ")";
    }
    //call the function passed into the join game function with the paramater of the match info
    public void JoinGame()
    {
        joinGameCallBack.Invoke(Match);
    }
}
