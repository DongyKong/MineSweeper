using UnityEngine;
using System.Collections;

public class Gun : MonoBehaviour {

	public Transform shotSpawn;
	public GameObject clearShot;
	public GameObject flagShot;

	public float bulletImpulse;

	// Update is called once per frame
	void Update () {
		//	Shooting
		GameObject currShot = null;
		if (Input.GetButtonDown ("Fire1"))
			currShot = clearShot;
		else if(Input.GetButtonDown("Fire2"))
			currShot = flagShot;

		if (currShot != null) {
			GameObject bullet = Instantiate (currShot, shotSpawn.position, shotSpawn.rotation) as GameObject;

			bullet.GetComponent<BaseShot>().parent = this.transform.parent.gameObject;
			bullet.rigidbody.AddForce (shotSpawn.transform.forward * bulletImpulse, ForceMode.Impulse);
			audio.Play();
		}
	}
}
