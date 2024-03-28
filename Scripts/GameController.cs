using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameController : MonoBehaviour {

	public TextMesh infoText;
	public GameObject ball;
	public Player player;
	public Cup[] cups;
	public GameObject manoIzquierda, manoDerecha;
	

	private float resetTimer = 3f;

	// Use this for initialization
	void Start () {
		infoText.text = "¿dónde está la bolita?, espera un momento";
		foreach (Cup cup in cups)
		{
			cup.ball=null;
		}

		StartCoroutine (ShuffleRoutine());
	}
	
	// Update is called once per frame
	void Update () {
		if (player.picked) {
			if (player.won) {
				infoText.text = "Has Ganado!";
			} else {
				infoText.text = "Has perdido :( intentalo de nuevo!";
			}

			resetTimer -= Time.deltaTime;
			if (resetTimer <= 0f) {
				SceneManager.LoadScene (SceneManager.GetActiveScene().name);
			}
		}
	}

	private IEnumerator ShuffleRoutine () {
		yield return new WaitForSeconds (1f);

		foreach (Cup cup in cups) {
			cup.MoveUp ();
		}

		yield return new WaitForSeconds (0.5f);

		
		Cup targetCup = cups[Random.Range(0, cups.Length)];
		targetCup.ball = ball;
		
		

		ball.transform.position = new Vector3 (
			targetCup.transform.position.x,
			ball.transform.position.y,
			targetCup.transform.position.z
		);


		yield return new WaitForSeconds (1.0f);
		
		foreach (Cup cup in cups) {
			cup.MoveDown ();
		}

		yield return new WaitForSeconds (1.0f);

		for (int i = 0; i < 5; i++) {
			Cup cup1 = cups[Random.Range(0, cups.Length)];
			Cup cup2 = cup1;

			while (cup2 == cup1) {
				cup2 = cups[Random.Range(0, cups.Length)];
			}

			Vector3 cup1Position = cup1.targetPosition;

			cup1.targetPosition = cup2.targetPosition;
			cup2.targetPosition = cup1Position;

			yield return new WaitForSeconds (0.75f);
		}

		player.canPick = true;
		infoText.text = "Pincha en el Correcto";
	}
}
