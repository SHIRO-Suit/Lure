using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class reloadScene : MonoBehaviour
{
    public void RestartGame() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        
    }
}
