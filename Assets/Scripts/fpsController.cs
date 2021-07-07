using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class fpsController : MonoBehaviour
{
    
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
    }
    public void ChangeSensitivity(){
        sensitivity = 0.2f + 0.6f * sensitivitySlider.value; // valeur du slider entre 0.2 et 0.6
    }
    void Awake() {
            controls = new Inputmaster();     
    }

    public void PlayerCameraRotate(Vector2 delta){
        transform.rotation =  Quaternion.Euler(0,transform.rotation.eulerAngles.y + delta.x * sensitivity,0);
        Xrotate =  Mathf.Min(90,Mathf.Max(-90, Xrotate + delta.y * sensitivity)); //blocage de la rotation en X a 180deg vers l'avant
        ThisCamera.transform.localRotation = Quaternion.Euler(-Xrotate ,0,0);
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
    }


    private void OnEnable() {
        controls.Enable();
        Xrotate =  ThisCamera ? ThisCamera.transform.localEulerAngles.x : 0f; // evite les mouvements de camera et remise a 0 de la rotation quand on pause. 
        Xrotate = (Xrotate > 180) ? -Xrotate + 360 : -Xrotate;
    }
    private void OnDisable() {
        controls.Disable();
        
    }
}
