using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour {
	
	public void OnClickPlay() {
		Application.LoadLevel("Main");
	}

	public void OnClickOptions() {
		Application.LoadLevel("OptionsMenu");
	}

	public void OnClickQuit() {
		Application.Quit();
	}

	public void OnClickControls() {
		Application.LoadLevel("ControlsMenu");
	}
}
