using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
[RequireComponent(typeof(PlayerManager))]//must have a player manager component on the object to compile
public class PlayerSetUp : NetworkBehaviour
{
    [SerializeField]
    Behaviour[] ComponentsToDisable;
    [SerializeField]
    string remoteLayerName = "RemotePlayer";
    [SerializeField]
    GameObject playerUIPrefab;
    private GameObject playerUIInstance;

    
    private void Start()
    {
        if(!isLocalPlayer)
        {//if this is not the local player then send to the following functions 
            DisableComps();
            AssignLayer();
        }
        else
        {
           
            playerUIInstance = Instantiate(playerUIPrefab); // instanciate player UI
            playerUIInstance.name = playerUIPrefab.name;//set player name 
        }
        GetComponent<PlayerManager>().Setup();//set up player
       
    }

    public override void OnStartClient()//this function will run when each client connects
    {
        base.OnStartClient();
        string netID = GetComponent<NetworkIdentity>().netId.ToString();//set the net id to the corrosponding network id
        PlayerManager player = GetComponent<PlayerManager>();

        GameManager.RegisterPlayer(netID, player);//register the player into the player dictionary 
    }
    void DisableComps()
    {
        for (int i = 0; i < ComponentsToDisable.Length; i++)
        {
            ComponentsToDisable[i].enabled = false;//disable the correct components for a non local player 
        }
    }
    void AssignLayer()
    {
        gameObject.layer = LayerMask.NameToLayer(remoteLayerName);//assign the correct layer for the player
    }
    private void OnDisable()
    {
        Destroy(playerUIInstance); // destroy the player
        GameManager.instance.SetSceneCam(true);//set scene camera back to true 
        GameManager.UnRegisterPlayer(transform.name);//remove player from the dictionary 
    }
}
