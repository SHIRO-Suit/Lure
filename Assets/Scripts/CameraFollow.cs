using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    // Start is called before the first frame update
    
    GameObject player;
    void Start() {
        player = GameObject.Find("player");
    }
    void Update()
    {
        transform.rotation = Quaternion.Euler(0,player.transform.rotation.eulerAngles.y,0);
    }
}
