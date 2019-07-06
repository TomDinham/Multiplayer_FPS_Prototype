using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUI : MonoBehaviour
{
    //class for enabling and disableing the pause menu on key press
    [SerializeField]
   private GameObject pauseMenu;
    
    void Start()
    {
        PauseMenu.isOn = false; 
        
    }

    
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape)) //if the escape key is pressed then open or close the pause menu dependent of the boolean variable.
        {
            TogglePauseMenu();
        }
    }
   public void TogglePauseMenu()
    {
        pauseMenu.SetActive(!pauseMenu.activeSelf);
        PauseMenu.isOn = pauseMenu.activeSelf;
    }
}
