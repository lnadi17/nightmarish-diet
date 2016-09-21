using UnityEngine;
using System.Collections;

public class FoodScript : MovingObject {
	private Transform target;

	protected override void Start (){
		//This allows the GameManager to issue movement commands.
		GameManager.instance.AddFoodToList (this);
		target = GameObject.FindGameObjectWithTag ("Player").transform;
		base.Start ();
	}

	//MoveFood is called by the GameManger each turn to tell each food to try to move towards the player.
	public bool MoveFood (){
		int xDir = 0;
		int yDir = 0;

		if(Mathf.Abs (target.position.x - transform.position.x) < float.Epsilon)
			yDir = target.position.y > transform.position.y ? 1 : -1;
		else
			xDir = target.position.x > transform.position.x ? 1 : -1;
			
		return (Move (xDir, yDir));
	}
}
