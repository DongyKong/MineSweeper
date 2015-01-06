using UnityEngine;
using System.Collections;

public class Crosshair : MonoBehaviour {

	public Texture2D crosshair;
	public Rect position;
	
	void Start () {
		//	Location to draw crosshair
		position = new Rect((Screen.width - crosshair.width) / 2,
		                (Screen.height - crosshair.height) / 2,
		                crosshair.width, crosshair.height);
	}

	void OnGUI() {
		GUI.DrawTexture(position, crosshair);
	}
}
