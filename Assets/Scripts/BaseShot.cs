using UnityEngine;
using System.Collections;

public class BaseShot : MonoBehaviour {

	public GameObject parent;

	public enum SHOT_TYPE {
		CLEAR,
		FLAG
	}

	public SHOT_TYPE myType;

	void OnTriggerEnter(Collider col) {
		if (parent == col.gameObject)
			return;

		if(col.gameObject.tag == "Mine") {
			if(!col.gameObject.GetComponent<MineBox>().cleared)
				col.GetComponent<MineBox>().hitByShot(myType);
		}
		else if(col.gameObject.tag == "Enemy") {
			col.gameObject.GetComponent<EnemyScript>().getDisabled();
		}

		if (col.gameObject.tag != "OutOfBounds")
		{
			Destroy (this.gameObject);
		}
	}
}
