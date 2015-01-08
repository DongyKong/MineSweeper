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
	public Text winText;
	private bool bRestart = false;
	private bool bGameOver = false;
	private bool bWin = false;

	public bool startAssist;
	public int startAssistNumReveal;

	public int remainingMines;
		
	// Use this for initialization
	void Start () {

		//	Pull info from Preferences
		numBoxes = Preferences.numBoxes;
		distanceBetweenBoxes = Preferences.distanceBetweenBoxes;
		fractionMines = Preferences.fractionMines;
		startAssist = Preferences.startAssist;
		startAssistNumReveal = Preferences.startAssistNumReveal;

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
				box.renderer.material.color = Color.gray;

				//	Decide if box has a mine
				if(Random.Range(0, 100) < (fractionMines*100)) {
					box.GetComponent<MineBox>().isMine = true;
					remainingMines++;
				}

				allBoxes[i, j] = box;

				locToSpawn.x -= (1 + distanceBetweenBoxes);
			}
			locToSpawn.y -= (1 + distanceBetweenBoxes);
			locToSpawn.x = offset;
		}

		mineText.text = "Remaining mines: " + remainingMines;

		//	Reveal 0 boxes if startAssist is on
		if (startAssist) {
			int numRevealed = 0;
			for (int i = 0; i < numPerRow; i++) {
				for (int j = 0; j < numPerRow; j++) {
					if (getNearbyMines(allBoxes[i, j]) == 0 && numRevealed < startAssistNumReveal &&
					    	!allBoxes[i, j].GetComponent<MineBox>().isMine) {

						allBoxes [i, j].renderer.material.color = Color.green;
						numRevealed++;
					}
				}
			}
		}
	}

	//	Check for restarting the game
	void Update() {
		if (bRestart && Input.GetKeyDown(KeyCode.R)) {
			Application.LoadLevel(Application.loadedLevel);
		}

		//	Check win condition
		bool win = true;
		for (int i = 0; i < allBoxes.GetLength(0); i++) {
			for(int j = 0; j < allBoxes.GetLength(1); j++) {
				if(allBoxes[i, j] != null) {
					MineBox script = allBoxes[i, j].GetComponent<MineBox>();
					if(script.isMine && script.myFireEffect == null)
						win = false;
				}
			}
		}

		if (win && gameOverText.text != "GAME OVER") {
			winText.text = "YOU WIN!";
			bWin = true;
			restartText.text = "Press 'R' to Restart";
			bRestart = true;
		}

		//	Check for player pressing 'Z'
		if(Input.GetKeyDown(KeyCode.Z)) {
			Screen.lockCursor = false;
			Application.LoadLevel("MainMenu");
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
		if(!bWin && !bGameOver) {
			gameOverText.text = "GAME OVER";
			bGameOver = true;
			StartCoroutine (blowUpEverything ());
		}
	}

	//	Game over, blow up everything
	IEnumerator blowUpEverything() {

		ArrayList toBlowUp = new ArrayList();

		for(int i = 0; i < allBoxes.GetLength(0); i++) {
			for(int j = 0; j < allBoxes.GetLength(1); j++) {
				toBlowUp.Add(allBoxes[i, j]);
			}
		}

		//	Generate random indices until list is empty
		int initCount = toBlowUp.Count;
		while(toBlowUp.Count > 0) {
			int rand = Random.Range(0, toBlowUp.Count - 1);
			GameObject box = (GameObject)toBlowUp[rand];
			if(box != null)
				box.GetComponent<MineBox>().gameOver();
			toBlowUp.RemoveAt(rand);

			yield return new WaitForSeconds(0.02f);

			//	Display 'R' after 10% boxes gone
			if(toBlowUp.Count == (int)(initCount*0.9)) {
				restartText.text = "Press 'R' to Restart";
				bRestart = true;
			}
		}

//		for(int i = 0; i < allBoxes.GetLength(0); i++) {
//			for(int j = 0; j < allBoxes.GetLength(1); j++) {
//				if(allBoxes[i, j] != null) {
//					allBoxes[i, j].GetComponent<MineBox>().gameOver();
//					yield return new WaitForSeconds(0.05f);
//				}
//			}
//
//			if(i == (int)allBoxes.GetLength(0)/4) {
//				restartText.text = "Press 'R' to Restart";
//				bRestart = true;
//			}
//		}
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
