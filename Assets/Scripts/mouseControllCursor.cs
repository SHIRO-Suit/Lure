using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class mouseControllCursor : MonoBehaviour
{
    public Inputmaster controls;
    public RectTransform Circle, Indicator, CountDown;
    public Animator IndicatorAnimator;
    float CountDownProgress = 0;
    RectTransform Self;
    bool CoroutineRun = false;
    float ScreenScale;
    Canvas ScreenCanvas;
    Vector2 RandMov,currentMov = Vector2.up;
    public Animator CanvasAnimator;
    
    void Awake() {
            Self = GetComponent<RectTransform>();
            controls = new Inputmaster();     
            ScreenCanvas = GetComponentInParent<Canvas>();
            Debug.Log(ScreenScale);
            ResetGame();
    }
    public bool AnimIsResistPhase(){
        return CanvasAnimator.GetInteger("Step") == 2;
    }

    void FixedUpdate() {   
        ScreenScale = ScreenCanvas.scaleFactor;
        if(!CoroutineRun && AnimIsResistPhase()) StartCoroutine(DirChange());
        if(AnimIsResistPhase() && !Pause.IsPaused){
            transform.Translate((getMouse()*5f*fpsController.sensitivity + currentMov)*ScreenScale*3.5f);
            CountDownProgress += Time.fixedDeltaTime * 0.2f;
            CountDown.localScale = Vector3.Lerp(Vector3.zero, Vector3.one,CountDownProgress);
            
            if(Vector2.Distance(Self.position,Circle.position) > (Circle.rect.width+Self.rect.width)/2*ScreenScale){
                ResetGame();
            }
            if(CountDownProgress >= 1){
                Attraction.isEscaping = true;
                Choice.CurrentMonster = null;
                ResetGame(); 
            }
        }
        
    }

    IEnumerator DirChange(){
        CoroutineRun = true;
        while (AnimIsResistPhase()){
            while(Vector2.SignedAngle(RandMov,currentMov)<90 && Vector2.SignedAngle(RandMov,currentMov)>-90){
                RandMov = Random.insideUnitCircle.normalized;
            }
        Indicator.transform.rotation = Quaternion.Euler(0,0,Vector2.SignedAngle(Vector2.up,RandMov));
        yield return new WaitForSeconds(0.3f);
        IndicatorAnimator.SetTrigger("Indication");
        yield return new WaitForSeconds(0.2f);
        currentMov = RandMov;
        yield return new WaitForSeconds(0.5f);
        }
        CoroutineRun = false;
    }
    public void ResetGame(){
        Self.anchoredPosition = Vector3.zero;
        CountDownProgress=0;
    }
    public Vector2 getMouse(){
        return controls.player.Camera.ReadValue<Vector2>();
    }
    private void OnEnable() {
        controls.Enable();
    }
    private void OnDisable() {
        controls.Disable();
    }
}
