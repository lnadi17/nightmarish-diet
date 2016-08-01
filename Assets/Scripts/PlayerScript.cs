using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

//PlayerScript inherits from MovingObject, our base class for objects that can move.
public class Player : MovingObject
{
	public float restartLevelDelay = 1f;        //Delay time in seconds to restart level.
	public int pointsPerFood = 10;              //Number of points to add to player food points when picking up a food object.

	private SpriteRenderer spriteRdr;           //Used to store a reference to the Player's SpriteRenderer component.
	private int food;                           //Used to store player food points total during level.


	//Start overrides the Start function of MovingObject
	protected override void Start ()
	{
		//Store our SpriteRenderer component reference to the spiteRdr variable.
		spriteRdr = GetComponent<SpriteRenderer> ();

		//Get the current food point total stored in GameManager.instance between levels.
		food = GameManager.instance.playerFoodPoints;

		//Call the Start function of the MovingObject base class.
		base.Start ();
	}


	//This function is called when the behaviour becomes disabled or inactive.
	private void OnDisable ()
	{
		//When Player object is disabled, store the current local food total in the GameManager so it can be re-loaded in next level.
		GameManager.instance.playerFoodPoints = food;
	}


	private void Update ()
	{
		//If it's not the player's turn, exit the function.
		if(!GameManager.instance.playersTurn) return;

		int horizontal = 0;     //Used to store the horizontal move direction.
		int vertical = 0;       //Used to store the vertical move direction.


		//Get input from the input manager, round it to an integer and store in horizontal to set x axis move direction
		horizontal = (int) (Input.GetAxisRaw ("Horizontal"));

		//Get input from the input manager, round it to an integer and store in vertical to set y axis move direction
		vertical = (int) (Input.GetAxisRaw ("Vertical"));

		//Check if moving horizontally, if so set vertical to zero.
		if(horizontal != 0)
		{
			vertical = 0;
		}

		//Check if we have a non-zero value for horizontal or vertical
		if(horizontal != 0 || vertical != 0)
		{
			//Call AttemptMove passing in the generic parameter Wall, since that is what Player may interact with if they encounter one (by attacking it)
			//Pass in horizontal and vertical as parameters to specify the direction to move Player in.
			AttemptMove (horizontal, vertical);
		}
	}


	//AttemptMove overrides the AttemptMove function in the base class MovingObject.
	protected override void AttemptMove (int xDir, int yDir)
	{

		//Call the AttemptMove method of the base class passing x and y direction to move.
		base.AttemptMove (xDir, yDir);

		//Hit allows us to reference the result of the Linecast done in Move.
		RaycastHit2D hit;

		//If Move returns true, meaning Player was able to move into an empty space.
		//If not, it will wait until player makes a valid move.
		if (Move (xDir, yDir, out hit)) 
		{
			//Every time player moves, subtract from food points total.
			food--;

			//Since the player has moved and lost food points, check if the game has ended.
			CheckIfGameOver ();

			//Checks if sprite needs to change.
			CheckSprite ();
		
			//Set the playersTurn boolean of GameManager to false now that players turn is over.
			GameManager.instance.playersTurn = false;
		}
	}


	//OnTriggerEnter2D is sent when another object enters a trigger collider attached to this object (2D physics only).
	private void OnTriggerEnter2D (Collider2D other)
	{
		//Check if the tag of the trigger collided with is Exit.
		if(other.tag == "Exit")
		{
			//Invoke the Restart function to start the next level with a delay of restartLevelDelay (default 1 second).
			Invoke ("Restart", restartLevelDelay);

			//Disable the player object since level is over.
			enabled = false;
		}

		//Check if the tag of the trigger collided with is Food.
		else if(other.tag == "Food")
		{
			//Add pointsPerFood to the players current food total.
			food += pointsPerFood;

			//Disable the food object the player collided with.
			other.gameObject.SetActive (false);
		}
			
	}


	//Changes player sprite if needed.
	private void CheckSprite(){
		print (spriteRdr.name); //Just.
	}


	//Restart reloads the scene when called.
	private void Restart ()
	{
		//Load the active scene loaded, in this case Main, the only scene in the game.
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}


	//CheckIfGameOver checks if the player is out of food points and if so, ends the game.
	private void CheckIfGameOver ()
	{
		//Check if food point total is less than or equal to zero.
		if (food <= 0) 
		{
			//Call the GameOver function of GameManager.
			GameManager.instance.GameOver ();
		}
	}
}