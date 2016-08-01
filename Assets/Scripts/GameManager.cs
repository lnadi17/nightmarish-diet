using UnityEngine;
using System.Collections;
using System.Collections.Generic; //Allows us to use lists

public class GameManager : MonoBehaviour {

	public float levelStartDelay = 2f;
	public float turnDelay = .1f;
	public static GameManager instance = null; //Static instance of GameManager which allows it to be accessed by any other script
	public int playerFoodPoints = 100; //Player's starting food points
	[HideInInspector] public bool playersTurn = true; //Indicates if it's player's turn to move in game.
	[HideInInspector] public List<FoodScript> foods; //List of all Food units, used to issue them move commands.
	//public int foodIndex; //Used to store food's index;

	private BoardManager boardScript; //Store a reference to our BoardManager which will set up the level
	private int level = 3; //Current level number, expressed in game as "Day 1"
	private bool foodsMoving; //Boolean to check if foods are moving.

	//Awake is always called before any Start functions
	void Awake(){
		
		//Check if instance already exists
		if (instance == null){
			//if not, set the instance to this
			instance = this;
			print ("GameManager: Instance was null, now instance is this");
		}

		//If instance already exists and is not this:
		else if (instance != this){
			print ("GameManager: There was another instance, so I destoryed it");
			//Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager
			Destroy(gameObject);
		}

		//Sets this to not be destroyed when reloading scene
		DontDestroyOnLoad (gameObject);

		//Assign food to a new List of FoodScript objects.
		foods = new List<FoodScript>();

		//Get a component reference to the attached BoardManager script
		boardScript = GetComponent<BoardManager> ();

		//Call the InitGame function to initialize the first level
		InitGame ();
	}

	void OnLevelWasLoaded(int index)
	{
		//Add one to our level number.
		level++;
		//Call InitGame to initialize our level.
		InitGame();
	}

	//Initializes the game for each level
	void InitGame(){
		foods.Clear ();
		print ("GameManager: Called BoardScript's SetupScene");
		//Call the SetupScene function of the BoardManager script, pass it current level number
		boardScript.SetupScene (level);
	}

	public void GameOver(){
		enabled = false;	
	}

	void Update(){
		//Check that playersTurn or enemiesMoving or doingSetup are not currently true.
		if(playersTurn || foodsMoving)

			//If any of these are true, return and do not start MoveEnemies.
			return;

		//Start moving enemies.
		StartCoroutine (MoveFoods ());
	}

	//Call this to add the passed in food to the List of food objects.
	public void AddFoodToList(FoodScript script)
	{
		//Add food to List enemies.
		foods.Add(script);
	}

	//Coroutine to move enemies in sequence.
	IEnumerator MoveFoods()
	{
		//While foodsMoving is true player is unable to move.
		foodsMoving = true;

		//Wait for turnDelay seconds, defaults to .1 (100 ms).
		yield return new WaitForSeconds(turnDelay);

		//If there are no foods spawned (IE in first level):
		if (foods.Count == 0) 
		{
			//Wait for turnDelay seconds between moves, replaces delay caused by enemies moving when there are none.
			yield return new WaitForSeconds(turnDelay);
		}

		//Loop through List of food objects.
		for (int i = 0; i < foods.Count; i++)
		{
			//Call the MoveFood function of food at index i in the enemies List.
			foods[i].MoveFood();

			//Wait for Food's moveTime before moving next Food, 
			yield return new WaitForSeconds(foods[i].moveTime);
		}
		//Once foods are done moving, set playersTurn to true so player can move.
		playersTurn = true;

		//Foods are done moving, set foodsMoving to false.
		foodsMoving = false;
	}
}
