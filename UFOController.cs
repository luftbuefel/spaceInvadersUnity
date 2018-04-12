using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UFOController : DestroyableThing {

    public int minPointValue = 150;
    public int maxPointValue = 250;
    public float maxXPosition = 16.5f;
    public float speed = 10f;

    int scoreValue;

    // Use this for initialization
    void Start () {
        //get a random point value from the specified range
        scoreValue = Random.Range(minPointValue,maxPointValue);
        //make the score value a multiple of 5
        scoreValue += scoreValue % 5;
	}
	
    public void startMoving()
    {
        gameObject.GetComponent<Rigidbody>().velocity = new Vector2(speed, 0f);
    }

	// Update is called once per frame
	void Update () {
        //if it is moving then check if it is still onscreen, otherwise destroy it
        if (transform.position.x > maxXPosition)
        {
            base.Explode();
        }
	}

    //enemies can only be destroyed by bullets from the ship and not by barriers or their own bullets
    internal override void OnTriggerEnter(Collider other)
    {
        //base.OnTriggerEnter(other);
        if (other.gameObject.name == "ShipBullet(Clone)")
        {
            Explode();
        }
    }
    
    override public void Explode()
    {
        //run an explosion animation with sound
        base.Explode();
    }
}
