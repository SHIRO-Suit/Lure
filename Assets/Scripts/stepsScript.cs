using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class stepsScript : MonoBehaviour
{
    public MonsterAttraction playerMAScript;
    Vector3 pastPos;
    public AudioSource feet;
    public AudioClip[] clips;
    float movement;
    //float timeElapsed=1f;
    AudioClip prevClip;


    void Start() {

        StartCoroutine(fixedAudioUpdate());    
    }
    IEnumerator fixedAudioUpdate(){
        yield return new WaitForSeconds(0.1f);
        updateSim();
        StartCoroutine(fixedAudioUpdate());
    }
    void updateSim()
    {
        if(transform.position != pastPos) movement += (transform.position - pastPos).sqrMagnitude ;//* Time.fixedDeltaTime;
        if(playerMAScript.GetAnimState()!=0 ){
            if(playerMAScript.antidoteInjected){
                if(movement>.6f){
            PlayRandom();
            ResetMov();
            }
            }else{
            if(movement>.03f){
            PlayRandom();
            ResetMov();
            }
            }}
        else if(playerMAScript.isEscaping){
            if(movement >.45f){
                PlayRandom();
                ResetMov();
            }
        }else{
            if(movement >.6f){
                PlayRandom();
                ResetMov();
            }
        }
        
        pastPos = transform.position;
    }

    

    void PlayRandom(){
        while(feet.clip == prevClip){
            feet.clip = clips[Random.Range(0,clips.Length)]; // empeche d'avoir 2 fois le même son
        }
        prevClip = feet.clip;
        feet.Play();
    }
    void ResetMov(){
        movement = 0;
    }
}
