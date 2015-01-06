using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MineManager : MonoBehaviour {

	public GameObject boxToSpawn;
	private GameObject[,] allBoxes;

	public int numBoxes;
	public float distanceBetweenBoxes;
	public float fractionMines;

	public Text mineText;
	public Text restartText;
	public Text gameOverText;
	bool bRestart = false;

	int remainingMines;

	// Use this for initialization
	void Start () {
		//	Generate mines
		int numPerRow = (int)Mathf.Sqrt (numBoxes);
		allBoxes = new GameObject[numPerRow, numPerRow];

		float offset = numPerRow / 2 + (numPerRow / 2) * distanceBetweenBoxes;
		Vector3 locToSpawn = new Vector3 (offset, offset, 0.0f);
		remainingMines = 0;
		for (int i = 0; i < numPerRow; i++) {
			for(int j = 0; j < numPerRow; j++) {
				Quaternion rote = Quaternion.identity;
				GameObject box = Instantiate(boxToSpawn, locToSpawn, rote) as GameObject;

				//	Decide if box has a mine
				if(Random.Range(0, 100) < (fractionMines*100)) {
					box.GetComponent<MineBox>().isMine = true;
//					box.GetComponent<MineBox>().renderer.material.color = Color.red;
					remainingMines++;
				}

				allBoxes[i, j] = box;

				locToSpawn.x -= (1 + distanceBetweenBoxes);
			}
			locToSpawn.y -= (1 + distanceBetweenBoxes);
			locToSpawn.x = offset;
		}

		mineText.text = "Remaining mines: " + remainingMines;
	}

	//	Check for restarting the game
	void Update() {
		if (bRestart && Input.GetKeyDown(KeyCode.R)) {
			Application.LoadLevel(Application.loadedLevel);
		}
	}

	//	Clear all nearby boxes with 0s if player shoots a box with 0
	public void chainClear(GameObject startingBox) {
		ArrayList neighbors = getNeighbors (startingBox);
		for (int i = 0; i < neighbors.Count; i++) {
			GameObject neighbor = (GameObject)neighbors[i];

			if(neighbor.GetComponent<MineBox>().isMine)
				continue;

			neighbor.GetComponent<MineBox>().clearBox(false);

			//	Recurse if also a 0 box
			if(getNearbyMines(neighbor) == 0)
				chainClear(neighbor);
		}
	}

	//	Initiate game over
	public void gameOver() {
		gameOverText.text = "GAME OVER";
		StartCoroutine (blowUpEverything ());
	}

	//	Game over, blow up everything
	IEnumerator blowUpEverything() {

		for(int i = 0; i < allBoxes.GetLength(0); i++) {
			for(int j = 0; j < allBoxes.GetLength(1); j++) {
				if(allBoxes[i, j] != null) {
					allBoxes[i, j].GetComponent<MineBox>().gameOver();
					yield return new WaitForSeconds(0.05f);
				}
			}

			if(i == (int)allBoxes.GetLength(0)/4) {
				restartText.text = "Press 'R' to Restart";
				bRestart = true;
			}
		}
	}

	//	Get number of neighboring boxes with mines
	public int getNearbyMines(GameObject box) {
		int nearbyMines = 0;

		ArrayList neighbors = getNeighbors (box);
		for (int i = 0; i < neighbors.Count; i++) {
			GameObject neighbor = (GameObject)neighbors[i];

			if(neighbor.GetComponent<MineBox>().isMine)
				nearbyMines++;
		}

		return nearbyMines;
	}

	//	Return all valid neighbors of a box
	private ArrayList getNeighbors(GameObject box) {
		ArrayList neighbors = new ArrayList();

		for (int i = 0; i < allBoxes.GetLength(0); i++) {
			for(int j = 0; j < allBoxes.GetLength(1); j++) {
				if(allBoxes[i, j] == box) {
					//	Check all neighbors
					int a = i-1, b = j-1;
					
					while(a <= i + 1) {
						while(b <= j + 1) {
							//	Make sure indices are within array and not the box that was shot
							if(a >= 0 && b >= 0 && a < allBoxes.GetLength(0) && b < allBoxes.GetLength(0)
							   && (a != i || b != j)) {
								
								if(allBoxes[a, b] != null && allBoxes[a, b].GetComponent<MineBox>().tag != "Cleared")
									neighbors.Add(allBoxes[a, b]);
							}
							b++;
						}
						a++;
						b = j-1;
					}
				}
			}
		}

		return neighbors;
	}
}
