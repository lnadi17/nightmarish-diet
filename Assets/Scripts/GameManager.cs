using UnityEngine;
using System.Collections;
using System.Collections.Generic; 
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour 
{
	public float levelStartDelay = 2f;
	public float turnDelay = 0.5f;
	public static GameManager instance = null;
	public int playerFoodPoints = 0; //Player's starting food points
	[HideInInspector] public bool playersTurn = true; 
	[HideInInspector] public List<FoodScript> foods; 

	private BoardManager boardScript;
	private int level = 1; 
	private bool foodsMoving;


	void Awake(){
		if (instance == null){
			instance = this;
		}
		else if (instance != this){
			Destroy(gameObject);
		}

		DontDestroyOnLoad (gameObject);

		foods = new List<FoodScript>();
		boardScript = GetComponent<BoardManager> ();
		InitGame ();
	}

	//Deprecated
	void OnLevelWasLoaded(int index){
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

	public void AddFoodToList(FoodScript script){
		foods.Add(script);
	}

	//Coroutine to move foods in sequence.
	IEnumerator MoveFoods(){
		foodsMoving = true;
		yield return new WaitForSeconds(turnDelay);

		if (foods.Count == 0){
			yield return new WaitForSeconds(turnDelay);
		}

		for (int i = 0; i < foods.Count; i++){
			foods[i].MoveFood();
			//Wait for Food's moveTime before moving next Food, 
			yield return new WaitForSeconds(foods[i].moveTime);
		}

		playersTurn = true;
		foodsMoving = false;
	}
}
