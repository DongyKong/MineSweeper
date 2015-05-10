using UnityEngine;
using System.Collections;

public class EnemyScript : MonoBehaviour {

	public GameObject mineManager;
	private MineManager manager;
	private GameObject[] allMines;

	public GameObject worldBounds;
	public GameObject[] shotTypes;

	public float moveSpeed;
	public float attackRange;
	public float maxAttackSpeed;
	public float shotSpeed;

	private float currAttackSpeed;
	private float attackTimer;

	public float disableTime;
	private bool disabled;

	private Vector3 destination;

	void Start () {
		destination = findRandomDestination ();

		manager = mineManager.GetComponent<MineManager> ();
		Invoke ("getTheMines", 5f);

		attackTimer = 0;
		disabled = false;
		currAttackSpeed = Random.Range (0, maxAttackSpeed);
	}

	void Update () {
		//	It's been shot by player
		if (disabled)
			return;

		attackTimer += Time.deltaTime;
		this.transform.position = Vector3.MoveTowards (this.transform.position,
		                                              destination,
		                                              moveSpeed * Time.deltaTime);

		if (allMines == null || manager.bGameOver)
			return;

		//	Check if reached destination
		if (Vector3.Distance (this.transform.position, destination) < 1f)
						destination = findRandomDestination ();

		if (attackTimer >= currAttackSpeed) {
			shootRandomBox();
		}
	}

	void shootRandomBox() {
		//	Pick a random box that isn't cleared
		GameObject target = allMines [Random.Range (0, allMines.Length)];
		while(target.GetComponent<MineBox>().cleared)
			target = allMines [Random.Range (0, allMines.Length)];

		//	Check if target in attack range
		if(Vector3.Distance(this.transform.position, target.transform.position) <= attackRange)
		{
			//	Pick a random shot type
			GameObject shotType = shotTypes[Random.Range(0, shotTypes.Length)];
			GameObject shot = Instantiate(shotType, this.transform.position, Quaternion.identity) as GameObject;
			shot.GetComponent<BaseShot>().parent = this.gameObject;

			//	Rotate to face box and kick it
			Vector3 forceDir = target.transform.position - this.transform.position;
			shot.transform.rotation = Quaternion.LookRotation(-forceDir);
			shot.rigidbody.AddForce(forceDir.normalized * shotSpeed, ForceMode.Impulse);
			
			attackTimer = 0;
			currAttackSpeed = Random.Range (0, maxAttackSpeed);
		}
	}

	Vector3 findRandomDestination() {
		Bounds bounds = worldBounds.collider.bounds;

		//	Get random point inside bounds
		Vector3 point;
		float buff = 5f;
		point.x = Random.Range (bounds.min.x + buff, bounds.max.x - buff);
		point.y = Random.Range (bounds.min.y + buff, bounds.max.y - buff);
		point.z = Random.Range (bounds.min.z + buff, bounds.max.z - buff);

		return point;
	}

	void getTheMines() {
		allMines = manager.getAllMines ();
	}

	public void getDisabled() {
		if(!disabled) {
			disabled = true;
			Invoke ("resetDisable", disableTime);
		}
	}
	void resetDisable() {
		disabled = false;
	}
}
