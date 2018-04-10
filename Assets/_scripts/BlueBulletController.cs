using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueBulletController : MonoBehaviour
{

    public ParticleSystem explode;

    public GameController gameController;

    // Use this for initialization
    void Start()
    {
        explode.Stop();

        GameObject gameControllerObject = GameObject.FindWithTag("GameController");
        gameController = gameControllerObject.GetComponent<GameController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.ToString() != "BlueFighter (UnityEngine.SphereCollider)" && other.tag != "territory")
        {
            // Debug.Log("BULLET HIT" + other.ToString());
            Vector3 notmoving = new Vector3(0, 0, 0);
            explode.Play();
            PreventExplodeLinkedDeath();
            Destroy(gameObject);
        }
        if (other.ToString() == "RedFighter (UnityEngine.SphereCollider)")
        {
            gameController.damageDealt(1, 0);
        }
    }

    public void PreventExplodeLinkedDeath()
    {
        explode.transform.parent = null;
        Destroy(explode.gameObject, explode.main.duration);
    }

    // Update is called once per frame
    void Update()
    {

    }
}

