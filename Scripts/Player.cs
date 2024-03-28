using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	public bool canPick = false;

	public bool picked = false;
	public bool won = false;

	public GameObject[] vasos;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	public void MirarRayo(RaycastHit hit) {
		if (canPick == true) {
			Cup cup = hit.transform.GetComponent<Cup> ();
			if (cup != null) {
				canPick = false;
				picked = true;
				won = (cup.ball != null);
				cup.MoveUp ();
			}
		}


	}


}
