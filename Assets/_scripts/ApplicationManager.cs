using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplicationManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
    public void changeScene(string name)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(name);
    }

	// Update is called once per frame
	void Update () {
		
	}
}
