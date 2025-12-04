using UnityEngine;
using System.Collections;

public class PauseP : MonoBehaviour {

    bool  paused = false;
    private GameOverScriptP PauseMenu;

    void Start ()
    {
       
        PauseMenu = GetComponent<GameOverScriptP> ();
       
    }
   
    void  Update (){

        if(Input.GetKeyDown (KeyCode.P))
        if (Input.GetKeyDown(KeyCode.Escape))
        {

            if(!paused){

                Time.timeScale = 0;
                paused=true;
                PauseMenu.enabled = true;


            }else{

                Time.timeScale = 1;
                paused=false;
                PauseMenu.enabled = false;
               
            }
        }

    }

}