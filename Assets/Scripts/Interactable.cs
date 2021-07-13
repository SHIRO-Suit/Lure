using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEditor;
//using Knife.HDRPOutline.Core;


public enum State{ PickUpItem, InteractWith}

public class Interactable : MonoBehaviour
{
    
    public UnityEvent OnUsed;
    public bool isConsuming,singleUse,infiniteSupply;
    public KeyCode UseTakeKey = KeyCode.E;
    
    public string objectNeeded = "";
    
    public State InteractableState = State.PickUpItem;
    
    
    public void Use(){
        if(singleUse){
            gameObject.layer = 0; 
        }
              
        OnUsed.Invoke();
    }
    

}

#if (UNITY_EDITOR) 
[CustomEditor(typeof(Interactable))]
public class InteractableEditor : Editor {
    SerializedProperty OnUsedProp,isConsumingProp,ObjectNeededProp,infiniteSupplyProp,singleUseProp;

    private void OnEnable() {
        OnUsedProp = serializedObject.FindProperty("OnUsed");
        isConsumingProp = serializedObject.FindProperty("isConsuming");
        ObjectNeededProp = serializedObject.FindProperty("objectNeeded");
        infiniteSupplyProp = serializedObject.FindProperty("infiniteSupply");
        singleUseProp = serializedObject.FindProperty("singleUse");
    }
    public override void OnInspectorGUI() {
        Interactable Script = (Interactable)target;
        serializedObject.Update();
 
        EditorGUILayout.PropertyField(OnUsedProp);
         Script.InteractableState = (State) EditorGUILayout.EnumPopup("Interactable State",Script.InteractableState);
        
        EditorGUILayout.Space(); 
        if(Script.InteractableState == State.InteractWith){
            EditorGUILayout.PropertyField(isConsumingProp);
            EditorGUILayout.PropertyField(singleUseProp);
            Script.UseTakeKey = (KeyCode) EditorGUILayout.EnumPopup("Key to Use",Script.UseTakeKey);
            EditorGUILayout.Space(); 
            EditorGUILayout.PropertyField(ObjectNeededProp);
            
            EditorGUILayout.HelpBox("Leave blank if no object needed to interact with", MessageType.Info);
        }else{
            EditorGUILayout.PropertyField(infiniteSupplyProp);
            Script.UseTakeKey = (KeyCode) EditorGUILayout.EnumPopup("Key to take",Script.UseTakeKey);
        }
        
         GameObject go = Selection.activeGameObject;
         Outline oo = go.GetComponent<Outline>();
         if(oo == null){
             EditorGUILayout.HelpBox("OutlineObject needs to be added to GameObject", MessageType.Error);
         }

        serializedObject.ApplyModifiedProperties();
    }
}
#endif



