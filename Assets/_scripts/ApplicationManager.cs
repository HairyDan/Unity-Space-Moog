using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Simple script to change scene from main menu. 
public class ApplicationManager : MonoBehaviour {
	
    public void changeScene(string name)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(name);
    }

}
