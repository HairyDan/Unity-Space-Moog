using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Controller for game states, UI values and ship health.
public class GameController : MonoBehaviour {

    public int rScoreInt, bScoreInt, bhealth, rhealth;
    public GUIText redScore, blueScore, redHealthDisplay, blueHealthDisplay, roundTimerDisplay;

    public GameObject redShip, blueShip, startPanel, gameoverpanel;
    public UnityEngine.UI.Text gameoverText;

    private ShipController redController;
    private BlueController blueController;

    private float roundTimer;
    private bool gameover;

	// Initialising healths, scores and the timer
	void Start () {
        rScoreInt = 0;
        bScoreInt = 0;
        bhealth = 3;
        rhealth = 3;

        redScore.text = "Score: \n" + rScoreInt.ToString();
        blueScore.text = "Score: \n" + bScoreInt.ToString();
        redHealthDisplay.text = "HP: " + rhealth.ToString();
        blueHealthDisplay.text = "HP: " + bhealth.ToString();

        roundTimer = 5;
        roundTimerDisplay.text = "Time Remaining: " + roundTimer.ToString();

        redController = redShip.GetComponent<ShipController>();
        blueController = blueShip.GetComponent<BlueController>();
        gameover = false;
        gameoverpanel.SetActive(false);
    }

    //Simple method called in the territorycontrollers to add to score
    public void addScore(int redAdd, int blueAdd)
    {
        rScoreInt += redAdd;
        bScoreInt += blueAdd;
    }
	
	// Update used mostly for the pregame/postgame states, as well as updating the UI
	void Update () {

        if (startPanel.activeSelf)
        {
            if (Input.anyKeyDown)
            {
                startPanel.SetActive(false);
            }
        }
        if (!startPanel.activeSelf && !gameover)
        {
            roundTimer -= Time.deltaTime;
            roundTimerDisplay.text = "Time Remaining: " + Mathf.Round(roundTimer).ToString();
            if (roundTimer < 0)
            {
                RoundOver();
            }


            redScore.text = "Score: \n" + rScoreInt.ToString();
            blueScore.text = "Score: \n" + bScoreInt.ToString();
            redHealthDisplay.text = "HP: " + rhealth.ToString();
            blueHealthDisplay.text = "HP: " + bhealth.ToString();
        }
        if (gameover)
        {
            if (Input.GetButtonDown("Submit"))
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene("mainmenu");
            }
        }
    }

    //Enters game-over state, freezes players and sets up final screen
    void RoundOver()
    {
        gameover = true;
        redController.currentlyDestroyed = true;
        blueController.currentlyDestroyed = true;

        string winner;
        string winnerpoints;
        string loser;
        string loserpoints;
        string finalString;
        if (rScoreInt > bScoreInt)
        {
            winner = "Red";
            winnerpoints = rScoreInt.ToString();
            loser = "Blue";
            loserpoints = bScoreInt.ToString();

            finalString = "The Winner is " +winner +" with "+ winnerpoints + " points! \n  Hard luck " +loser+" \n Press Enter to return to the main menu.";

        }
        else if (rScoreInt < bScoreInt){
            winnerpoints = bScoreInt.ToString();
            winner = "Blue";
            loser = "Red";
            loserpoints = rScoreInt.ToString();

            finalString = "The Winner is " +winner +" with "+ winnerpoints + " points! \n Hard luck " +loser+" \n Press Enter to return to the main menu.";
        } else if (rScoreInt == bScoreInt){
            finalString = "A Draw? That was unlikely... \n Press Enter to return to the main menu.";
        } else {
            finalString = "Game ended on a bad note... \n Press Enter to return to the main menu.";
        }

        gameoverText.text =  finalString;
        gameoverpanel.SetActive(true);


    }
    //Manages damage being dealt to ships, provides a link between ships bullets and the UI
    public void damageDealt(int rDamage, int bDamage)
    {
        rhealth -= rDamage;
        bhealth -= bDamage;
        if (rhealth <= 0)
        {
            redController.shipDestroyed();
            //true for the isRed argument
            respawnShip(true);
            rhealth = 3;
        }
        if (bhealth <= 0)
        {
            blueController.shipDestroyed();
            //false for the isRed argument
            respawnShip(false);
            bhealth = 3;
        }
    }

    //The respawnShip method exists just to thread the respawn timers
    public void respawnShip(bool isRed)
    {
        if (isRed)
        {
            StartCoroutine(respawnTimer(isRed));
        }
        else
        {
            StartCoroutine(respawnTimer(isRed));
        }
    }

    //Waits 3 seconds before respawning a dead ship
    private IEnumerator respawnTimer(bool red)
    {
        yield return new WaitForSeconds(3);
        if (red)
        {
            redController.shipRespawn();
        } else
        {
            blueController.shipRespawn();
        }
    }

}
