using UnityEngine;
using System.Collections;

public class MineBox : MonoBehaviour {

	public GameObject textMesh;
	TextMesh myMesh;

	public GameObject standardSplode;
	public GameObject gameOverSplode;

	public bool isMine = false;
	private MineManager manager;

	void Start() {
		//	Get the mine manager
		GameObject managerObj = GameObject.FindGameObjectWithTag ("MineManager");
		manager = managerObj.GetComponent<MineManager> ();

		if (manager == null) {
			Debug.LogError("Error: Unable to find 'MineManager'");
		}
		//	Add text meshes to center of cube cause Unity decided it was cool to make it visible over all objects
		GameObject text = Instantiate(textMesh, rigidbody.transform.position,
		                              rigidbody.transform.rotation) as GameObject;

		myMesh = text.GetComponent<TextMesh>();
		renderer.material.color = Color.gray;
	}

	public void gameOver() {
		Instantiate(gameOverSplode, transform.position, transform.rotation);
		myMesh.text = "";
		Destroy (this.gameObject);
	}

	//	Process getting shot by a bullet
	void OnTriggerEnter(Collider other) {
		if(other.tag != "OutOfBounds" && other.tag != "Player") {
			if(other.tag == "ClearBullet") {
				if(isMine) {
					//	YOU LOSE
					gameOver();
					manager.gameOver();
				}
				else if(tag != "Cleared") {
					//	Blow up the box and reveal num nearby mines
					Instantiate(standardSplode, transform.position, transform.rotation);

					int near = manager.getNearbyMines (this.gameObject);
					displayText(near.ToString());

					//	Set tag so bullets won't hit it
					tag = "Cleared";
				}
			}
			else if(other.tag == "FlagBullet") {

			}

			if(tag != "Cleared")
				Destroy (other.gameObject);
		}
	}

	//	Display number on box and remove the box
	private void displayText(string text) {
		renderer.enabled = false;
		myMesh.text = text;
	}
}
