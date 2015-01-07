using UnityEngine;
using System.Collections;

public static class Preferences {
	public static int[] numBoxOptions = { 25, 36, 49, 64, 81, 100, 121, 144, 169, 196, 225, 256, 289, 324, 361, 400};
	public static int currNumBoxOption;

	public static int numBoxes;
	public static float distanceBetweenBoxes;
	public static float fractionMines;

	public static bool startAssist;
	public static int startAssistNumReveal;
}
