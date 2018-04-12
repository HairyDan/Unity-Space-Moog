using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//TerritoryController set up for the tutorial
public class tutTerritoryController : MonoBehaviour
{

    private char currentOwner;
    private bool needsToChange;
    private Renderer rend;
    private tutController gameController;

    public ParticleSystem whiteRing, blueRing, redRing;
    private ParticleSystem currentRingInstance;

    // Use this for initialization
    void Start()
    {
        GameObject gameControllerObject = GameObject.FindWithTag("GameController");
        gameController = gameControllerObject.GetComponent<tutController>();
        rend = GetComponent<Renderer>();

        currentRingInstance = Instantiate(whiteRing, transform.position, Quaternion.Euler(-90, 0, 0));
    }

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

    void changeColourIfTouched()
    {
        Destroy(currentRingInstance.gameObject);
        if (currentOwner == 'r')
        {
            //rend.material.color = Color.red;
            currentRingInstance = Instantiate(redRing, transform.position, Quaternion.Euler(-90, 0, 0));
        }
        else if (currentOwner == 'b')
        {
            //rend.material.color = Color.blue;
            currentRingInstance = Instantiate(blueRing, transform.position, Quaternion.Euler(-90, 0, 0));
        }
        needsToChange = false;
    }

    // Update is called once per frame
    void Update()
    {
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
