using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

    public int rScoreInt, bScoreInt, bhealth, rhealth;
    public GUIText redScore, blueScore, redHealthDisplay, blueHealthDisplay, roundTimerDisplay;

    public GameObject redShip, blueShip, startPanel, gameoverpanel;
    public UnityEngine.UI.Text gameoverText;

    private ShipController redController;
    private BlueController blueController;

    private float roundTimer;
    private bool gameover;

	// Use this for initialization
	void Start () {
        rScoreInt = 0;
        bScoreInt = 0;
        bhealth = 3;
        rhealth = 3;

        redScore.text = "Score: \n" + rScoreInt.ToString();
        blueScore.text = "Score: \n" + bScoreInt.ToString();
        redHealthDisplay.text = "HP: " + rhealth.ToString();
        blueHealthDisplay.text = "HP: " + bhealth.ToString();

        roundTimer = 60;
        roundTimerDisplay.text = "Time Remaining: " + roundTimer.ToString();

        redController = redShip.GetComponent<ShipController>();
        blueController = blueShip.GetComponent<BlueController>();
        gameover = false;
        gameoverpanel.SetActive(false);
    }

    public void addScore(int redAdd, int blueAdd)
    {
        rScoreInt += redAdd;
        bScoreInt += blueAdd;
    }
	
	// Update is called once per frame
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

    void RoundOver()
    {
        gameover = true;
        redController.currentlyDestroyed = true;
        blueController.currentlyDestroyed = true;

        string winner;
        string winnerpoints;
        if (rScoreInt >= bScoreInt)
        {
            winner = "Red";
            winnerpoints = rScoreInt.ToString();
        }
        else {
            winnerpoints = bScoreInt.ToString();
            winner = "Blue";
        }

        gameoverText.text = "The Winner is " +winner +" with "+ winnerpoints + " points! \n Press Enter to return to the main menu.";

        gameoverpanel.SetActive(true);


    }

    public void damageDealt(int rDamage, int bDamage)
    {
        rhealth -= rDamage;
        bhealth -= bDamage;
        if (rhealth <= 0)
        {
            redController.shipDestroyed();
            respawnShip(true);
            rhealth = 3;
        }
        if (bhealth <= 0)
        {
            blueController.shipDestroyed();
            respawnShip(false);
            bhealth = 3;
        }
    }

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
