using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pause : MonoBehaviour
{
    public static bool IsPaused = true;
    public GameObject PauseMenu;
    public fpsController FPSPlayer;
    public mouseControllCursor minigame;
   
    void Start() {
        //Time.timeScale = 0; 
        FPSPlayer = GetComponent<fpsController>();
        Resume();//REgle un probleme apparu pendant la nuit WTF...
        Stop();
        
    }
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))  {
            if(IsPaused){
                Resume();
            }else{
                Stop();
            }
        }  
    }
    public void Resume(){
        Cursor.visible = false;
        IsPaused = false;
        Time.timeScale = 1;
        FPSPlayer.enabled = true; 
        PauseMenu.SetActive(false);
    }
    public void Stop(){
        Cursor.visible = true;
        IsPaused = true;
        Time.timeScale = 0;
        FPSPlayer.enabled = false; 
        PauseMenu.SetActive(true);
    }
}
