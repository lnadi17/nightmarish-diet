using UnityEngine;
using System.Collections;

public class FoodScript : MovingObject 
{
	private Transform target;                           //Transform to attempt to move toward each turn.
	private bool skipMove;                              //Boolean to determine whether or not food should skip a turn or move this turn.


	//Start overrides the virtual Start function of the base class.
	protected override void Start ()
	{
		//This allows the GameManager to issue movement commands.
		GameManager.instance.AddFoodToList (this);
		target = GameObject.FindGameObjectWithTag ("Player").transform;
		base.Start ();
	}


	//Override the AttemptMove function of MovingObject to include functionality needed for food to skip turns.
	//See comments in MovingObject for more on how base AttemptMove function works.
	protected override void AttemptMove (int xDir, int yDir)
	{
		if(skipMove)
		{
			skipMove = false;
			return;
		}
		base.AttemptMove (xDir, yDir);
		skipMove = true;
	}


	//MoveFood is called by the GameManger each turn to tell each food to try to move towards the player.
	public void MoveFood ()
	{
		int xDir = 0;
		int yDir = 0;

		//If the difference in positions is approximately zero (Epsilon) do the following:
		if(Mathf.Abs (target.position.x - transform.position.x) < float.Epsilon)
			yDir = target.position.y > transform.position.y ? 1 : -1;
		else
			xDir = target.position.x > transform.position.x ? 1 : -1;

		//Call the AttemptMove function and pass in the generic parameter Player, because food is moving and expecting to potentially encounter a Player
		AttemptMove (xDir, yDir);
	}
}
