using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Paired with BlueBulletController - Script to manage the behaviour of a bullet fired by the Red ship.
public class RedBulletController : MonoBehaviour {

    public ParticleSystem explode;

    public GameController gameController;

    void Start () {
        explode.Stop();
        GameObject gameControllerObject = GameObject.FindWithTag("GameController");
        gameController = gameControllerObject.GetComponent<GameController>();

    }

    //Recognise when hitting something (that isn't your own ship or a territory)
    private void OnTriggerEnter(Collider other)
    {
        if (other.ToString() != "RedFighter (UnityEngine.SphereCollider)" && other.tag != "territory")
        {
            // Debug.Log("BULLET HIT" + other.ToString());
            Vector3 notmoving = new Vector3(0, 0, 0);
            explode.Play();
            PreventExplodeLinkedDeath();
            Destroy(gameObject);
        }
        if (other.ToString() == "BlueFighter (UnityEngine.SphereCollider)")
        {
            gameController.damageDealt(0, 1);
        }
    }

    //Simple method to prevent explosion from being destroyed as the bullet is
    public void PreventExplodeLinkedDeath()
    {
        explode.transform.parent = null;
        Destroy(explode.gameObject, explode.main.duration);
    }

}
