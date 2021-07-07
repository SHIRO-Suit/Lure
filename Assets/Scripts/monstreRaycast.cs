using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class monstreRaycast : MonoBehaviour
{
    public bool isRaycasting = true;
    public float Raydistance;
    Choice Player;

    Animator GlitchEffect;
    AudioSource Tension;
    void Start()
    {
        Tension = GetComponent<AudioSource>();
        GlitchEffect = GetComponent<Animator>();
        Player = FindObjectOfType<Choice>();  
    }

    private void OnDrawGizmosSelected() {
        Gizmos.DrawWireSphere(transform.position,Raydistance);
    }
    void Update()
    {
        
        isRaycasting = (!Choice.CurrentMonster && !Attraction.isEscaping && !Attraction.isAttracted) || Attraction.AntidoteInjected;
        if(isRaycasting){
       
            if(Physics.OverlapSphere(transform.position, Raydistance,1<<6).Length >0){
            // if(Physics.RaycastAll(transform.position, transform.forward,Raydistance)[0].transform.gameObject.name == "player"){
                if (Player.PhaseCanvas.GetInteger("Step") == 0){
                    Choice.CurrentMonster = transform.GetComponent<monstreRaycast>(); // injecte le script pour que QTE.cs puisse Desactiver isRaucasting; 
                    Attraction.monsterDistRay = Raydistance;
                    if(!Attraction.AntidoteInjected){
                        
                        GlitchEffect.Play("changeState");
                    }else{
                        GlitchEffect.Play("Antidote");
                        
                    }
                    Debug.Log("ici");
                    Attraction.isAttracted = true;
                    Debug.Log("iciiiii"+ Attraction.isAttracted);
                    if(!Tension.isPlaying){
                        Tension.Play();

                    }
                    
                }                

            // }
            }else if(Attraction.AntidoteInjected){
               // Attraction.isAttracted = false;
            }else{
                GlitchEffect.Play("idle");
            }
        }
    }
}
