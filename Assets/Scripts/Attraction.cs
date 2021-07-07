using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attraction : MonoBehaviour
{   Vector3 MonsterPosition = Vector3.zero;
    Vector3 direction;
    
    public AudioSource Breathing;
    Quaternion toRotation, CamStartRot, playerStartRot;
    public static bool isAttracted = false, isEscaping = false;
    float smoothing,startTime;
    public static int GiveInSpeed = 1;
    fpsController Self;
    public static bool AntidoteInjected;
    public static float monsterDistRay;
    
    void Awake() {
        isAttracted = false;
        isEscaping = false;    
    }
    void Start(){
        Self = GetComponent<fpsController>();
    }
    public void injectAntidote(){
        AntidoteInjected = true;
        foreach(SwitchStateHalucination obj in GameObject.FindObjectsOfType<SwitchStateHalucination>()){
            obj.OnRealLife.Invoke(); // rends tout les monstres visibles normalement et bruyants.
        }
        
    }
    void FixedUpdate()
    {
        Debug.Log("La");
        Debug.Log(isAttracted);
        if(isAttracted){
            if(MonsterPosition == Vector3.zero){ // called once
                playerStartRot = transform.rotation;
                if(!AntidoteInjected){
                    Self.enabled = false;
                }
                
                GetComponent<Pause>().enabled = false;
                MonsterPosition = Choice.CurrentMonster.transform.position;
                direction = MonsterPosition - transform.position ;
                direction.y = 0;
            toRotation = Quaternion.LookRotation(direction,Vector3.up);
            startTime = Time.time;
            CamStartRot = fpsController.ThisCamera.transform.localRotation;
            
            }
            
            smoothing = Time.time - startTime;
            if( AntidoteInjected){
                direction = MonsterPosition - transform.position ;
                direction.y = 0;
                toRotation = Quaternion.LookRotation(direction,Vector3.up);
                if(Self.getMouse()!=Vector2.zero) smoothing =0;
                transform.rotation = Quaternion.Slerp(transform.rotation, toRotation,smoothing*.01f );
                transform.Translate(direction.normalized * (monsterDistRay/direction.magnitude) * GiveInSpeed * Time.fixedDeltaTime * .6f, Space.World);
            }else{
                transform.rotation = Quaternion.Slerp(playerStartRot, toRotation,smoothing );

            }
            fpsController.ThisCamera.transform.localRotation = Quaternion.Slerp(CamStartRot,Quaternion.identity, smoothing);
            
            if(smoothing > 0.8f){
                transform.Translate(transform.forward* GiveInSpeed * Time.fixedDeltaTime * .6f, Space.World);
            }
        }
        
        if(isEscaping){
            if(isAttracted){ //Called once
                //MonsterPosition = Vector3.zero;
                Breathing.Play();
                isAttracted=false;
                toRotation = Quaternion.LookRotation(-direction,Vector3.up);
                playerStartRot = transform.rotation;
                startTime = Time.time;
            }
            smoothing = (Time.time) - startTime;
            transform.rotation = Quaternion.Slerp(playerStartRot, toRotation,smoothing * 4 );
                if(Vector3.Distance(MonsterPosition,transform.position)<=monsterDistRay+1){
                    transform.Translate(-direction.normalized * 5 * Time.fixedDeltaTime , Space.World);
                }else{
                    isEscaping = false;
                    StartCoroutine(BreathingCooldown());
                    Self.enabled = true;
                    GetComponent<Pause>().enabled = true;
                }

        }
        if(!isEscaping && !isAttracted) MonsterPosition = Vector3.zero;
    }

    IEnumerator BreathingCooldown(){
        yield return new WaitForSeconds(2);
        Breathing.Stop();
    }

}
