using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
[RequireComponent(typeof(AudioSource))] // must have an audio source on the object to complile 
public class PlayerShooting : NetworkBehaviour
{
    private const string PLAYER_TAG = "Player";

    [SerializeField]
    private Camera cam;
    [SerializeField]
    private LayerMask Mask;
    [SerializeField]
    private string weaponLayerName = "Weapon";
    [SerializeField]
    private PlayerWeapon PWeapon;
    [SerializeField]
    private GameObject weaponGFX;
    public AudioClip GunSFX;
    [SerializeField]
    private AudioSource AS;

    private void Start()
    {
        if (cam == null)
        {
            Debug.Log("PlayerShooting: No cam found!");
            this.enabled = false;
        }
        AS = GetComponent<AudioSource>();
        weaponGFX.layer = LayerMask.NameToLayer(weaponLayerName);
    }
    private void Update()
    {
        if (PauseMenu.isOn)//if pause menu is enabled then return so the player cannot fire
            return;
        if (Input.GetButtonDown("Fire1"))//if the mousebutton is pressed
        {
            Shooting();
        }
    }
    [Client]
    void Shooting()
    {
        CmdPlayShotAudio();//send to the audio shot command
        RaycastHit hit;
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, PWeapon.range, Mask)) // send out a raycast from the cameras position foward to the distance of the weapon range, and return the information into the hit value
        {
            if (hit.collider.tag == PLAYER_TAG) // if the objects tag matches the players tag send to the command dor
            {
                CmdPlayerIsShot(hit.collider.name, PWeapon.damage);//send to the command for dealing damage to the play

            }
        }
    }
    [Command]//command functions will run only on the server, this function will call a client RPC call for playing gun shot audio
    void CmdPlayShotAudio()
    {
        RpcPlayerShotAudio();
    }
    [ClientRpc]//Client RPC calls run on all connected clients on the server, this one will play the audio shot for the client
    void RpcPlayerShotAudio()
    {
        AS.PlayOneShot(GunSFX);
    }
    [Command]//command function for dealing damage to the player
    void CmdPlayerIsShot(string ID, int damage)
    {
        Debug.Log(ID + " Has Been Shot");

       PlayerManager player =  GameManager.GetPlayer(ID);

        player.RpcTakeDamage(damage);
    }
}
