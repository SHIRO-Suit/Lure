using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Knife.HDRPOutline.Core;
using TMPro;
using UnityEngine.Rendering;

public class Inventory : MonoBehaviour
{   
    public static Dictionary<string,int> Objects = new Dictionary<string, int>();
    public Animator /*AttractionAnimator,*/ MessageAnimator;
    public TextMeshProUGUI MessageText,UseObjectText;
    public Collider[] ObjectsFound;
    OutlineObject objectOutline;
    public Volume postProcessVolume;
    public HDRPOutline outlineShader;     
    Interactable objInteractable;
    void Start() {
        postProcessVolume.profile.TryGet<HDRPOutline>(out outlineShader);    
    }
    
    void Update()
    {   
        ObjectsFound = Physics.OverlapSphere(transform.position,2f,1<<3);
        if(ObjectsFound.Length==0){
            UseObjectText.text = "";
            objInteractable = null;
            if(objectOutline) ActivateOutline(false);
            ObjectNeeded(false);
        }
        
        foreach(Collider Object in ObjectsFound){
            SetObjValues(Object); // ne se pose pas de questions car ils sont dans un layer

            if(objInteractable.InteractableState == State.PickUpItem)
            {
                ActivateOutline(true);            
                UseObjectText.text = "["+objInteractable.UseTakeKey +"] : Take <u>" + Object.transform.name + "</u>";
                if(Input.GetKeyDown(objInteractable.UseTakeKey)){
                    InventoryAdd(Object.transform.name);
                    if(!objInteractable.infiniteSupply){
                        Object.gameObject.SetActive(false);
                    }
                }
            }
            else
            {
                if( objInteractable.objectNeeded == ""){
                    UseObjectText.text = "["+objInteractable.UseTakeKey +"] : Use <u>" + Object.transform.name+ "</u>";
                    ActivateOutline(true);
                    if(Input.GetKeyDown(objInteractable.UseTakeKey)){
                        objInteractable.Use();
                    }
                }else{
                    if(Objects.ContainsKey(objInteractable.objectNeeded)){
                        UseObjectText.text = "["+objInteractable.UseTakeKey +"] : Use <u>" + objInteractable.objectNeeded +"</u> on <u>"+ Object.transform.name + "</u>";
                        ActivateOutline(true);
                        if(Input.GetKeyDown(objInteractable.UseTakeKey)){
                            objInteractable.Use();
                            if(objInteractable.isConsuming){
                                InventorySubstract(objInteractable.objectNeeded);
                            }
                        }
                    }
                    else
                    {
                        ObjectNeeded(true);
                        UseObjectText.text = "<!> Requires <u>" + objInteractable.objectNeeded + "</u>";
                    }   
                }
            }  
        }    
    }
    void SetObjValues(Collider Object){
        objInteractable = objInteractable ?? Object.GetComponent<Interactable>();
        objectOutline= Object.GetComponent<OutlineObject>() ?? Object.GetComponentInChildren<OutlineObject>();
    }
    void InventorySubstract(string objName){
        if(Objects[objName]>1){
            Objects[objName] -=1;
        }else{
            Objects.Remove(objName);
        }
    }
    void InventoryAdd(string objName){
        if(Objects.ContainsKey(objName)){
            Objects[objName] +=1;
        }else{
            Objects.Add(objName,1);
        }
        MessageDisplay(objName);
    }
    void ActivateOutline(bool activation){
        objectOutline.Color = activation? Color.white : Color.clear;
    }
    void MessageDisplay(string objName){
        MessageText.text = "[+1] " + objName;
        MessageAnimator.SetTrigger("TextChanged");
    }
    void ObjectNeeded(bool isNeeded){
        if(isNeeded){
            outlineShader.patternTexture.overrideState = true;
            objectOutline.Color = Color.red;
        }else{
            outlineShader.patternTexture.overrideState = false;
        }  
    }
}

    

