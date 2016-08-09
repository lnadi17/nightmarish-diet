using UnityEngine;
using System;
using System.Collections.Generic;
using Random = UnityEngine.Random; 

public class BoardManager : MonoBehaviour 
{
	//Using Serializable allows us to embed a class with sub properties in the inspector
	[Serializable]
	public class Count {
		public int minimum;
		public int maximum;

		//Assignment constructor.
		public Count (int min, int max){
			minimum = min;
			maximum = max;
		}
	}

	public int columns = 8;
	public int rows = 8; 
	public Count wallCount = new Count(5,9); 
	public GameObject exit; 
	public GameObject[] floorTiles; 
	public GameObject[] foodTiles; 
	public GameObject[] wallTiles; 
	public GameObject[] outerWallTiles; 

	private Transform boardHolder; //A variable to store a reference to the transform of our Board object
	private List<Vector3> gridPositions = new List<Vector3>(); //A list of possible locations to place tiles


	void InitialiseList(){ //Clears our list gridPositions and prepares it to generate a new board
		gridPositions.Clear (); 
		for (int x = 1; x < columns - 1; x++) { 
			for (int y = 1; y < rows - 1; y++) { 
				gridPositions.Add (new Vector3 (x, y, 0f)); 
			}
		}
	}


	void BoardSetup(){ //Sets up the outer walls and floor (background) of the game board
		boardHolder = new GameObject ("Board").transform; 
		for (int x = -1; x <= columns; x++){ 
			for (int y = -1; y <= rows; y++){ 
				//Choose a random tile from our array of floor tile prefabs and prepare to instantiate it
				GameObject toInstantiate = floorTiles[Random.Range(0, floorTiles.Length)];
				if (x == -1 || x == columns || y == -1 || y == rows){ 
					toInstantiate = outerWallTiles[Random.Range(0, outerWallTiles.Length)];
				}
				//Insantiante the gameObject instance using the prefab chosen for toInstantiate at the Vector3 corresponding to current grid position in loop, cast it to GameObject
				GameObject instance = Instantiate (toInstantiate, new Vector3 (x, y, 0f), Quaternion.identity) as GameObject;
				//Set the parent of our newly instantiated object instance to boardHolder, this is just organizational
				instance.transform.SetParent (boardHolder); 
			}
		}
	}


	//RandomPosition returns a random position from our list gridPositions
	Vector3 RandomPosition(){
		int randomIndex = Random.Range (0, gridPositions.Count);
		Vector3 randomPosition = gridPositions [randomIndex];
		gridPositions.RemoveAt (randomIndex);
		return randomPosition;
	}


	//LayoutObjectAtRandom accepts an array of game objects to choose from along with a minimum and maximum range for the number of objects to create
	void LayoutObjectAtRandom(GameObject[] tileArray, int minimum, int maximum){
		//Choose a random number of objects to instantiate within the minimum and maximum limits
		int objectCount = Random.Range (minimum, maximum + 1);
		for (int i = 0; i < objectCount; i++) {
			Vector3 randomPosition = RandomPosition ();
			GameObject tileChoice = tileArray [Random.Range (0, tileArray.Length)];
			Instantiate (tileChoice, randomPosition, Quaternion.identity);
		}
	}
		

	//SetupScene initializes our level and calls the previous functions to lay out the game board
	public void SetupScene(int level){
		BoardSetup ();
		InitialiseList ();
		LayoutObjectAtRandom (wallTiles, wallCount.minimum, wallCount.maximum);
		int foodCount = level;
		LayoutObjectAtRandom (foodTiles, foodCount, foodCount);
		Instantiate (exit, new Vector3 (columns - 1, rows - 1, 0f), Quaternion.identity);
	}
}
