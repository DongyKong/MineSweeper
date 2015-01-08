using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class OptionsMenu : MonoBehaviour {
	
	[SerializeField] Transform menuPanel;
	[SerializeField] GameObject sliderPrefab;
	[SerializeField] GameObject togglePrefab;
	[SerializeField] GameObject textFieldPrefab;

	private GameObject[] slidersAndToggle;
	private GameObject[] texts;

	public void OnClickBack() {
		Application.LoadLevel("MainMenu");
	}

	void Start() {

		slidersAndToggle = new GameObject[5];
		texts = new GameObject[5];

		int yCoord = 100;
		int sliderDecrement = 100;
		int textDecrement = 25;
		int screenWidth = Screen.width;
		for(int i = 0; i < 5; i++) {
			if(i == 4) {
				//	Instantiate the toggle
				GameObject tog = Instantiate(togglePrefab) as GameObject;
				tog.transform.SetParent(menuPanel, false);
				RectTransform togTransform = tog.GetComponent<RectTransform>();
				togTransform.anchoredPosition = new Vector2(screenWidth/2, yCoord);
				slidersAndToggle[i] = tog;

				continue;
			}

			//	Instantiate the slider
			GameObject obj = Instantiate(sliderPrefab) as GameObject;
			obj.transform.SetParent(menuPanel, false);
			RectTransform myTransform = obj.GetComponent<RectTransform>();
			myTransform.anchoredPosition = new Vector2(screenWidth/2, yCoord);
			slidersAndToggle[i] = obj;

			yCoord += textDecrement;

			//	Instantiate its text label and outline
			obj = Instantiate(textFieldPrefab) as GameObject;
			obj.transform.SetParent(menuPanel, false);
			obj.GetComponent<Text>().text = "";
			myTransform = obj.GetComponent<RectTransform>();
			myTransform.anchoredPosition = new Vector2(screenWidth/2, yCoord);
			texts[i] = obj;
			obj = Instantiate(textFieldPrefab) as GameObject;
			obj.transform.SetParent(menuPanel, false);
			obj.GetComponent<Text>().text = "";
			myTransform = obj.GetComponent<RectTransform>();
			myTransform.anchoredPosition = new Vector2(screenWidth/2, yCoord);
			texts[i] = obj;

			yCoord -= sliderDecrement;
		}

		//	SET INITIAL VALUES

		//	Number of boxes
		Slider init = slidersAndToggle[0].GetComponent<Slider>();
		init.maxValue = Preferences.numBoxOptions.Length - 1;
		init.wholeNumbers = true;
		init.value = Preferences.currNumBoxOption;

		//	Distance between boxes
		init = slidersAndToggle[1].GetComponent<Slider>();
		init.maxValue = 10;
		init.wholeNumbers = false;
		init.value = Preferences.distanceBetweenBoxes;

		//	Fraction that are mines
		init = slidersAndToggle[2].GetComponent<Slider>();
		init.maxValue = 1;
		init.wholeNumbers = false;
		init.value = Preferences.fractionMines;

		//	Number of 0 boxes to reveal
		init = slidersAndToggle[3].GetComponent<Slider>();
		init.maxValue = 20;
		init.wholeNumbers = true;
		init.value = Preferences.startAssistNumReveal;

		//	Toggle for start assist
		Toggle initTog = slidersAndToggle[4].GetComponent<Toggle>();
		initTog.isOn = Preferences.startAssist;
	}

	//	Constantly update from slider info
	void Update() {
		//	Number of boxes
		Slider updateSlider = slidersAndToggle[0].GetComponent<Slider>();
		Text updateText = texts[0].GetComponent<Text>();
		Preferences.currNumBoxOption = (int)updateSlider.value;
		Preferences.numBoxes = Preferences.numBoxOptions[Preferences.currNumBoxOption];
		updateText.text = "Grid size: " + Preferences.numBoxes;

		//	Distance between boxes
		updateSlider = slidersAndToggle[1].GetComponent<Slider>();
		updateText = texts[1].GetComponent<Text>();
		float roundIt = Mathf.Round(updateSlider.value * 10f) / 10f;
		Preferences.distanceBetweenBoxes = roundIt;
		updateText.text = "Distance between boxes: " + roundIt;

		//	Fraction that are mines
		updateSlider = slidersAndToggle[2].GetComponent<Slider>();
		updateText = texts[2].GetComponent<Text>();
		roundIt = Mathf.Round(updateSlider.value * 100f) / 100f;
		Preferences.fractionMines = roundIt;
		updateText.text = "Mine frequency: " + (roundIt*100) + "%";

		//	Number of 0 boxes to reveal
		updateSlider = slidersAndToggle[3].GetComponent<Slider>();
		updateText = texts[3].GetComponent<Text>();
		Preferences.startAssistNumReveal = (int)updateSlider.value;
		updateText.text = "# Revealed for start assist: " + updateSlider.value;

		//	Toggle for start assist
		Toggle updateTog = slidersAndToggle[4].GetComponent<Toggle>();
		Preferences.startAssist = updateTog.isOn;
	}
}
