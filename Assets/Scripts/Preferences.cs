using UnityEngine;
using System.Collections;

public static class Preferences {
	public static int[] numBoxOptions = { 25, 36, 49, 64, 81, 100, 121, 144, 169, 196, 225, 256, 289, 324, 361, 400};
	public static int currNumBoxOption;

	public static int numBoxes = 100;
	public static float distanceBetweenBoxes = 0.5f;
	public static float fractionMines = 0.25f;

	public static bool startAssist = true;
	public static int startAssistNumReveal = 10;
}
