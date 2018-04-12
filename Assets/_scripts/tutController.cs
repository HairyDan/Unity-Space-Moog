using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tutController : MonoBehaviour {

    int rScoreInt, bScoreInt, bhealth, rhealth;
    public GUIText redScore, blueScore, redHealthDisplay, blueHealthDisplay;

    public GameObject redShip, blueShip, redEnemy, blueEnemy;

    private ShipController redController;
    private BlueController blueController;

    public GameObject destructWall;

    public AudioSource shipBoom;
    public GameObject redExplode, blueExplode;

    private bool redEnemyDead, blueEnemyDead;

    public GameObject welcomepanel, territoriespanel, shootingpanel, finishpanel;
    private bool welcomefreeze, territoryfreeze;
    private bool blueForward, blueTurn, redForward, redTurn;
    private bool norefreeze, noshootrefreeze;


	void Start () {
        rScoreInt = 0;
        bScoreInt = 0;
        bhealth = 3;
        rhealth = 3;

        redController = redShip.GetComponent<ShipController>();
        blueController = blueShip.GetComponent<BlueController>();

        redEnemyDead = false;
        blueEnemyDead = false;

       // redController.currentlyDestroyed = true;
       // blueController.currentlyDestroyed = true;

        redScore.fontSize = -5;
        blueScore.fontSize = -5;

        redHealthDisplay.fontSize = -5;
        blueHealthDisplay.fontSize = -5;

        welcomefreeze = true;
        territoryfreeze = norefreeze = noshootrefreeze = false;
        blueForward = blueTurn = redForward = redTurn = false;

        territoriespanel.SetActive(false);

        shootingpanel.SetActive(false);

        finishpanel.SetActive(false);

    }

    public void addScore(int redAdd, int blueAdd)
    {
        rScoreInt += redAdd;
        bScoreInt += blueAdd;
    }

	void Update () {
        
        redScore.text = "Score: \n" + rScoreInt.ToString();
        blueScore.text = "Score: \n" + bScoreInt.ToString();
        redHealthDisplay.text =  "Dummy HP:\n"+rhealth.ToString();
        blueHealthDisplay.text = "Dummy HP:\n"+bhealth.ToString();

        if (redEnemyDead && blueEnemyDead)
        {
            finishpanel.SetActive(true);
            if (Input.GetButtonDown("Submit"))
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene("mainmenu");
            }
        }

        if(rScoreInt >= 10 && bScoreInt >= 10 && !noshootrefreeze)
        {
            redScore.fontSize = 39;
            blueScore.fontSize = 39;
            Destroy(destructWall);

            shootingpanel.SetActive(true);

            redController.currentlyDestroyed = true;
            blueController.currentlyDestroyed = true;

            if (Input.GetButtonDown("BlueFire") || Input.GetButtonDown("RedFire"))
            {
                shootingpanel.SetActive(false);
                noshootrefreeze = true;
                redController.currentlyDestroyed = false;
                blueController.currentlyDestroyed = false;

                redHealthDisplay.fontSize = 39;
                blueHealthDisplay.fontSize = 39;

            }
        }
        
            if (!blueForward && Input.GetButtonDown("VerticalBlue"))
            {
                blueForward = true;
            }
            if (!blueTurn && Input.GetButtonDown("HorizontalBlue"))
            {
                blueTurn = true;
            }
            if (!redForward && Input.GetButtonDown("Vertical"))
            {
                redForward = true;
            }
            if (!redTurn && Input.GetButtonDown("Horizontal"))
            {
                redTurn = true;
            }

        if (welcomefreeze)
        {
            if (welcomefreeze && (blueForward || blueTurn || redForward || redTurn))
            {
                welcomefreeze = false;
                Destroy(welcomepanel);
            }
        }
        if (!welcomefreeze)
        {
            if(blueForward && redForward && blueTurn && redTurn && !norefreeze)
            {
                redController.currentlyDestroyed = true;
                blueController.currentlyDestroyed = true;

                territoriespanel.SetActive(true);
                territoryfreeze = true;
                norefreeze = true;
            }

            if (Input.GetButtonDown("BlueFire"))
            {
                territoriespanel.SetActive(false);
                
            }
            if (territoryfreeze && Input.GetButtonUp("BlueFire"))
            {
                redController.currentlyDestroyed = false;
                blueController.currentlyDestroyed = false;
                territoryfreeze = false;
            }
        }

    }

    public void damageDealt(int bDamage, int rDamage)
    {
        rhealth -= rDamage;
        bhealth -= bDamage;
        if (rhealth <= 0 && redEnemyDead == false)
        {
            shipDestroyed(true);
        }
        if (bhealth <= 0 && blueEnemyDead == false)
        {
            shipDestroyed(false);
        }
    }

    public void shipDestroyed(bool isRed)
    {
        if (isRed)
        {
            AudioSource boomSound = Instantiate(shipBoom);
            Destroy(boomSound.gameObject, 2.0f);
            GameObject boom = Instantiate(redExplode, redEnemy.transform.position, Quaternion.Euler(0, 0, 0));
            Destroy(boom, 1);
            Destroy(redEnemy);
            redEnemyDead = true;
        }

        else
        {
            AudioSource boomSound = Instantiate(shipBoom);
            Destroy(boomSound.gameObject, 2.0f);
            GameObject boom = Instantiate(blueExplode, blueEnemy.transform.position, Quaternion.Euler(0, 0, 0));
            Destroy(boom, 1);
            Destroy(blueEnemy);
            blueEnemyDead = true;
        }

    }

}
