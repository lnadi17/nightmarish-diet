using UnityEngine;
using System.Collections;

public class Loader : MonoBehaviour {

	public GameObject gameManager; //GameManager prefab to instantiate

	void Awake () {
		//Check if a GameManager has already been assigned to static variable GameManager.instance or if it's still null
		if(GameManager.instance == null){
			print ("Loader: GameManager.instance == null, instantiating GameManager prefab");
			//Instantiate gameManager prefab
			Instantiate (gameManager);
		}
	}
}
