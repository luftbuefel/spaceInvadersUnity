using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {
    public int numEnemyRows = 3;
    public int enemiesPerRow = 5;
    public List<GameObject> enemyPrefabs = new List<GameObject>();
    public GameObject ufoPrefab;
    public Text scoreBox;
    
    private float _screenSize = 28f;
    private List<GameObject> allEnemies = new List<GameObject>();
    private int directionControl = 1;
    private int totalScore = 0;
    private GameObject enemyHolder;
    private UFOController ufoController;

    private float enemyMovementInterval = 0.5f;
    private float enemyStepDistance = 0.2f;
    private float elapsedTime;
    private Vector3 enemyHolderXPosition;
    private float enemyDescentInterval = -1f;
    bool letEnemiesMove = false;

    //make gameController a singleton
    public static GameController _instance;

    public static GameController Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<GameController>();
            }
            return _instance;
        }
    }

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
    
    //allow the screenSize to be accessed but not modified
    public float screenSize
    {
        get { return _screenSize; }
    }

	// Use this for initialization
	void Start () {
        //create an empty container to hold all the enemies and control the movement since they all move together
        enemyHolder = new GameObject();
        enemyHolder.name = "EnemyHolder";
        enemyHolderXPosition = enemyHolder.transform.position;
        AddEnemies();
        elapsedTime = Time.time;
        letEnemiesMove = true;

        //setup the UFO
        initUFO();
    }

    // Update is called once per frame
    void Update()
    {
        //create an interval to call the enemies to move
        if (letEnemiesMove)
        {
            if (Time.time - elapsedTime > enemyMovementInterval)
            {
                elapsedTime = Time.time;
                MoveEnemies();
            }
        }
	}

    //this gets called by individual enemies
    //you may want to add a timer before retriggering if you are getting multiple triggers close together
    public void changeEnemyDirection()
    {
       // print("NOW CHANGING DIRECTION");
        //reverse direction
        enemyStepDistance *= -1;
        //drop down a bit
        enemyHolderXPosition += new Vector3(0f, enemyDescentInterval);
        //increase speed by 25%
        enemyMovementInterval *= 0.75f;
    }

    private void AddEnemies()
    {
        //create each row of enemies
        for (int i = 0; i < numEnemyRows; i++)
        {        //these numbers space the rows by 3 starting at the top at 6
            float rowYPosition = 6-(3*i);
            //create the enemies in each row
            for(int j = 0; j < enemiesPerRow; j++)
            {
                //-14 -- 14 = 28 units
                float enemyXPosition = -14f+((28/enemiesPerRow)*j);
                GameObject newEnemy = Instantiate<GameObject>(enemyPrefabs[i],enemyHolder.transform);
                allEnemies.Add(newEnemy);
                newEnemy.transform.position = new Vector3(enemyXPosition,rowYPosition,0f);
            }
        }
        print("Total number of enemies added: "+allEnemies.Count);
    }

    private void initUFO()
    {
       //print("CREATED THE UFO");
       ufoController = Instantiate<GameObject>(ufoPrefab).GetComponent<UFOController>();
        //set a timeout for sending the UFO out
        startUFO();
    }

    private void startUFO()
    {
        ufoController.startMoving();
    }

    //moves enemies based on a ticker calling this method
    private void MoveEnemies()
    {
        //Store start position to move from, based on objects current transform position.
        enemyHolder.transform.position = enemyHolderXPosition + new Vector3(enemyStepDistance, 0f);
        enemyHolderXPosition = enemyHolder.transform.position;        
    }

    public void ChangeEnemyDirection()
    {
        int numEnemies = allEnemies.Count;
        //change their direction
        directionControl *= -1;
        while (numEnemies > 0)
        {
            //allEnemies[numEnemies].transform.ve
            numEnemies--;
        }
    }

    public void OnEnemyDestroyed(EnemyController enemy)
    {
        AddToScore(enemy.scoreValue);      
        allEnemies.Remove(enemy.gameObject);
        //print("TOTAL ENEMIES LEFT: " + allEnemies.Count.ToString() + "  " + enemy.gameObject.name);
        //check if all the enemies have been destroyed
        if (allEnemies.Count <= 0)
        {
            WinLevel();
        }
    }
    
    private void AddToScore(int value)
    {
        //Update the score
        totalScore += value;
        scoreBox.text = "Score: " + totalScore.ToString();
    }

    public void LoseLevel()
    {
        letEnemiesMove = false;
        print("You Lose the level!");
    }

    private void WinLevel()
    {
        letEnemiesMove = false;
        print("YAY! YOU BEAT THE LEVEL!");
    }
}
