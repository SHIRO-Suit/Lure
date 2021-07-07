using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class fpsController : MonoBehaviour
{
    public Canvas QTECanvas;
    public static Camera ThisCamera;
    float Xrotate;
    public float speed = 3f;
    public Slider sensitivitySlider;

    
    public static float sensitivity = 0.1f;
    public Inputmaster controls;  
    Vector2 velocity,rotation;
    void Start()
    {
        sensitivitySlider.value = (sensitivity -0.2f) / 0.6f;
        ThisCamera = transform.GetComponentsInChildren<Camera>()[0];
        //Cursor.visible =false;
    }
    public void ChangeSensitivity(){
        sensitivity = 0.2f + 0.6f * sensitivitySlider.value;
    }
    void Awake() {
            controls = new Inputmaster();
            // controls.player.Movement.performed += ctx => Move(ctx.ReadValue<Vector2>());
            //controls.player.Camera.performed += ctx => PlayerCameraRotate(ctx.ReadValue<Vector2>());       
    }
    // public void PlayerCameraRotate(Vector2 delta){
    //     //transform.Rotate(Vector3.up * delta.x * 2 * Time.deltaTime);
    //     velocity = new Vector2(
    //         Mathf.MoveTowards(velocity.x,delta.x*sensitivity,1000*Time.deltaTime),
    //         Mathf.MoveTowards(velocity.x,delta.x*sensitivity,1000*Time.deltaTime)
    //     );
    //     rotation += velocity * Time.deltaTime;
    //     transform.rotation =  Quaternion.Euler(0,/*transform.rotation.eulerAngles.y +*/ rotation.x ,0);
    //     Xrotate =  Mathf.Min(90,Mathf.Max(-90, Xrotate + rotation.x));
    //     ThisCamera.transform.localRotation = Quaternion.Euler(-Xrotate ,0,0);
    //    // if(delta.magnitude > deltaCache.magnitude)
        
        
    //     //ThisCamera.transform.Rotate(new Vector3(-Xrotate ,0,0)* Time.deltaTime,Space.Self);   
    //     //if(ThisCamera.transform.rotation.x > )
    // }
    public void PlayerCameraRotate(Vector2 delta){
        //transform.Rotate(Vector3.up * delta.x * 2 * Time.deltaTime);
        
        transform.rotation =  Quaternion.Euler(0,transform.rotation.eulerAngles.y + delta.x * sensitivity,0);
        Xrotate =  Mathf.Min(90,Mathf.Max(-90, Xrotate + delta.y * sensitivity));
        ThisCamera.transform.localRotation = Quaternion.Euler(-Xrotate ,0,0);
       // if(delta.magnitude > deltaCache.magnitude)
        //deltaCache = delta.normalized;
        
        //ThisCamera.transform.Rotate(new Vector3(-Xrotate ,0,0)* Time.deltaTime,Space.Self);   
        //if(ThisCamera.transform.rotation.x > )
    }

    public Vector2 getMovement(){
        return controls.player.Movement.ReadValue<Vector2>();
    }
    public Vector2 getMouse(){
        return controls.player.Camera.ReadValue<Vector2>();
    }
    
    
    void Update() {
        transform.Translate(new Vector3(getMovement().x,0,getMovement().y) *speed * Time.deltaTime);
        PlayerCameraRotate(getMouse());
        // transform.Rotate(Vector3.up * getMouse().x * Time.fixedDeltaTime * sensitivity);
        // Xrotate = getMouse().y;
        // Xrotate = Mathf.Clamp(Xrotate, -90 , 90);
        // ThisCamera.transform.rotation = Quaternion.Euler(Xrotate ,ThisCamera.transform.rotation.y,0);
    }


    private void OnEnable() {
        controls.Enable();
        Xrotate =  ThisCamera ? ThisCamera.transform.localEulerAngles.x : 0f;
        Xrotate = (Xrotate > 180) ? -Xrotate + 360 : -Xrotate;
    }
    private void OnDisable() {
        controls.Disable();
        
    }
}
