using UnityEngine;
using System.Collections;

public class OutOfBounds : MonoBehaviour {

	void OnTriggerExit(Collider other) {
		Destroy (other.gameObject);
	}
}
