using UnityEngine;
using System.Collections;

public class FoodScript : MovingObject {

	private Transform target;                           //Transform to attempt to move toward each turn.
	private bool skipMove;                              //Boolean to determine whether or not food should skip a turn or move this turn.

	//Start overrides the virtual Start function of the base class.
	protected override void Start ()
	{
		//Register this fiid with our instance of GameManager by adding it to a list of FoodScript objects. 
		//This allows the GameManager to issue movement commands.
		GameManager.instance.AddFoodToList (this);

		//Find the Player GameObject using it's tag and store a reference to its transform component.
		target = GameObject.FindGameObjectWithTag ("Player").transform;

		//Call the start function of our base class MovingObject.
		base.Start ();
	}


	//Override the AttemptMove function of MovingObject to include functionality needed for food to skip turns.
	//See comments in MovingObject for more on how base AttemptMove function works.
	protected override void AttemptMove (int xDir, int yDir)
	{
		//Check if skipMove is true, if so set it to false and skip this turn.
		if(skipMove)
		{
			skipMove = false;
			return;

		}

		//Call the AttemptMove function from MovingObject.
		base.AttemptMove (xDir, yDir);

		//Now that food has moved, set skipMove to true to skip next move.
		skipMove = true;
	}


	//MoveFood is called by the GameManger each turn to tell each food to try to move towards the player.
	public void MoveFood ()
	{
		//Declare variables for X and Y axis move directions, these range from -1 to 1.
		//These values allow us to choose between the cardinal directions: up, down, left and right.
		int xDir = 0;
		int yDir = 0;

		//If the difference in positions is approximately zero (Epsilon) do the following:
		if(Mathf.Abs (target.position.x - transform.position.x) < float.Epsilon)

			//If the y coordinate of the target's (player) position is greater than the y coordinate of this food's position set y direction 1 (to move up). If not, set it to -1 (to move down).
			yDir = target.position.y > transform.position.y ? 1 : -1;

		//If the difference in positions is not approximately zero (Epsilon) do the following:
		else
			//Check if target x position is greater than food's x position, if so set x direction to 1 (move right), if not set to -1 (move left).
			xDir = target.position.x > transform.position.x ? 1 : -1;

		//Call the AttemptMove function and pass in the generic parameter Player, because food is moving and expecting to potentially encounter a Player
		AttemptMove (xDir, yDir);
	}
}
