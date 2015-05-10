using UnityEngine;
using System.Collections;

public class TimeOut : MonoBehaviour {

	public float timeOut;
	private float currTime;

	void Start () {
		currTime = 0;
	}

	void Update () {
		currTime += Time.deltaTime;
		if (currTime >= timeOut)
			Destroy (this.gameObject);
	}
}
