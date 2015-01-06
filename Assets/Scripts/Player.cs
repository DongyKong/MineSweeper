using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	public float moveSpeed;
	public float mouseSensitivity;
	
	public float upDownRange;
	float vertRote = 0.0f;
	float horizRote = 0.0f;
	
	void Start() {
		Screen.lockCursor = true;
	}
	
	void Update() {
		
		//	Left/Right rotation
		horizRote += Input.GetAxis ("Mouse X") * mouseSensitivity;
		//transform.Rotate (0.0f, roteLeftRight, 0.0f);
		
		//	Up/Down rotation
		vertRote -= Input.GetAxis ("Mouse Y") * mouseSensitivity;
		vertRote = Mathf.Clamp (vertRote, -upDownRange, upDownRange);
		transform.localRotation = Quaternion.Euler (vertRote, horizRote, 0.0f);
		
		//	Movement
		float forwardSpeed = Input.GetAxis ("Vertical");
		float sideSpeed = Input.GetAxis ("Horizontal");
		float riseSpeed = Input.GetAxis ("Rise");
		
		CharacterController cc = GetComponent<CharacterController> ();
		
		Vector3 speed = new Vector3 (sideSpeed * moveSpeed, riseSpeed * moveSpeed, forwardSpeed * moveSpeed);
		
		speed = transform.rotation * speed;
		
		cc.Move (speed * Time.deltaTime);
		
	}
}
