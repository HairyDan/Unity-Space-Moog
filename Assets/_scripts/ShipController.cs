using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipController : MonoBehaviour {

    private Rigidbody rb;
    public float speed;
    public float twistSpeed;
    public float xBound, zBound;

    public ParticleSystem trail;

    private bool colliding;

    public GameObject bullet;
    private GameObject bulletEjector;

    public bool currentlyDestroyed;

    public GameObject deathExplosion;

    public AudioSource laserPew;
    public AudioSource deathSound;

    public GameObject spawnpoint1, spawnpoint2, spawnpoint3, spawnpoint4;

    // Use this for initialization
    void Start () {
        rb = GetComponent<Rigidbody>();
        bulletEjector = GameObject.FindWithTag("redBulletEjector");
        trail.Stop();
    }

    private void OnCollisionEnter(Collision collision)
    {
        colliding = true;
    }

    private void OnCollisionExit(Collision collision)
    {
        colliding = false;
    }

    private void createAndFireBullet()
    {
        AudioSource pew = Instantiate(laserPew);
        Destroy(pew.gameObject, 0.5f);
        GameObject instanceBullet = Instantiate(bullet, bulletEjector.transform.position, transform.rotation) as GameObject;
        Rigidbody laserBullet = instanceBullet.GetComponent<Rigidbody>();
        laserBullet.AddRelativeForce(new Vector3(0.0f, -1300.0f, 0.0f));
    }

    public void shipDestroyed()
    {
        AudioSource boomSound = Instantiate(deathSound);
        Destroy(boomSound.gameObject, 2.0f);
        currentlyDestroyed = true;
        GameObject boom = Instantiate(deathExplosion, transform.position, Quaternion.Euler(0, 0, 0));
        Destroy(boom, 1);
        //don't actually destroy ship, just hide to save resources
        transform.position = new Vector3(500f, 500f, 500f);
        
    }

    public void shipRespawn()
    {
        currentlyDestroyed = false;
        int seed = Random.Range(1, 5);
        if (seed == 1)
        {
            transform.position = spawnpoint1.transform.position;
            transform.rotation = spawnpoint1.transform.rotation;
        }
        if (seed == 2)
        {
            transform.position = spawnpoint2.transform.position;
            transform.rotation = spawnpoint2.transform.rotation;
        }
        if (seed == 3)
        {
            transform.position = spawnpoint3.transform.position;
            transform.rotation = spawnpoint3.transform.rotation;
        }
        if (seed == 4)
        {
            transform.position = spawnpoint4.transform.position;
            transform.rotation = spawnpoint4.transform.rotation;
        }

    }

    // Update is called once per frame
    void Update() {

        if (!currentlyDestroyed)
        {

            if (Input.GetButtonDown("RedFire"))
            {
                createAndFireBullet();
            }

            if (Input.GetButtonDown("Vertical"))
            {
                trail.Play();
            }
            if (Input.GetButtonUp("Vertical"))
            {
                trail.Stop();
            }

            float twistLeftRight = Input.GetAxis("Horizontal");
            float moveVertical = Input.GetAxis("Vertical");

            Vector3 twistVect = new Vector3(0.0f, twistLeftRight * twistSpeed, 0.0f);

            rb.angularVelocity = twistVect;

            Vector3 forwardBackward = new Vector3(0.0f, -moveVertical, 0.0f);

            rb.AddRelativeForce(forwardBackward * speed);

            rb.position = new Vector3(
                Mathf.Clamp(rb.position.x, -xBound, xBound), 0, Mathf.Clamp(rb.position.z, -zBound, zBound)
                );

            float currenty = rb.rotation.y;

            if (!colliding)
            {
                Quaternion rotationFrom = rb.rotation;
                Quaternion rotationTo = Quaternion.Euler(-90, rb.rotation.eulerAngles.y, 0);
                rb.rotation = Quaternion.RotateTowards(rotationFrom, rotationTo, Time.deltaTime * 100.0f);
                // rb.rotation = Quaternion.Euler(-90.0f, rb.rotation.eulerAngles.y, 0.0f);
            }
        }

    }
}
