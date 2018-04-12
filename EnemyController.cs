using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : DestroyableThing {

    public int scoreValue = 10;
    public GameObject bullet;
    float xMovement = 10f;
    float bulletSpawnPointYOffset = -1.52f;
    //this value is negative because we want the bullets to go down
    public float bulletSpeed = -15f;
    
    private GameController gameController;

	// Use this for initialization
	void Awake () {
        gameController = GameController.Instance;
	}
	        
    public void Move()
    {
        gameObject.transform.Translate(new Vector3(xMovement, 0f, 0f));
    }


    //enemies can only be destroyed by bullets from the ship and not by barriers or their own bullets
    internal override void OnTriggerEnter(Collider other)
    {
        //base.OnTriggerEnter(other);
        if (other.gameObject.name == "ShipBullet(Clone)")
        {
            Explode();
        }
        //when you hit the side wall tell the enemy controller to reverse direction
        if (other.gameObject.name == "sideWall")
        {
            //trigger the enemy holder to reverse direction
            gameController.changeEnemyDirection();
        }
        //if an enemy hits the bottom, then the game is over
        if(other.gameObject.name == "particleDestroyerBottom")
        {
            gameController.LoseLevel();
        }
    }

    //when an enemy dies, the score is increased
    public override void Explode()
    {
        gameController.OnEnemyDestroyed(this);
        base.Explode();
    }

    public void Attack()
    {
        Vector3 spawnPosition = transform.position + new Vector3(0f, bulletSpawnPointYOffset, 0f);
        GameObject currentBullet = Instantiate(bullet, spawnPosition, Quaternion.identity);
        currentBullet.GetComponent<Rigidbody>().velocity = new Vector3(0f, bulletSpeed, 0f);
    }
}
