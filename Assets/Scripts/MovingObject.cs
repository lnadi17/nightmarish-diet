using UnityEngine;
using System.Collections;

public abstract class MovingObject : MonoBehaviour{
	public float moveTime = 0.5f;
	public LayerMask blockingLayer;
	public BoxCollider2D boxCollider;      
	public Rigidbody2D rb2D;

	private float inverseMoveTime;

	//Protected, virtual functions can be overridden by inheriting classes.
	protected virtual void Start (){
		boxCollider = GetComponent <BoxCollider2D> ();
		rb2D = GetComponent <Rigidbody2D> ();
		inverseMoveTime = 1f / moveTime;
	}


	//Move returns true if it is able to move and false if not. 
	protected virtual bool Move (int xDir, int yDir){
		RaycastHit2D hit;

		Vector2 start = transform.position;
		Vector2 end = start + new Vector2 (xDir, yDir);
	
		boxCollider.enabled = false;
		hit = Physics2D.Linecast (start, end, blockingLayer);
		boxCollider.enabled = true;

		if(hit.transform == null || transform.tag == "Player" && hit.transform.tag != "Untagged"){
			StartCoroutine (SmoothMovement (end));
			return true;
		}

		return false;
	}

	//Co-routine for moving units from one space to next, takes a parameter end to specify where to move to.
	protected IEnumerator SmoothMovement (Vector3 end){
		float sqrRemainingDistance = (transform.position - end).sqrMagnitude;

		while(sqrRemainingDistance > float.Epsilon){
			Vector3 newPostion = Vector3.MoveTowards(rb2D.position, end, inverseMoveTime * Time.deltaTime);
			rb2D.MovePosition (newPostion);
			sqrRemainingDistance = (transform.position - end).sqrMagnitude;
			yield return null;
		}
	}

	//The virtual keyword means AttemptMove can be overridden by inheriting classes using the override keyword.
	//protected virtual void AttemptMove (int xDir, int yDir){
	//	RaycastHit2D hit;
	//	Move (xDir, yDir, out hit);
	//}
}
