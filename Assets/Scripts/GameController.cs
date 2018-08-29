using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour {

	public PlayerStats mPlayerStats;
	public GridLayoutGroup gridGrpBg;

	int hzDivCount = 6;
	int vtDivCount = 8;

	// Use this for initialization
	void Start () {
		float Hz = Screen.width;
		float Vt = Screen.height;

		float gridSizeHz = Hz / hzDivCount;
		float gridSizeVt = Vt / vtDivCount;

		mPlayerStats.grid.sizeInPixel = new Vector2(gridSizeHz, gridSizeVt);
		mPlayerStats.grid.rows = hzDivCount;
		mPlayerStats.grid.columns = vtDivCount;

		gridGrpBg.cellSize = mPlayerStats.grid.sizeInPixel;
	}
}
