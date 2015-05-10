using UnityEngine;
using System.Collections;

public class ControlsMenu : MonoBehaviour {

	public void OnClickBack() {
		Application.LoadLevel("MainMenu");
	}
}
