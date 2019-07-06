using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
[RequireComponent(typeof(Rigidbody))]
public class PlayerManager : NetworkBehaviour
{

    [SerializeField]
    private int maxHealth = 100;
    [SerializeField]
    private Rigidbody rb;
    [SyncVar]//sync variables will sync their values across all connected clients when the value is changed 
    private int currHealth;
    public bool isDead
    {
        get { return _isDead; }
        protected set { _isDead = value; }
    }

    private bool _isDead = false;

    [SerializeField]
    private Behaviour[] disableOnDeath;
    [SerializeField]
    private GameObject[] disableGameObjectsOnDeath;
    private bool[] wasEnabled;
    
    public void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    public void Setup()
    {
        wasEnabled = new bool[disableOnDeath.Length];//set was enabled array to the length of the disabled on death array 
        for (int i = 0; i < wasEnabled.Length; i++)
        {
            wasEnabled[i] = disableOnDeath[i].enabled;// set the was enabled values to the disable of death values 
        }

        InitilisePlayer();
    }
    private void Update()
    {
        if (!isLocalPlayer)//if this is not the local player then retun 
            return;

        if (Input.GetKeyDown(KeyCode.K))
        {
            RpcTakeDamage(9999);
        }
    }
    [ClientRpc] // rpc call for dealing damage 
    public void RpcTakeDamage(int damage)
    {
        if (isDead)
            return;
        currHealth -= damage;

        Debug.Log(transform.name + " Health is: " + currHealth);
        if(currHealth <=0)
        {
            Die();
        }
    }
    private IEnumerator Respawn() // respawning the player at a positon recognised by the network manager
    {
        yield return new WaitForSeconds(GameManager.instance.MatchSettings.RespawnTimer);
        InitilisePlayer();
        Transform spawnPoint = NetworkManager.singleton.GetStartPosition();
        transform.position = spawnPoint.position;
        transform.rotation = spawnPoint.rotation;

        Debug.Log(transform.name + " respawned");
    }
    private void Die()
    {
        //set the objects and behaviors  to false when the player dies to disable the correct components
        isDead = true;
        for (int i = 0; i < disableOnDeath.Length; i++)
        {

            disableOnDeath[i].enabled = false;
        }
        for (int i = 0; i < disableOnDeath.Length; i++)
        {

            disableGameObjectsOnDeath[i].SetActive(false);
        }
        Collider col = GetComponent<Collider>();
        if (col != null)//set the collider to fales and disable gravity on the player
        {
            col.enabled = false;
            rb.useGravity = false;
        }
        if(isLocalPlayer)// set the scene camera to true so the player is not looking at nothing
        {
            GameManager.instance.SetSceneCam(true);
        }
        Debug.Log(transform.name + " Is Dead!");
        //respawn the player
        StartCoroutine(Respawn());

    }
    public void InitilisePlayer()
    {
 
        //set health back to max
        isDead = false;
        currHealth = maxHealth;
        //set the components that were disabled on death back to true 
        for (int i = 0; i < disableOnDeath.Length; i++)
        {
            disableOnDeath[i].enabled = wasEnabled[i];
        }
        //enable the objects that may have been disabled on death
        for (int i = 0; i < disableOnDeath.Length; i++)
        {

            disableGameObjectsOnDeath[i].SetActive(true);
        }
        //enable the collider and gravity for the player
        Collider col = GetComponent<Collider>();
        if(col != null)
        {
            col.enabled = true;
            rb.useGravity = true;
        }
        //retun the scene camera back to false so the player returns to their perspective
        if (isLocalPlayer)
        {
            GameManager.instance.SetSceneCam(false);
        }
    }

}
