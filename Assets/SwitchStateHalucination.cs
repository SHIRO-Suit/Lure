using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class SwitchStateHalucination : MonoBehaviour
{
    public UnityEvent OnHallucination;
    public UnityEvent OnRealLife;
    bool cachebool;
    public bool isHallucinating = true;
    
    void Update() {
        if(cachebool==isHallucinating) return;
        if(isHallucinating){
            OnHallucination.Invoke();

        }else{
            OnRealLife.Invoke();
        }
        cachebool = isHallucinating;
    }
    

}
