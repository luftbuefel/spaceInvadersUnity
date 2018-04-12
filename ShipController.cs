using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipController: DestroyableThing {
    
    public float shipSpeed = 28f;
    public GameObject bullet;
    public float bulletReloadTime = 3f;
    public float bulletSpeed = 15f;
    public AudioClip fireBulletSound;
    private float bulletSpawnPointYOffset = 1.52f;
    private float timeSinceLastShot;
    private float yPosition;
    private float zPosition;
    //for now you can only shoot one bullet at a time
    private GameObject currentBullet;
    private AudioSource audioPlayer;

	// Use this for initialization
	void Start () {
        yPosition = transform.position.y;
        zPosition = transform.position.z;      
        audioPlayer = GetComponent<AudioSource>();
        audioPlayer.clip = fireBulletSound;
    }
	
	// Update is called once per frame
	void Update () {
        //move the ship
        Move();

        //fire a bullet if the fire key was pressed
        if (Input.GetButtonDown("Fire1"))
        {
            Shoot();
        }
	}
    
    //all movement of the ship is handled in this method
    private void Move()
    {
        float horizontalMovement = Input.GetAxis("Horizontal") * Time.deltaTime * shipSpeed;
        float xPosition = gameObject.transform.position.x;
        //make sure the ship doesn't go offscreen
        if (xPosition < -14f)
        {
            //stop the ship movement
            horizontalMovement = 0f;
            //position the ship at the edge
            transform.position = new Vector3(-14f, yPosition, zPosition);
        }
        else if (xPosition > 14f)
        {
            //stop the ship movement
            horizontalMovement = 0f;
            //position the ship at the edge
            transform.position = new Vector3(14f, yPosition, zPosition);
        }
        //make sure the ship does not move offscreen
        transform.Translate(horizontalMovement, 0,0);
    }

    private void Shoot()
    {
        if (currentBullet == null || timeSinceLastShot + bulletReloadTime<=Time.fixedTime)
        {
            //play the ship shooting sound
            //audioPlayer.Stop();
            //audioPlayer.clip = fireBulletSound;
            audioPlayer.Play();
           // print("Fire a bullet "+(timeSinceLastShot+bulletReloadTime).ToString()+" NOW: "+Time.fixedTime.ToString());
            //spawn a bullet bulletSpawnPointYOffset units in front of the ship
            Vector3 spawnPosition = transform.position + new Vector3(0f, bulletSpawnPointYOffset, 0f);
            currentBullet = Instantiate(bullet, spawnPosition, Quaternion.identity);
            currentBullet.GetComponent<Rigidbody>().velocity = new Vector3(0f,bulletSpeed,0f);
            //reset the timeSinceLastShot
            timeSinceLastShot = Time.fixedTime;
        }
    }
    
    //whenever something touches the ship you can assume that the ship explodes
    internal override void OnTriggerEnter(Collider other)
    {
        Explode();
        print("Ship was touched by " + other.name);
    }

    public override void Explode()
    {
        base.Explode();
        print("Ship Explodes");
        GameController.Instance.LoseLevel();
    }
}
