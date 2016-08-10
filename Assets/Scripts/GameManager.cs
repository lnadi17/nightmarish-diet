using UnityEngine;
using System.Collections;
using System.Collections.Generic; 

public class GameManager : MonoBehaviour 
{
	public float levelStartDelay = 2f;
	public float turnDelay = 0.1f;
	public static GameManager instance = null; //Static instance of GameManager which allows it to be accessed by any other script
	public int playerFoodPoints = 0; //Player's starting food points
	[HideInInspector] public bool playersTurn = true; 
	[HideInInspector] public List<FoodScript> foods; 
	//public int foodIndex; //Used to store food's index;

	private BoardManager boardScript; //Store a reference to our BoardManager which will set up the level
	private int level = 1; 
	private bool foodsMoving; //Boolean to check if foods are moving.


	//Awake is always called before any Start functions
	void Awake(){
		
		//Check if instance already exists
		if (instance == null){
			//if not, set the instance to this
			instance = this;
		}

		//If instance already exists and is not this:
		else if (instance != this){
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
		level++;
		InitGame();
	}


	//Initializes the game for each level
	void InitGame(){
		foods.Clear ();
		boardScript.SetupScene (level);
	}

	public void GameOver(){
		enabled = false;	
	}


	void Update(){
		if(playersTurn || foodsMoving)
			return;
		StartCoroutine (MoveFoods ());
	}


	//Call this to add the passed in food to the List of food objects.
	public void AddFoodToList(FoodScript script)
	{
		foods.Add(script);
	}


	//Coroutine to move foods in sequence.
	IEnumerator MoveFoods()
	{
		foodsMoving = true;
		yield return new WaitForSeconds(turnDelay);

		if (foods.Count == 0) 
		{
			yield return new WaitForSeconds(turnDelay);
		}

		for (int i = 0; i < foods.Count; i++)
		{
			foods[i].MoveFood();

			//Wait for Food's moveTime before moving next Food, 
			yield return new WaitForSeconds(foods[i].moveTime);
		}

		playersTurn = true;
		foodsMoving = false;
	}
}
