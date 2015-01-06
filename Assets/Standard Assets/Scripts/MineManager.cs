using UnityEngine;
using System.Collections;

public class MineManager : MonoBehaviour {

	public GameObject mineToSpawn;

	public int numMines;
	public int distanceBetweenMines;

	// Use this for initialization
	void Start () {
		//	Generate mines
		int numPerRow = (int)Mathf.Sqrt (numMines);

		float offset = numPerRow / 2 + (numPerRow / 2) * distanceBetweenMines;
		Vector3 locToSpawn = new Vector3 (offset, offset, 0.0f);
		for (int i = 0; i < numPerRow; i++) {
			for(int j = 0; j < numPerRow; j++) {
				Quaternion rote = Quaternion.identity;
				Instantiate(mineToSpawn, locToSpawn, rote);

				locToSpawn.x -= (1 + distanceBetweenMines);
			}
			locToSpawn.y -= (1 + distanceBetweenMines);
			locToSpawn.x = offset;
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
