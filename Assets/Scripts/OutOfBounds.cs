using UnityEngine;
using System.Collections;

public class OutOfBounds : MonoBehaviour {

	void OnTriggerExit(Collider col) {
		if(col.gameObject.tag != "Player")
			Destroy (col.gameObject);
	}
}
