using UnityEngine;
using System.Collections;

//The abstract keyword enables you to create classes and class members that are incomplete and must be implemented in a derived class.
public abstract class MovingObject : MonoBehaviour
{
	public float moveTime = 0.1f;           //Time it will take object to move, in seconds.
	public LayerMask blockingLayer;         //Layer on which collision will be checked.

	private BoxCollider2D boxCollider;      
	private Rigidbody2D rb2D;               
	private float inverseMoveTime;          //Used to make movement more efficient.

	//Protected, virtual functions can be overridden by inheriting classes.
	protected virtual void Start ()
	{
		boxCollider = GetComponent <BoxCollider2D> ();
		rb2D = GetComponent <Rigidbody2D> ();
		inverseMoveTime = 1f / moveTime;
	}


	//Move returns true if it is able to move and false if not. 
	//Move takes parameters for x direction, y direction and a RaycastHit2D to check collision.
	protected bool Move (int xDir, int yDir, out RaycastHit2D hit)
	{
		Vector2 start = transform.position;
		Vector2 end = start + new Vector2 (xDir, yDir);
	
		boxCollider.enabled = false;
		hit = Physics2D.Linecast (start, end, blockingLayer);
		boxCollider.enabled = true;

		if(hit.transform == null)
		{
			//If nothing was hit, start SmoothMovement co-routine passing in the Vector2 end as destination
			StartCoroutine (SmoothMovement (end));
			//Return true to say that Move was successful
			return true;
		}

		//If something was hit, return false, Move was unsuccesful.
		return false;
	}


	//Co-routine for moving units from one space to next, takes a parameter end to specify where to move to.
	protected IEnumerator SmoothMovement (Vector3 end)
	{
		float sqrRemainingDistance = (transform.position - end).sqrMagnitude;

		while(sqrRemainingDistance > float.Epsilon)
		{
			Vector3 newPostion = Vector3.MoveTowards(rb2D.position, end, inverseMoveTime * Time.deltaTime);
			rb2D.MovePosition (newPostion);
			sqrRemainingDistance = (transform.position - end).sqrMagnitude;
			yield return null;
		}
	}


	//The virtual keyword means AttemptMove can be overridden by inheriting classes using the override keyword.
	protected virtual void AttemptMove (int xDir, int yDir)
	{
		//Hit will store whatever our linecast hits when Move is called.
		RaycastHit2D hit;

		Move (xDir, yDir, out hit);
	}
}
