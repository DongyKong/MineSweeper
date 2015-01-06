using UnityEngine;
using System.Collections;

public class Gun : MonoBehaviour {

	public Transform shotSpawn;
	public GameObject clearShot;
	public GameObject flagShot;

	public float bulletImpulse;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		//	Destroy mine box
		if (Input.GetButtonDown ("Fire1")) {
			GameObject bullet = Instantiate(clearShot, shotSpawn.position, shotSpawn.rotation) as GameObject;
			bullet.rigidbody.AddForce(shotSpawn.transform.forward * bulletImpulse, ForceMode.Impulse);
		}

		//	Flag mine box
		if(Input.GetButtonDown ("Fire2")) {
			GameObject bullet = Instantiate(flagShot, shotSpawn.position, shotSpawn.rotation) as GameObject;
			bullet.rigidbody.AddForce(shotSpawn.transform.forward * bulletImpulse, ForceMode.Impulse);
		}
	}
}
