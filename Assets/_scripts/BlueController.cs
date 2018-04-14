using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueController : MonoBehaviour {

    private Rigidbody rb;
    public float speed;
    public float twistSpeed;

    private bool colliding;

    public ParticleSystem trail;

    public GameObject bullet;
    private GameObject bulletEjector;

    public bool currentlyDestroyed;

    public GameObject deathExplosion;

    public AudioSource laserPew;
    public AudioSource deathSound;

    public GameObject spawnpoint1, spawnpoint2, spawnpoint3, spawnpoint4;

    //Set-up references, initialise particle trail as stopped
    void Start () {
        rb = GetComponent<Rigidbody>();
        bulletEjector = GameObject.FindWithTag("blueBulletEjector");
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

    //Instantiates new bullet, and the associated noise.
    private void createAndFireBullet()
    {
        AudioSource pew = Instantiate(laserPew);
        Destroy(pew.gameObject, 0.5f);
        GameObject instanceBullet = Instantiate(bullet, bulletEjector.transform.position, transform.rotation) as GameObject;
        Rigidbody laserBullet = instanceBullet.GetComponent<Rigidbody>();
        // Vector3 bulletVelocity = new Vector3(0.0f, 0.0f, 100.0f);
        // laserBullet.velocity = bulletVelocity;
        laserBullet.AddRelativeForce(new Vector3(0.0f, -1300.0f, 0.0f));
    }

    //When ship is destroyed, it explodes and makes a noise, but the gameobject is just hidden until respawn for simplicity
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

    //Respawn ship in one of the 4 random locations with the correct transform
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
    void Update()
    {
        //Check to prevent ship from doing anything during respawn timer
        if (!currentlyDestroyed)
        {
            
            if (Input.GetButtonDown("BlueFire"))
            {
                createAndFireBullet();
            }

            //Play trail when moving
            if (Input.GetButtonDown("VerticalBlue"))
            {
                trail.Play();
            }
            if (Input.GetButtonUp("VerticalBlue"))
            {
                trail.Stop();
            }


            //Clamps the ship's y axis to prevent unwanted 3D "fun"
            rb.position = new Vector3(
                rb.position.x, 0, rb.position.z);


            //In case of unwanted twist being applied by a dodgey collision - twist is undone
            if (!colliding)
            {
                Quaternion rotationFrom = rb.rotation;
                Quaternion rotationTo = Quaternion.Euler(-90, rb.rotation.eulerAngles.y, 0);
                rb.rotation = Quaternion.RotateTowards(rotationFrom, rotationTo, Time.deltaTime * 100.0f);
            }
        }

    }

    void FixedUpdate(){
         if (!currentlyDestroyed)
        {
          //Set floats for turning and acceleration based on GetAxis inputs, for modularity
            float twistLeftRight = Input.GetAxis("HorizontalBlue");
            float moveVertical = Input.GetAxis("VerticalBlue");

            Vector3 twistVect = new Vector3(0.0f, twistLeftRight * twistSpeed, 0.0f);

            rb.angularVelocity = twistVect;

            Vector3 forwardBackward = new Vector3(0.0f, -moveVertical, 0.0f);

            rb.AddRelativeForce(forwardBackward * speed);
        }
    }
}
