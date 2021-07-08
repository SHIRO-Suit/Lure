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
    float ScreenScale;
    public MonsterAttraction playerMonsterAttr;

    Canvas ScreenCanvas;
    Vector2 RandMov,currentMov = Vector2.up,fixedMov;
    public Animator CanvasAnimator;
    Rigidbody2D CursorRB;
    
    void Awake() {
        
            Self = GetComponent<RectTransform>();
            controls = new Inputmaster();     
            ScreenCanvas = GetComponentInParent<Canvas>();
            ResetGame();
            CursorRB = GetComponent<Rigidbody2D>();
    }
    public bool AnimIsResistPhase(){
        return CanvasAnimator.GetInteger("Step") == 2;
    }

    void FixedUpdate() {   
        ScreenScale = ScreenCanvas.scaleFactor; // adapte la vitesse dans l'ui par rapport a la taille de l'ecran afin que le mouvement ne soit pas plus lent en haute resolution
        if(AnimIsResistPhase() && !Pause.IsPaused){

            CursorRB.MovePosition(CursorRB.position + (getMouse() +currentMov)*ScreenScale*7 );
            CountDownProgress += Time.fixedDeltaTime * 0.2f;
            CountDown.localScale = Vector3.Lerp(Vector3.zero, Vector3.one,CountDownProgress);
            
            if(Vector2.Distance(Self.position,Circle.position) > (Circle.rect.width+Self.rect.width)/2*ScreenScale){
                ResetGame();
            }
            if(CountDownProgress >= 1){
                playerMonsterAttr.Escape();
                ResetGame(); 
            }
        }
        
    }

    public IEnumerator DirChange(){
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
    }
    public void ResetGame(){
        Self.anchoredPosition = Vector3.zero;
        CountDownProgress=0;
    }
    public Vector2 getMouse(){
        return controls.player.Camera.ReadValue<Vector2>() * fpsController.sensitivity;
    }
    private void OnEnable() {
        controls.Enable();
    }
    private void OnDisable() {
        controls.Disable();
    }
}
