using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Script to control territories, manages the particles' shape and colour, manages changing colours on contact with ships
public class TerritoryController : MonoBehaviour {

    //simple tracker of ownership, 'r' = red, 'b' = blue
    private char currentOwner;
    private bool needsToChange;
    private Renderer rend;
    private GameController gameController;

    public ParticleSystem whiteRing, blueRing, redRing;
    private ParticleSystem currentRingInstance;

	//Instantiates the rings, sets up references
	void Start () {
        GameObject gameControllerObject = GameObject.FindWithTag("GameController");
        gameController = gameControllerObject.GetComponent<GameController>();
        rend = GetComponent<Renderer>();

        currentRingInstance = Instantiate(whiteRing, transform.position, Quaternion.Euler(-90,0,0));
	}
    //Manages when rings are flown into by ships, setup 
    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log(other.gameObject.ToString());
        if (other.gameObject.ToString() == "RedFighter (UnityEngine.GameObject)" && currentOwner != 'r')
        {
            currentOwner = 'r';
            needsToChange = true;
        }
        if (other.gameObject.ToString() == "BlueFighter (UnityEngine.GameObject)" && currentOwner != 'b')
        {
            currentOwner = 'b';
            needsToChange = true;
        }
    }

    //Changes colour by reinstantiating a new ring - this should probably be improved, there must be a way to change just colour of the particles.
    void changeColourIfTouched()
    {
        Destroy(currentRingInstance.gameObject);
            if (currentOwner == 'r')
            {
                //rend.material.color = Color.red;
                currentRingInstance = Instantiate(redRing, transform.position, Quaternion.Euler(-90, 0, 0));
            } else if (currentOwner == 'b')
            {
                //rend.material.color = Color.blue;
                currentRingInstance = Instantiate(blueRing, transform.position, Quaternion.Euler(-90, 0, 0));
            }
            needsToChange = false;
    }

    // Update waits for the need to change colour and does so - also adds score by calling methods in the gamecontroller (hence the reference)
    void Update () {
        if (needsToChange)
        {
            changeColourIfTouched();
        }
        if (currentOwner == 'r')
        {
            gameController.addScore(1, 0);
        }
        if (currentOwner == 'b')
        {
            gameController.addScore(0, 1);
        }
	}
}
