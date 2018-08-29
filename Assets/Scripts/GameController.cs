using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

	public PlayerStats mPlayerStats;
	float gridSizeHz;

	int hzDivCount = 6;
	int vtDivCount = 8;

	// Use this for initialization
	void Start () {
		float Hz = Screen.width;
		float Vt = Screen.height;

		gridSizeHz = Hz / hzDivCount;

		float gridSizeVt = Vt / vtDivCount;

		mPlayerStats.grid.sizeInPixel = new Vector2(gridSizeHz, gridSizeVt);
		mPlayerStats.grid.rows = hzDivCount;
		mPlayerStats.grid.columns = vtDivCount;
	}
}
