using UnityEngine;
using System.Collections;
using System.Collections.Generic; //Allows us to use lists

public class GameManager : MonoBehaviour {

	public static GameManager instance = null; //Static instance of GameManager which allows it to be accessed by any other script
	private BoardManager boardScript; //Store a reference to our BoardManager which will set up the level
	private int level = 3; //Current level number, expressed in game as "Day 1"
	public int playerFoodPoints = 100;
	[HideInInspector] public bool playersTurn = true;

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

		//Get a component reference to the attached BoardManager script
		boardScript = GetComponent<BoardManager> ();

		//Call the InitGame function to initialize the first level
		InitGame ();
	}

	//Initializes the game for each level
	void InitGame(){
		print ("GameManager: Called BoardScript's SetupScene");
		//Call the SetupScene function of the BoardManager script, pass it current level number
		boardScript.SetupScene (level);
	}

	public void GameOver(){
		enabled = false;	
	}
}
