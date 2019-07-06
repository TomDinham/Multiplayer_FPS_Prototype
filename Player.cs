using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Rigidbody))]//must have a rigidbody component on the object 
public class Player : MonoBehaviour
{
    [SerializeField]
    private Camera Cam;
    private Vector3 movement = Vector3.zero;
    private Vector3 Rotation = Vector3.zero;
    private float CamRotationX = 0f;
    private float currentCamRotationX = 0f;

    [SerializeField]
    private float CameraRotLimit = 80f;
    private Rigidbody rb;


    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {

        MovePlayer();
        RotatePlayer();
    }
    public void Mover(Vector3 move) // set the movement amount 
    {
        movement = move;
    }
    public void Rotator(Vector3 rot)//set the rotation amount for the player 
    {
        Rotation = rot;
    }
    public void RotateCam(float CamRotX)//rotate the camera
    {
        CamRotationX = CamRotX;
    }
    private void MovePlayer() // move the player
    {
        if(movement != Vector3.zero)
        {
            rb.MovePosition(rb.position + movement * Time.fixedDeltaTime);
        }
    }
    private void RotatePlayer()//rotate the player 
    {
        if(Rotation!= Vector3.zero)
        {
            rb.MoveRotation(rb.rotation * Quaternion.Euler(Rotation));
        }
        if(Cam!=null) // lock the player rotation so the players can not look in a 360 degree motion
        {
            currentCamRotationX -= CamRotationX;
            currentCamRotationX = Mathf.Clamp(currentCamRotationX, -CameraRotLimit, CameraRotLimit);

            Cam.transform.localEulerAngles = new Vector3(currentCamRotationX, 0, 0);
        }
    }
}
