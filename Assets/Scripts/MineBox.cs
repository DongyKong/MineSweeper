using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class MineBox : MonoBehaviour {

	public GameObject textMesh;
	private TextMesh myTextMesh;

	public GameObject standardSplode;
	public GameObject gameOverSplode;
	public GameObject fireEffect;
	public GameObject myFireEffect;

	public AudioClip clearSplodeFX;
	public AudioClip loseSplodeFX;
	public AudioClip fireSplodeFX;

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
	}

	public void gameOver() {
		Instantiate(gameOverSplode, transform.position, transform.rotation);
		audio.volume = 1.0f;
		AudioSource.PlayClipAtPoint(loseSplodeFX, transform.position);
		myTextMesh.text = "";
		Destroy (this.gameObject);
		Destroy (myFireEffect);
	}

	//	Process getting shot by a bullet
	void OnTriggerEnter(Collider other) {
		if(other.tag != "OutOfBounds" && other.tag != "Player") {
			if(other.tag == "ClearBullet" && renderer.material.color != Color.red) {
				//	Put out the fire
				Destroy (myFireEffect);

				if(isMine) {
					//	YOU LOSE
					gameOver();
					manager.gameOver();
				}
				else if(tag != "Cleared") {
					clearBox(true);
				}
			}
			else if(other.tag == "FlagBullet" && tag != "Cleared") {
				if(renderer.material.color == Color.gray || renderer.material.color == Color.green) {
					if(myTextMesh.text != "?") {
						//	Set fire (flagged as having mine)
						myFireEffect = Instantiate(fireEffect, transform.position, transform.rotation) as GameObject;
						renderer.material.color = Color.red;

						//	Update remaining mines
						manager.remainingMines--;
						manager.mineText.text = "Remaining mines: " + manager.remainingMines;
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

					//	Update remaining mines
					manager.remainingMines++;
					manager.mineText.text = "Remaining mines: " + manager.remainingMines;
				}
				audio.volume = 0.3f;
				audio.PlayOneShot(fireSplodeFX);
			}

			if(tag != "Cleared")
				Destroy (other.gameObject);
		}
	}

	//	Clear the box
	//		recurse = 'false' avoids infinite recursion with chainClear()
	public void clearBox(bool recurse) {
		Destroy(myFireEffect);

		//	Blow up the box and reveal num nearby mines
		Instantiate(standardSplode, transform.position, transform.rotation);
		audio.volume = 0.2f;
		audio.PlayOneShot(clearSplodeFX);
		
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
		collider.enabled = false;
	}

	//	Display number on box and remove the box
	private void displayText(string text) {
		myTextMesh.text = text;
	}
}
