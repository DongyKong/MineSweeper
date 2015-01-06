using UnityEngine;
using System.Collections;

public class MineBox : MonoBehaviour {

	public GameObject textMesh;
	private TextMesh myTextMesh;

	public GameObject standardSplode;
	public GameObject gameOverSplode;
	public GameObject fireEffect;
	private GameObject myFireEffect;

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

		myTextMesh = text.GetComponent<TextMesh>();
		renderer.material.color = Color.gray;
	}

	public void gameOver() {
		Instantiate(gameOverSplode, transform.position, transform.rotation);
		myTextMesh.text = "";
		Destroy (this.gameObject);
		Destroy (myFireEffect);
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
					clearBox(true);
				}
			}
			else if(other.tag == "FlagBullet") {
				if(renderer.material.color == Color.gray) {
					if(myTextMesh.text != "?") {
						//	Set fire (flagged as having mine)
						myFireEffect = Instantiate(fireEffect, transform.position, transform.rotation) as GameObject;
						renderer.material.color = Color.red;
					}
					else {
						//	Return to unmarked and unflagged
						myTextMesh.text = "";
					}
				}
				else {
					//	Remove flame and change back to gray
					renderer.material.color = Color.gray;
					if(myFireEffect != null)
						Destroy(myFireEffect);

					//	Mark with question mark
					myTextMesh.text = "?";
				}
			}

			if(tag != "Cleared")
				Destroy (other.gameObject);
		}
	}

	//	Clear the box
	public void clearBox(bool recurse) {	//	recurse = 'false' avoids infinite recursion with chainClear()
		//	Blow up the box and reveal num nearby mines
		Instantiate(standardSplode, transform.position, transform.rotation);
		
		int near = manager.getNearbyMines (this.gameObject);

		if (near == 0) {
			if (recurse)
				manager.chainClear (this.gameObject);
		}
		else
			displayText(near.ToString());
		renderer.enabled = false;
		
		//	Set tag so bullets won't hit it
		tag = "Cleared";
	}

	//	Display number on box and remove the box
	private void displayText(string text) {
		myTextMesh.text = text;
	}
}
