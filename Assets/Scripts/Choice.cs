using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Choice : MonoBehaviour
{
    public static monstreRaycast CurrentMonster;
    float percentage ;
    public RectTransform SuccessCircle;
    public static bool success = false;
    public Animator PhaseCanvas;
    public AudioSource UiSource;
    public AudioClip choiceDone;
    void Update(){
        if (CurrentMonster){
            if(CurrentMonster.isRaycasting){
                if(!Attraction.AntidoteInjected){
                    PhaseCanvas.SetInteger("Step",1);
                }
            }
            CurrentMonster.isRaycasting = false;

            if(Input.GetKeyDown("r")){
                PhaseCanvas.SetInteger("Step",2);
                UiSource.PlayOneShot(choiceDone);
            }else if (Input.GetKeyDown("g")){
                PhaseCanvas.SetInteger("Step",0);
                Attraction.GiveInSpeed = 2;
                UiSource.PlayOneShot(choiceDone);
            }
            if(Vector3.Distance(CurrentMonster.transform.position,transform.position) <2.5){ // annule tout quand on est trop pres du monstre
                PhaseCanvas.SetInteger("Step",0);
            }
            
        } 
        if(Attraction.isAttracted){ //death
     
            if(Vector3.Distance(CurrentMonster.transform.position,transform.position)<1.5){
                PhaseCanvas.SetInteger("Step",3);
                GetComponent<fpsController>().enabled =false;
                Cursor.visible = true;
            }
        }
        if(Attraction.isEscaping  ){
            PhaseCanvas.SetInteger("Step",0);
            //Attraction.isEscaping = true;
        }
        percentage = SuccessCircle.localScale.x;
        
    }

  
   
}
